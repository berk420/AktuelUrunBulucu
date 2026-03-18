namespace AktuelUrunBulucu.BLL.Services;

public interface IMailService
{
    Task SendNotificationConfirmationAsync(string toEmail, string productName);
}
