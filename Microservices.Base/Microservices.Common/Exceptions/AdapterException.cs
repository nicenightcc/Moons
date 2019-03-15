using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Common
{
    public class AdapterException : Exception
    {
        //
        // 摘要:
        //     Initializes a new instance of the System.Exception class.
        public AdapterException()
        {
        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message.
        //
        // 参数:
        //   message:
        //     The message that describes the error.
        public AdapterException(string message) : base(message)
        {
        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message and a reference to the inner exception that is the cause of this exception.
        //
        // 参数:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public AdapterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
