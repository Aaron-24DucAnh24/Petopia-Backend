using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exceptions
{
	public class FormNotFoundException : DomainException
	{
		public FormNotFoundException() : base("The form is not found")
		{
			ErrorCode = DomainErrorCode.NOT_FOUND_FORM;
		}
	}
}