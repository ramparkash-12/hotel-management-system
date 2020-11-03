using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Booking.API.Model;
using Polly;
using Serilog;

namespace Booking.API.Services
{
  public class SMTPEmailNotifier : IEmailNotifier
  {

    private string _smptServer;
    private int _smtpPort;
    private string _userName;
    private string _password;


    public SMTPEmailNotifier(string smtpServer, int smtpPort, string userName, string password)
    {
      _smptServer = smtpServer;
      _smtpPort = smtpPort;
      _userName = userName;
      _password = password;
    }

    public async Task SendEmailAsync(EmailModel emailModel)
    {
        try
        {
            using (SmtpClient client = new SmtpClient(_smptServer, _smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_userName, _password);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailModel.From, emailModel.BusinessNameForEmail);
                mailMessage.To.Add(new MailAddress(emailModel.To.Email, emailModel.To.Name));
                mailMessage.Body = emailModel.Email;
                mailMessage.Subject = emailModel.Subject;

                mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailModel.Email, null, MediaTypeNames.Text.Html));

                await Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, r => TimeSpan.FromSeconds(2), (ex, ts) => { Log.Error("Error sending mail. Retrying in 2 sec."); })
                    .Execute(() => client.SendMailAsync(mailMessage))
                    .ContinueWith(_ => Log.Information("Mail sent to {0} ({1}) ", emailModel.To.Name,  emailModel.To.Email ));
            }
        }   //** Log Errors...
        catch (SmtpException Smtpex)
        {
            throw Smtpex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    
  }
}