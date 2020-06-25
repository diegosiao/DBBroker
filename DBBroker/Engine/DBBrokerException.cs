using System;
using System.Collections.Generic;
using System.Text;

namespace DBBroker.Engine
{
    /// <summary>
    /// The generic DBBroker exception
    /// </summary>
    public class DBBrokerException : Exception
    {
        /// <summary>
        /// The constructor of the the generic DBBroker exception
        /// </summary>
        /// <param name="message">The error message</param>
        public DBBrokerException(string message) : base(message) { }

        /// <summary>
        /// The constructor of the the generic DBBroker exception
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">The prior or inner error that caused this exception</param>
        public DBBrokerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
