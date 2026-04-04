"""
Gmail'i polling yaparak CI failure e-postalarına gelen yanıtları işler.
Yanıt bulunursa ci-fix.yml workflow'unu tetikler.

Ortam değişkenleri:
  GMAIL_CLIENT_ID
  GMAIL_CLIENT_SECRET
  GMAIL_REFRESH_TOKEN
  GH_PAT             – GitHub Personal Access Token (repo scope)
  GITHUB_REPOSITORY  – owner/repo
"""

import base64
import json
import os
import re
import sys
import urllib.request
import urllib.parse

# ---------------------------------------------------------------------------
# Gmail OAuth token yenileme
# ---------------------------------------------------------------------------

def refresh_access_token() -> str:
    """OAuth refresh token kullanarak yeni access token alır."""
    data = urllib.parse.urlencode({
        "client_id": os.environ["GMAIL_CLIENT_ID"],
        "client_secret": os.environ["GMAIL_CLIENT_SECRET"],
        "refresh_token": os.environ["GMAIL_REFRESH_TOKEN"],
        "grant_type": "refresh_token",
    }).encode()

    req = urllib.request.Request(
        "https://oauth2.googleapis.com/token",
        data=data,
        method="POST",
    )
    with urllib.request.urlopen(req) as resp:
        result = json.loads(resp.read())
    return result["access_token"]


# ---------------------------------------------------------------------------
# Gmail API helpers
# ---------------------------------------------------------------------------

def gmail_get(path: str, token: str) -> dict:
    req = urllib.request.Request(
        f"https://gmail.googleapis.com/gmail/v1/{path}",
        headers={"Authorization": f"Bearer {token}"},
    )
    with urllib.request.urlopen(req) as resp:
        return json.loads(resp.read())


def gmail_post(path: str, token: str, body: dict) -> dict:
    data = json.dumps(body).encode()
    req = urllib.request.Request(
        f"https://gmail.googleapis.com/gmail/v1/{path}",
        data=data,
        headers={
            "Authorization": f"Bearer {token}",
            "Content-Type": "application/json",
        },
        method="POST",
    )
    with urllib.request.urlopen(req) as resp:
        return json.loads(resp.read())


def search_messages(token: str, query: str) -> list[str]:
    """Verilen sorguyla eşleşen mesaj ID'lerini döndürür."""
    path = f"users/me/messages?q={urllib.parse.quote(query)}&maxResults=10"
    result = gmail_get(path, token)
    return [m["id"] for m in result.get("messages", [])]


def get_message(token: str, msg_id: str) -> dict:
    return gmail_get(f"users/me/messages/{msg_id}?format=full", token)


def mark_as_read(token: str, msg_id: str) -> None:
    gmail_post(
        f"users/me/messages/{msg_id}/modify",
        token,
        {"removeLabelIds": ["UNREAD"]},
    )


def decode_body(msg: dict) -> str:
    """E-posta gövdesini plain text olarak çözer."""
    payload = msg.get("payload", {})

    def extract_text(part: dict) -> str:
        mime = part.get("mimeType", "")
        body_data = part.get("body", {}).get("data", "")
        if mime == "text/plain" and body_data:
            return base64.urlsafe_b64decode(body_data).decode("utf-8", errors="replace")
        for sub in part.get("parts", []):
            result = extract_text(sub)
            if result:
                return result
        return ""

    return extract_text(payload)


def get_header(msg: dict, name: str) -> str:
    headers = msg.get("payload", {}).get("headers", [])
    for h in headers:
        if h["name"].lower() == name.lower():
            return h["value"]
    return ""


# ---------------------------------------------------------------------------
# Parsing helpers
# ---------------------------------------------------------------------------

def parse_subject(subject: str) -> tuple[str, str] | None:
    """
    '[CI FAILURE] AktuelUrunBulucu | RUN:12345678 | COMPONENT:backend'
    'Re: [CI FAILURE] ...' formatından (run_id, component) çıkarır.
    """
    match = re.search(r"RUN:(\d+).*?COMPONENT:(backend|frontend|deploy)", subject)
    if match:
        return match.group(1), match.group(2)
    return None


def parse_choice(body: str) -> int | None:
    """Yanıt gövdesinden 1, 2 veya 3 seçimini okur."""
    # İlk satırda veya metnin başında "1", "2" ya da "3" olan cevabı bul
    for line in body.strip().splitlines():
        line = line.strip()
        if line in ("1", "2", "3"):
            return int(line)
    # Daha geniş arama
    match = re.search(r"(?<!\d)([123])(?!\d)", body[:200])
    if match:
        return int(match.group(1))
    return None


# ---------------------------------------------------------------------------
# GitHub Actions – artifact download
# ---------------------------------------------------------------------------

def download_solutions_artifact(run_id: str, token: str, repo: str) -> dict | None:
    """run_id için solutions artifact JSON içeriğini indirir."""
    api_url = f"https://api.github.com/repos/{repo}/actions/runs/{run_id}/artifacts"
    req = urllib.request.Request(
        api_url,
        headers={
            "Authorization": f"Bearer {token}",
            "Accept": "application/vnd.github+json",
            "X-GitHub-Api-Version": "2022-11-28",
        },
    )
    try:
        with urllib.request.urlopen(req) as resp:
            data = json.loads(resp.read())
    except Exception as exc:
        print(f"Could not list artifacts: {exc}", file=sys.stderr)
        return None

    artifact_name = f"solutions-{run_id}"
    artifact = next(
        (a for a in data.get("artifacts", []) if a["name"] == artifact_name), None
    )
    if not artifact:
        print(f"Artifact '{artifact_name}' not found.", file=sys.stderr)
        return None

    # Download URL al
    dl_url = artifact["archive_download_url"]
    req2 = urllib.request.Request(
        dl_url,
        headers={
            "Authorization": f"Bearer {token}",
            "Accept": "application/vnd.github+json",
        },
    )
    import io
    import zipfile

    with urllib.request.urlopen(req2) as resp:
        zip_bytes = resp.read()

    with zipfile.ZipFile(io.BytesIO(zip_bytes)) as zf:
        with zf.open("solutions.json") as f:
            return json.loads(f.read())


# ---------------------------------------------------------------------------
# GitHub Actions – workflow_dispatch tetikleme
# ---------------------------------------------------------------------------

def trigger_fix_workflow(
    solution: dict,
    component: str,
    run_id: str,
    repo: str,
    gh_token: str,
) -> None:
    """ci-fix.yml workflow'unu seçilen çözümle tetikler."""
    url = f"https://api.github.com/repos/{repo}/actions/workflows/ci-fix.yml/dispatches"
    payload = json.dumps({
        "ref": "main",
        "inputs": {
            "solution_json": json.dumps(solution, ensure_ascii=False),
            "component": component,
            "original_run_id": run_id,
        },
    }).encode()
    req = urllib.request.Request(
        url,
        data=payload,
        headers={
            "Authorization": f"Bearer {gh_token}",
            "Accept": "application/vnd.github+json",
            "X-GitHub-Api-Version": "2022-11-28",
            "Content-Type": "application/json",
        },
        method="POST",
    )
    with urllib.request.urlopen(req) as resp:
        status = resp.status
    print(f"workflow_dispatch HTTP status: {status}")


# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------

def main() -> None:
    required = ["GMAIL_CLIENT_ID", "GMAIL_CLIENT_SECRET", "GMAIL_REFRESH_TOKEN",
                "GH_PAT", "GITHUB_REPOSITORY"]
    missing = [k for k in required if not os.environ.get(k)]
    if missing:
        print(f"Missing env vars: {missing}", file=sys.stderr)
        sys.exit(1)

    repo = os.environ["GITHUB_REPOSITORY"]
    gh_token = os.environ["GH_PAT"]

    print("Refreshing Gmail access token...")
    gmail_token = refresh_access_token()

    query = 'subject:"Re: [CI FAILURE]" is:unread from:me'
    print(f"Searching Gmail: {query}")
    msg_ids = search_messages(gmail_token, query)
    print(f"Found {len(msg_ids)} unread reply(s).")

    if not msg_ids:
        print("No replies to process.")
        return

    # En yeni yanıtı işle (ilk = en yeni)
    msg_id = msg_ids[0]
    msg = get_message(gmail_token, msg_id)

    subject = get_header(msg, "Subject")
    print(f"Processing: {subject}")

    parsed = parse_subject(subject)
    if not parsed:
        print(f"Could not parse run_id/component from subject: {subject}", file=sys.stderr)
        mark_as_read(gmail_token, msg_id)
        return

    run_id, component = parsed
    print(f"run_id={run_id}, component={component}")

    body = decode_body(msg)
    choice = parse_choice(body)
    if not choice:
        print(f"Could not parse choice (1/2/3) from body: {body[:100]!r}", file=sys.stderr)
        mark_as_read(gmail_token, msg_id)
        return

    print(f"User chose option: {choice}")

    # solutions.json artifact'ını indir
    solutions_data = download_solutions_artifact(run_id, gh_token, repo)
    if not solutions_data:
        print("Could not download solutions artifact.", file=sys.stderr)
        mark_as_read(gmail_token, msg_id)
        return

    solutions = solutions_data.get("solutions", [])
    selected = next((s for s in solutions if s["id"] == choice), None)
    if not selected:
        print(f"Solution id={choice} not found in artifact.", file=sys.stderr)
        mark_as_read(gmail_token, msg_id)
        return

    print(f"Selected solution: {selected['description']}")

    # E-postayı okundu işaretle
    mark_as_read(gmail_token, msg_id)

    # ci-fix.yml'i tetikle
    trigger_fix_workflow(selected, component, run_id, repo, gh_token)
    print("ci-fix.yml triggered successfully.")


if __name__ == "__main__":
    main()
