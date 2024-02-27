using System;

namespace Petopia.Business.Models.Exceptions
{
  public class ForbiddenAccessException : Exception
  {
    public ForbiddenAccessException() : base("Forbidden")
    {
    }
  }
}