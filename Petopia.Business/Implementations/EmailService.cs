using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Email;
using Petopia.Business.Models.Exceptions;
using Petopia.Business.Models.Setting;
using Petopia.Business.Utils;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Business.Implementations
{
  public class EmailService : BaseService, IEmailService
  {
    private readonly SmtpClient _smtpClient;
    private readonly EmailSettingModel _settings;

    public EmailService(
      IServiceProvider provider,
      ILogger<EmailService> logger,
      EmailSettingModel settings
    ) : base(provider, logger)
    {
      _smtpClient = new SmtpClient(settings.SmtpClient, settings.Port)
      {
        EnableSsl = true,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(settings.EmailClient, settings.Password)
      };
      _settings = settings;
    }

    public async Task SendMailAsync(MailDataModel data)
    {
      await _smtpClient.SendMailAsync(CreateMailMessage(data));
    }

    public async Task<MailDataModel> CreateForgotPasswordMailDataAsync(string email)
    {
      User user = await CheckCorrectEmailAsync(email);
      user.ResetPasswordToken = TokenUtils.CreateSecurityToken();
      user.ResetPasswordTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenSettingConstants.PASSWORD_TOKEN_EXPIRATION_DAYS);
      await UnitOfWork.SaveChangesAsync();
      
      EmailTemplate emailTemplate = await UnitOfWork.EmailTemplates.FirstAsync(x => x.Type == EmailType.ForgotPassword);
      string body = emailTemplate.Body
        .Replace(EmailKey.FO_ROUTE, AppUrls.FrontOffice)
        .Replace(EmailKey.PASSWORD_TOKEN, user.ResetPasswordToken);

      List<string> toAddresses = new() { email };

      return new MailDataModel()
      {
        From = _settings.EmailClient,
        To = toAddresses,
        Subject = emailTemplate.Subject,
        Body = body,
        IsBodyHtml = true
      };
    }

    public async Task<MailDataModel> CreateValidateRegisterMailDataAsync(string email, string registerToken)
    {
      EmailTemplate emailTemplate = await UnitOfWork.EmailTemplates.FirstAsync(x => x.Type == EmailType.ValidateRegister);
      string body = emailTemplate.Body
        .Replace(EmailKey.EMAIL, email)
        .Replace(EmailKey.REGISTER_TOKEN, registerToken)
        .Replace(EmailKey.FO_ROUTE, AppUrls.FrontOffice);

      List<string> toAddresses = new() { email };

      return new MailDataModel()
      {
        From = _settings.EmailClient,
        To = toAddresses,
        Subject = emailTemplate.Subject,
        Body = body,
        IsBodyHtml = true
      };
    }

    private async Task<User> CheckCorrectEmailAsync(string email)
    {
      User user = await UnitOfWork.Users
        .AsTracking()
        .Include(x => x.UserIndividualAttributes)
        .Include(x => x.UserOrganizationAttributes)
        .FirstOrDefaultAsync(x => x.Email == HashUtils.EnryptString(email))
        ?? throw new IncorrectEmailException();
      return user;
    }

    private MailMessage CreateMailMessage(MailDataModel data)
    {
      MailMessage res = new()
      {
        From = new MailAddress(data.From),
        Subject = data.Subject,
        Body = data.Body,
        IsBodyHtml = data.IsBodyHtml,
      };
      foreach (var addressString in data.To)
      {
        res.To.Add(new MailAddress(addressString));
      }
      return res;
    }
  }
}