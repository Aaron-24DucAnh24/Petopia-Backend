using Petopia.Business.Models.Email;
using Petopia.Business.Models.Payment;

namespace Petopia.Business.Interfaces
{
  public interface IEmailService
  {
    public Task SendMailAsync(MailDataModel data);
    public Task<MailDataModel> CreateForgotPasswordMailDataAsync(string email);
    public Task<MailDataModel> CreateValidateRegisterMailDataAsync(string email, string registerToken);
    public Task<MailDataModel> CreateInvoiceMailDataAsync(CreatePaymentResponseModel model);
    public Task<MailDataModel> CreateAdminMailDataAsync(string email, string password);
    public Task<MailDataModel> CreateUpgradeMailDataAsync(string email, bool isSucceed);
  }
}