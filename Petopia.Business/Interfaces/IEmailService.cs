namespace Petopia.Business.Interfaces
{
  public interface IEmailService
  {
    public Task SendForgotPasswordEmailAsync(string email);
  }
}