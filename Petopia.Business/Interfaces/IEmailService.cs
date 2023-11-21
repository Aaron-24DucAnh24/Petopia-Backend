using Petopia.Business.Models.Email;

namespace Petopia.Business.Interfaces
{
  public interface IEmailService
  {
    public Task SendMailAsync(MailDataModel data);
    public Task<MailDataModel> CreateForgotPasswordMailDataAsync(string email);
    public Task<MailDataModel> CreateValidateRegisterMailDataAsync(string email, string registerToken);
  }
}