using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Interfaces;
using PetAdoption.Business.Models;

namespace PetAdoption.Business.Implementations
{
  public class EmailService : IEmailService
  {
    private readonly SmtpClient _smtpClient;

    public EmailService(IConfiguration configuration)
    {
      var emailSetting = configuration.GetSection(AppSettingKey.EMAIL).Get<EmailSettingModel>()
        ?? throw new Exception("Email settings not found");

      _smtpClient = new SmtpClient(emailSetting.SmtpClient, 587)
      {
        EnableSsl = true,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(
          emailSetting.EmailClient,
          emailSetting.Password
        )
      };
    }
  }
}