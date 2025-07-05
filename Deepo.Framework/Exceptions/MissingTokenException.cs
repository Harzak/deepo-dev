using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Framework.Exceptions;

public class MissingTokenException : System.Exception
{
    public MissingTokenException()
    {

    }
    public MissingTokenException(string message) : base(message)
    {

    }

    public MissingTokenException(string message, System.Exception innerException) : base(message, innerException)
    {

    }
}