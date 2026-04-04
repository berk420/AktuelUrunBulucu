"""
Hata logunu Claude API'a göndererek o hataya özel 2-3 çözüm önerisi üretir.
Çıktı: /tmp/solutions.json

Ortam değişkenleri:
  ANTHROPIC_API_KEY  – Claude API anahtarı
  COMPONENT          – backend | frontend | deploy
  RUN_ID             – GitHub Actions run ID
"""

import json
import os
import sys

import anthropic

COMPONENT = os.environ.get("COMPONENT", "backend")
RUN_ID = os.environ.get("RUN_ID", "0")

ERROR_LOG_PATH = "/tmp/error.log"


def read_error_log() -> str:
    """Hata logunu okur, yoksa varsayılan mesaj döndürür."""
    try:
        with open(ERROR_LOG_PATH, encoding="utf-8", errors="replace") as f:
            content = f.read().strip()
        if not content:
            return "Hata logu boş."
        # Son 150 satırı al (token limitini aşmamak için)
        lines = content.splitlines()
        return "\n".join(lines[-150:])
    except FileNotFoundError:
        return "Hata logu bulunamadı."


COMPONENT_CONTEXT = {
    "backend": "ASP.NET Core 10 Web API (.NET 10, C#, EF Core, PostgreSQL)",
    "frontend": "React 18 + Vite + TypeScript + ESLint",
    "deploy": "Windows Server deploy (self-hosted GitHub Actions runner, IIS/Windows Service, Nginx)",
}

SYSTEM_PROMPT = """Sen bir CI/CD ve yazılım geliştirme uzmanısın.
Sana bir CI/CD hata logu ve proje bilgisi verilecek.
Hatayı analiz edip 2 veya 3 adet somut, uygulanabilir çözüm önerisi üret.

Kurallar:
- Her çözüm gerçekten o hatayı çözmeli, genel tavsiye olmamalı
- Açıklamalar kısa ve Türkçe olmalı (max 1 cümle)
- "run_commands" tipindeki çözümler için komutlar bash/shell formatında olmalı
- "git_revert" tipi için commands dizisi boş olabilir
- "manual" tipi kullanıcıya rehberlik eder, komut çalıştırılmaz
- Kesinlikle sadece geçerli JSON döndür, başka hiçbir şey yazma

Çıktı formatı (kesinlikle bu formata uyman gerekiyor):
{
  "solutions": [
    {
      "id": 1,
      "description": "Kısa Türkçe açıklama",
      "type": "run_commands",
      "commands": ["komut1", "komut2"]
    },
    {
      "id": 2,
      "description": "Kısa Türkçe açıklama",
      "type": "git_revert",
      "commands": []
    }
  ]
}

type değerleri: "run_commands" | "git_revert" | "manual"
"""


def generate_solutions(error_log: str) -> dict:
    """Claude API'ya hata logunu gönderir ve çözüm önerileri alır."""
    client = anthropic.Anthropic()

    context = COMPONENT_CONTEXT.get(COMPONENT, COMPONENT)
    user_message = f"""Proje bileşeni: {context}

Hata logu:
```
{error_log}
```

Bu hatayı analiz et ve 2-3 adet spesifik çözüm önerisi üret. Sadece JSON döndür."""

    message = client.messages.create(
        model="claude-sonnet-4-6",
        max_tokens=1024,
        system=SYSTEM_PROMPT,
        messages=[{"role": "user", "content": user_message}],
    )

    response_text = message.content[0].text.strip()

    # JSON bloğu varsa içini al
    if "```json" in response_text:
        response_text = response_text.split("```json")[1].split("```")[0].strip()
    elif "```" in response_text:
        response_text = response_text.split("```")[1].split("```")[0].strip()

    return json.loads(response_text)


def fallback_solutions() -> dict:
    """Claude API'ya ulaşılamazsa temel çözümler döndürür."""
    return {
        "solutions": [
            {
                "id": 1,
                "description": "Son commit'i geri al, çalışan versiyona dön",
                "type": "git_revert",
                "commands": [],
            },
            {
                "id": 2,
                "description": "Workflow'u tekrar çalıştır (geçici hata ise düzelir)",
                "type": "manual",
                "commands": [],
            },
        ]
    }


def main() -> None:
    error_log = read_error_log()
    print(f"Component: {COMPONENT}")
    print(f"Run ID: {RUN_ID}")
    print(f"Error log ({len(error_log)} chars) loaded.")

    try:
        solutions = generate_solutions(error_log)
        # id alanlarını sıraya koy
        for i, sol in enumerate(solutions.get("solutions", []), start=1):
            sol["id"] = i
        print(f"Generated {len(solutions['solutions'])} solutions via Claude API.")
    except Exception as exc:
        print(f"Claude API error: {exc}", file=sys.stderr)
        print("Falling back to default solutions.")
        solutions = fallback_solutions()

    output_path = "/tmp/solutions.json"
    with open(output_path, "w", encoding="utf-8") as f:
        json.dump(solutions, f, ensure_ascii=False, indent=2)

    print(f"Solutions written to {output_path}")
    for sol in solutions["solutions"]:
        print(f"  {sol['id']}. [{sol['type']}] {sol['description']}")


if __name__ == "__main__":
    main()
