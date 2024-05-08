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
      if (string.IsNullOrEmpty(user.Password))
      {
        throw new WrongLoginMethodException();
      };
      user.ResetPasswordToken = TokenUtils.CreateSecurityToken();
      user.ResetPasswordTokenExpirationDate = DateTimeOffset.Now.AddDays(TokenSettingConstants.PASSWORD_TOKEN_EXPIRATION_DAYS);
      await UnitOfWork.SaveChangesAsync();

      EmailTemplate emailTemplate = await UnitOfWork.EmailTemplates.FirstAsync(x => x.Type == EmailType.ForgotPassword);
      string body = emailTemplate.Body
        .Replace(EmailKey.FO_ROUTE, AppUrls.FrontOffice)
        .Replace(EmailKey.PASSWORD_TOKEN, user.ResetPasswordToken)
        .Replace(EmailKey.EMAIL, email);

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

    public async Task SendUpgradeMailsAsync()
    {
      List<UpgradeForm> upgradeForms = await UnitOfWork.UpgradeForms
        .AsTracking()
        .Include(x => x.User)
        .Where(x => x.Status == UpgradeStatus.Accepted || x.Status == UpgradeStatus.Rejected)
        .ToListAsync();

      List<Task> tasks = new();
      foreach (var form in upgradeForms)
      {
        tasks.Add(SendUpgradeMailAsync(form));
      }
      await Task.WhenAll(tasks);
      await UnitOfWork.SaveChangesAsync();
    }

    public async Task SendAdminMailsAsync()
    {
      List<AdminForm> adminForms = await UnitOfWork.AdminForms
        .AsTracking()
        .Where(x => x.Status == AdminFormStatus.Pending)
        .ToListAsync();

      List<Task> tasks = new();
      foreach (var form in adminForms)
      {
        tasks.Add(SendAdminMailAsync(form));
      }
      await Task.WhenAll(tasks);
      await UnitOfWork.SaveChangesAsync();
    }

    #region private

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

    private static MailMessage CreateMailMessage(MailDataModel data)
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

    private async Task<MailDataModel> CreateUpgradeMailDataAsync(string email, bool isSucceed)
    {
      EmailType type = isSucceed ? EmailType.UpgradeAccountSuccess : EmailType.UpgradeAccountFailure;
      EmailTemplate emailTemplate = await UnitOfWork.EmailTemplates.FirstAsync(x => x.Type == type);
      string body = emailTemplate.Body
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

    private async Task<MailDataModel> CreateAdminFormMailDataAsync(string email, bool isAvailable)
    {
      EmailType type = isAvailable ? EmailType.UpgradedToAdmin : EmailType.InviteToBeAdmin;
      EmailTemplate emailTemplate = await UnitOfWork.EmailTemplates.FirstAsync(x => x.Type == type);
      string body = emailTemplate.Body
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

    private async Task SendUpgradeMailAsync(UpgradeForm form)
    {
      bool isAccepted = form.Status == UpgradeStatus.Accepted;
      form.Status = UpgradeStatus.Done;

      await UnitOfWork.UserOrganizationAttributes.CreateAsync(new UserOrganizationAttributes()
      {
        Id = form.User.Id,
        EntityName = form.EntityName,
        OrganizationName = form.OrganizationName,
        Email = HashUtils.EnryptString(form.Email),
        Website = form.Website,
        TaxCode = form.TaxCode,
        Type = form.Type,
        Description = form.Description,
      });

      form.User.Role = UserRole.Organization;
      form.User.Phone = form.Phone;
      form.User.ProvinceCode = form.PrivinceCode;
      form.User.DistrictCode = form.DistrictCode;
      form.User.WardCode = form.WardCode;
      form.User.Street = form.Street;
      form.User.Address = form.Address;

      UnitOfWork.UpgradeForms.Update(form);

      string email = HashUtils.DecryptString(form.User.Email);
      MailDataModel mailData = await CreateUpgradeMailDataAsync(email, isAccepted);
      await SendMailAsync(mailData);
      return;
    }

    private async Task SendAdminMailAsync(AdminForm form)
    {
      bool isAvailable = false;
      form.Status = AdminFormStatus.Done;
      UnitOfWork.AdminForms.Update(form);

      User? admin = await UnitOfWork.Users
        .AsTracking()
        .FirstOrDefaultAsync(x => x.Email == HashUtils.EnryptString(form.Email));

      if (admin != null)
      {
        admin.Role = UserRole.SystemAdmin;
        UnitOfWork.Users.Update(admin);
        isAvailable = true;
      }

      MailDataModel mailData = await CreateAdminFormMailDataAsync(form.Email, isAvailable);
      await SendMailAsync(mailData);
    }

    #endregion
  }
}