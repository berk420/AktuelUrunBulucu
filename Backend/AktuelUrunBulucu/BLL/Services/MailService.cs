using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace AktuelUrunBulucu.BLL.Services;

public class MailService : IMailService
{
    private readonly IConfiguration _config;

    public MailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendNotificationConfirmationAsync(string toEmail, string productName)
    {
        var smtp = _config.GetSection("Smtp");
        var host = smtp["Host"]!;
        var port = int.Parse(smtp["Port"]!);
        var username = smtp["Username"]!;
        var password = smtp["Password"]!;
        var fromAddress = smtp["FromAddress"]!;
        var fromName = smtp["FromName"]!;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromAddress));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = $"'{productName}' ürünü hakkında bildirim talebiniz";

        message.Body = new TextPart("html")
        {
            Text = $"""
                <div style="font-family: sans-serif; max-width: 480px; margin: 0 auto;">
                  <h2 style="color: #111827;">Aktüel Ürün Bulucu</h2>
                  <p>Merhaba,</p>
                  <p>
                    <strong>"{productName}"</strong> ürünü şu an zincir marketlerde bulunmamaktadır.
                    Ürün stoklara girdiğinde bu adrese bildirim gönderilecektir.
                  </p>
                  <p style="color: #6b7280; font-size: 13px;">Bu isteği siz oluşturmadıysanız bu maili dikkate almayınız.</p>
                </div>
            """
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(username, password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
