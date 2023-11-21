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
      _smtpClient = new SmtpClient(settings.SmtpClient, 587)
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
      var user = await CheckCorrectEmailAsync(email);
      user.ResetPasswordToken = TokenUtils.CreateSecurityToken();
      user.ResetPasswordTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenSettingConstants.PASSWORD_TOKEN_EXPIRATION_DAYS);
      await UnitOfWork.SaveChangesAsync();
      // TODO: create forgot password email template and import here
      var subject = string.Empty;
      var body = string.Empty;
      var toAddresses = new List<string>();
      toAddresses.Add(email);
      return new MailDataModel()
      {
        From = _settings.EmailClient,
        To = toAddresses,
        Subject = subject,
        Body = body,
        IsBodyHtml = true
      };
    }

    public async Task<MailDataModel> CreateValidateRegisterMailDataAsync(string email, string registerToken)
    {
      var emailTemplate = await UnitOfWork.Emails.FirstAsync(x => x.Type == EmailType.ValidateRegister);
      var body = emailTemplate.Body
        .Replace(EmailKey.EMAIL, email)
        .Replace(EmailKey.REGISTER_TOKEN, registerToken)
        .Replace(EmailKey.API_ROUTE, ApiRoute);

      var toAddresses = new List<string>();
      toAddresses.Add(email);

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
      var user = await UnitOfWork.Users
        .AsTracking()
        .Include(x => x.UserIndividualAttributes)
        .Include(x => x.UserOrganizationAttributes)
        .FirstOrDefaultAsync(x => x.Email == HashUtils.HashString(email))
        ?? throw new IncorrectEmailException();
      return user;
    }

    private MailMessage CreateMailMessage(MailDataModel data)
    {
      var res = new MailMessage();
      res.From = new MailAddress(data.From);
      res.Subject = data.Subject;
      res.Body = data.Body;
      res.IsBodyHtml = data.IsBodyHtml;
      foreach (var addressString in data.To)
      {
        res.To.Add(new MailAddress(addressString));
      }
      return res;
    }
  }
}