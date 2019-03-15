using System;
using System.Reflection;

namespace Easynvest.Infohub.Parse.Infra.CrossCutting.Options
{
    public class ConnectionStringOption
    {
        public string OracleConnection { get; set; }

        public string this[string connectionName]
        {
            get
            {
                PropertyInfo property = GetType().GetProperty(connectionName);

                if (property == null)
                    throw new ArgumentOutOfRangeException();

                return property.GetValue(this, null).ToString();
            }
        }
    }
}
