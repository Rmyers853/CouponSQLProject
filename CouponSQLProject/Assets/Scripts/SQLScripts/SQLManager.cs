using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public class SQLManager : MonoBehaviour
{
    public IDbConnection CreateAndOpenDatabase()
    {
        string dbUri = "URI=file:GroceryPriceComparer.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        return dbConnection;
    }

    public void CreateAndOpenTable(string tableName)
    {
        string tableString = "TableName";
        if (tableName == "StoresTable")
        {
            tableString = "StoresTable(addressid INTEGER PRIMARY KEY, storename TEXT, distance REAL)";
        }
        else if (tableName == "Addresses")
        {
            tableString = "Addresses(addressid INTEGER PRIMARY KEY, streetnum INTEGER, streetname TEXT, city TEXT, state TEXT, country TEXT, zipcode INTEGER)";
        }
        IDbCommand dbCommandCreateTable = CreateAndOpenDatabase().CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS " + tableString;
        dbCommandCreateTable.ExecuteReader();
    }

    public void ExecuteSQLCommand(string command)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();
        dbCommandInsertValue.CommandText = command;
        dbCommandInsertValue.ExecuteNonQuery();
        dbConnection.Close();
    }
}