using System;

namespace Petopia.Business.Models.Exceptions
{
  public class ForbiddenAccessException : System.Exception
  {
    public ForbiddenAccessException() : base("Forbidden")
    {
    }
  }
}