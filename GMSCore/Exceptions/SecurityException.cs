using System;
using System.Collections.Generic;
using System.Text;

namespace GMSCore.Exceptions
{
    public class SecurityException : System.Exception
    {
        private string tmp;
        /// <summary>
        /// Overrides the Exception's Message
        /// </summary>
        public override string Message
        {
            get
            {
                return "You do not have the access rights to " + tmp;
            }
        }

        public SecurityException(string type)
        {
            this.tmp = type;
        }

        public SecurityException(SecurityUsageType usage)
        {
            this.tmp = usage.ToString().ToLower();
        }
    }
}
