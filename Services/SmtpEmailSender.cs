using System.Net;
using System.Net.Mail;
using SpaceMission.Interfaces;

namespace SpaceMission.Services;

public class SmtpEmailSender : IReportSender
{
    private readonly string senderEmail;
    private readonly string senderPassword;
    private readonly string receiverEmail;
    private readonly string host;
    private readonly int port;

    public SmtpEmailSender(
        string senderEmail,
        string senderPassword,
        string receiverEmail,
        string host = "smtp.gmail.com",
        int port = 587)
    {
        this.senderEmail = senderEmail;
        this.senderPassword = senderPassword;
        this.receiverEmail = receiverEmail;
        this.host = host;
        this.port = port;
    }

    public void Send(string report)
    {
        using MailMessage message = new MailMessage(senderEmail, receiverEmail)
        {
            Subject = "SPACE Mission Report",
            Body = report
        };

        using SmtpClient client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(senderEmail, senderPassword)
        };

        client.Send(message);
    }
}
