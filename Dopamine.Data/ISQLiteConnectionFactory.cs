using Microsoft.Data.Sqlite;

namespace Dopamine.Data
{
    public interface ISQLiteConnectionFactory
    {
        string DatabaseFile { get; }
        SqliteConnection GetConnection();
    }
}
