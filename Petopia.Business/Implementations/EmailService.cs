using System.Configuration;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Setting;

namespace Petopia.Business.Implementations
{
  public class EmailService : BaseService, IEmailService
  {
    private readonly SmtpClient _smtpClient;

    public EmailService(
      IServiceProvider provider, 
      ILogger<EmailService> logger
    ) : base(provider, logger)
    {
      var emailSetting = Configuration.GetSection(AppSettingKey.EMAIL).Get<EmailSettingModel>()
        ?? throw new ConfigurationErrorsException();

      _smtpClient = new SmtpClient(emailSetting.SmtpClient, 587)
      {
        EnableSsl = true,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential
        (
          emailSetting.EmailClient,
          emailSetting.Password
        )
      };
    }

    public async Task SendForgotPasswordEmailAsync(string email)
    {
      var user = await UnitOfWork.Users.FirstOrDefaultAsync(x => x.Email == email)
        ?? throw new NotFoundEmailException();
      // todo here
    }
  }
}