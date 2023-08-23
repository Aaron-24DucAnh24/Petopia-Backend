using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.Business.Services.Interfaces;

namespace PetAdoption.Business.Services.Implementations
{
  public class EmailService : IEmailService
  {
    private readonly SmtpClient _smtpClient;
    private readonly IConfiguration _configuration;

    public EmailService(IServiceProvider serviceProvider)
    {
      _configuration = serviceProvider.GetService<IConfiguration>() ?? throw new Exception("Service not found");
      
      _smtpClient = new SmtpClient(_configuration.GetValue<string>("Email:SmtpClient"), 587)
      {
        EnableSsl = true,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(
          _configuration.GetValue<string>("Email:EmailClient"),
          _configuration.GetValue<string>("Email:Password")
        )
      };
    }
  }
}