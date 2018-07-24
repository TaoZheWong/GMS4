using System;
using System.Collections.Generic;
using System.Text;

namespace GMSCore.Exceptions
{
    public class NullSessionException : System.Exception
    {
        /// <summary>
        /// Overrides the Exception's Message
        /// </summary>
        public override string Message
        {
            get
            {
                return "Your session has expired. Please log in again.";
            }
        }

        public NullSessionException()
        {
        }
    }
}
