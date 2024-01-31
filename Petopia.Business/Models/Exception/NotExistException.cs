using Petopia.Business.Models.Exceptions;
using Petopia.Business.Constants;

namespace Petopia.Business.Models.Exception
{
    public class NotExistException : DomainException
    {
        public NotExistException() : base("Object Not Found")
        {
            ErrorCode = DomainErrorCode.NOT_EXIST;
        }
    }
}
