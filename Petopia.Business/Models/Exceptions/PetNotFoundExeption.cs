using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
  public class PetNotFoundException : DomainException
  {
    public PetNotFoundException() : base("The pet is not found")
    {
      ErrorCode = DomainErrorCode.NOT_FOUND_PET;
    }
  }
}