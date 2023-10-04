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
      user.ResetPasswordTokenExpirationDate
        = DateTimeOffset.Now.AddHours(TokenSettingConstants.PASSWORD_TOKEN_EXPIRATION_HOURS);
      var subject = user.FirstName;
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

    private async Task<User> CheckCorrectEmailAsync(string email)
    {
      var res = await UnitOfWork.Users
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Email == HashUtils.HashString(email))
        ?? throw new IncorrectEmailException();
      return res;
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