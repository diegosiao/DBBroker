using DBBroker.Engine;
using System;

namespace DBBroker.Mapping
{
    /// <summary>
    /// Should be used in properties that has initial value defined by a database mechanism.
    /// <para>The related property will be ignored in SQL instructions of INSERT and UPDATE.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DBReadOnly : Map
    {
        /// <summary>
        /// Database default value or function. This information is used in the script creation only (<see cref="SqlScriptMaker"/>).
        /// <para>e.g.: GETDATE()</para>
        /// </summary>
        public string DBDefaultValue { get; set; }
    }
}
