using System;
using System.Collections.Generic;
using System.Text;

namespace Complaints.Data.ViewModels
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException() : base()
        {
        }

        public AuthenticationException(string message) : base(message)
        {
        }

        public AuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
