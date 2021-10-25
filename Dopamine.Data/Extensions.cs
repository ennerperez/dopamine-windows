using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace Dopamine.Data
{
    public static class Extensions
    {
        public static T ExecuteScalar<T>(this SqliteConnection connection, string query, params object[] values)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            if (values != null && values.Any())
            {
                var parameters = values.Select(m => new SqliteParameter() { Value = m });
                command.Parameters.AddRange(parameters);
            }

            var result = command.ExecuteScalar();
            return (T)result;
        }

        public static void Execute(this SqliteConnection connection, string query, params object[] values)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            if (values != null && values.Any())
            {
                var parameters = values.Select(m => new SqliteParameter() { Value = m });
                command.Parameters.AddRange(parameters);
            }

            command.ExecuteNonQuery();
        }

        public static List<T> Query<T>(this SqliteConnection connection, string query, params object[] values)
        {
            var results = new List<T>();
            var command = connection.CreateCommand();
            command.CommandText = query;

            if (values != null && values.Any())
            {
                var parameters = values.Select(m => new SqliteParameter() { Value = m });
                command.Parameters.AddRange(parameters);
            }

            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                var result = Activator.CreateInstance<T>();
                var props = typeof(T).GetProperties();
                var schema = reader.GetSchemaTable();
                while (reader.Read())
                {
                    foreach (DataColumn col in schema.Columns)
                    {
                        var prop = props.FirstOrDefault(m => m.Name == col.ColumnName);
                        if (prop != null)
                        {
                            var value = reader.GetValue(col.ColumnName); // TODO. Ordinal?
                            prop.SetValue(result, value);
                        }
                    }

                    results.Add(result);
                }
            }

            return results;
        }

        public static void Update(this SqliteConnection connection, object entity)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public static void Insert(this SqliteConnection connection, object entity)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public static void InsertAll(this SqliteConnection connection, IEnumerable<object> entity)
        {
            //TODO:
            throw new NotImplementedException();
        }
        
        public static IQueryable<T> Table<T>(this SqliteConnection connection, params object[] functions)
        {
            //TODO:
            throw new NotImplementedException();
            return null;
        }
    }
}