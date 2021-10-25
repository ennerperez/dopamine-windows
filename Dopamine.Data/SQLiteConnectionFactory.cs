using Digimezzo.Foundation.Core.Settings;
using Dopamine.Core.Base;

using System.IO;
using Microsoft.Data.Sqlite;

namespace Dopamine.Data
{
    public class SQLiteConnectionFactory : ISQLiteConnectionFactory
    {
        public string DatabaseFile => Path.Combine(SettingsClient.ApplicationFolder(), ProductInformation.ApplicationName + ".db");
        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(this.DatabaseFile) { DefaultTimeout = 10*1000 };
        }
    }
}
