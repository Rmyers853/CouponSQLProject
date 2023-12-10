using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SQLManager : MonoBehaviour
{
    public string storeName;
    public int addressId;

    public void Awake()
    {
        if (!IsLoaded("SQLManagerScene"))
        {
            SceneManager.LoadScene("SQLManagerScene", LoadSceneMode.Additive);
        }
    }

    private static bool IsLoaded(string name)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == name)
            {
                return true;
            }
        }
        return false;
    }

    public IDbConnection CreateAndOpenDatabase()
    {
        string dbUri = "URI=file:GroceryPriceComparer.sqlite;";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        return dbConnection;
    }

    public void CreateAndOpenTable(string tableName)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase();
        string tableString = "TableName";
        if (tableName == "StoresTable")
        {
            tableString = "StoresTable(addressid INTEGER PRIMARY KEY, storename TEXT, distance REAL, CONSTRAINT fk_item FOREIGN KEY(addressid) REFERENCES Addresses(addressid))";
        } else if (tableName == "Addresses")
        {
            tableString = "Addresses(addressid INTEGER PRIMARY KEY, streetnum INTEGER, streetname TEXT, city TEXT, state TEXT, country TEXT, zipcode INTEGER)";
        } else if (tableName == "ItemPrices")
        {
            tableString = "ItemPrices(itemname TEXT, addressid INTEGER, price REAL, CONSTRAINT pk_items PRIMARY KEY(itemname,addressid), CONSTRAINT fk_item FOREIGN KEY(itemname) REFERENCES Items(itemname), CONSTRAINT fk_item2 FOREIGN KEY(addressid) REFERENCES Addresses(addressid))";
        } else if (tableName == "Items")
        {
            tableString = "Items(itemname TEXT PRIMARY KEY)";
        } else  if (tableName == "GroceryList")
        {
            tableString = "GroceryList(itemname TEXT PRIMARY KEY, count INTEGER, CONSTRAINT fk_items FOREIGN KEY(itemname) REFERENCES Items(itemname))";
        }
        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS " + tableString;
        dbCommandCreateTable.ExecuteNonQuery();
        dbConnection.Close();
    }

    public void ReadSQLValuesStrings(string command, List<String> items, int index)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = command;
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();

        while (dataReader.Read())
        {
            items.Add(dataReader.GetString(index));
        }

        dbConnection.Close();
    }

    public void ReadSQLValuesIntegers(string command, List<int> items, int index)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = command;
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();

        while (dataReader.Read())
        {
            items.Add(dataReader.GetInt32(index));
        }

        dbConnection.Close();
    }

    public void ReadSQLValuesFloats(string command, List<float> items, int index)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = command;
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();

        while (dataReader.Read())
        {
            items.Add(dataReader.GetFloat(index));
        }

        dbConnection.Close();
    }

    public string[] ReadSQLValuesAddress(int currentAddressId)
    {
        string[] returnString = new string[6];
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT * FROM Addresses WHERE addressId = " + currentAddressId;
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
        {
            returnString[0] = dataReader.GetInt32(1).ToString();
            returnString[1] = hexToString(dataReader.GetString(2));
            returnString[2] = hexToString(dataReader.GetString(3));
            returnString[3] = hexToString(dataReader.GetString(4));
            returnString[4] = hexToString(dataReader.GetString(5));
            returnString[5] = dataReader.GetInt32(6).ToString();
        }
        dbConnection.Close();
        return returnString;
    }

    public string[] ReadSQLValuesStoresTable(int currentAddressId)
    {
        string[] returnString = new string[2];
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT * FROM StoresTable WHERE addressId = " + currentAddressId;
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
        {
            returnString[0] = hexToString(dataReader.GetString(1));
            returnString[1] = dataReader.GetFloat(2).ToString();
        }
        dbConnection.Close();
        return returnString;
    }

    public string[] ReadSQLValuesItemsTable(int currentAddressId, string itemName)
    {
        string[] returnString = new string[2];
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT price FROM ItemPrices WHERE addressId = " + currentAddressId + " AND itemname = \'" + itemName + "\'";
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
        {
            returnString[0] = dataReader.GetFloat(0).ToString();
        }
        dbConnection.Close();
        return returnString;
    }

    public float GetPrice(int addressId, string itemName)
    {
        float returnVal = -1.0f;
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT price FROM ItemPrices WHERE addressid = " + addressId + " AND itemname = \'" + itemName + "\'";
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();

        while (dataReader.Read())
        {
            returnVal = dataReader.GetFloat(0);
        }

        dbConnection.Close();
        return returnVal;
    }

    public string ExecuteSQLCommand(string command)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();
        dbCommandInsertValue.CommandText = "PRAGMA foreign_keys=ON";
        dbCommandInsertValue.ExecuteNonQuery();
        dbCommandInsertValue.CommandText = command;
        try
        {
            dbCommandInsertValue.ExecuteReader();
        }
        catch (Exception myException)
        {
            return myException.Message;
        }
        dbConnection.Close();
        return null;
    }

    public void CompareStores(List<string> storeNames, List<float> prices)
    {
        List<string> itemNames = new List<string>();
        List<int> counts = new List<int>();
        List<int> addressIds = new List<int>();
        List<string> possibleStoreNames = new List<string>();
        ReadSQLValuesStrings("SELECT itemname FROM GroceryList", itemNames, 0);
        ReadSQLValuesIntegers("SELECT count FROM GroceryList", counts, 0);
        ReadSQLValuesIntegers("SELECT addressid FROM StoresTable ORDER BY addressid", addressIds, 0);
        ReadSQLValuesStrings("SELECT storename FROM StoresTable ORDER BY addressid", possibleStoreNames, 0);
        float[] finalPrices = new float[addressIds.Count];
        int[] invalidIndexes = new int[addressIds.Count];
        for (int i = 0; i < itemNames.Count; i++)
        {
            for (int j = 0; j < addressIds.Count; j++)
            {
                float currentPrice = GetPrice(addressIds[j], itemNames[i]);
                if (currentPrice != -1)
                {
                    finalPrices[j] += currentPrice * counts[i];
                } else
                {
                    invalidIndexes[j] = 1;
                }
            }
        }

        List<Tuple<string, float>> combinedList = new List<Tuple<string, float>>();

        for (int k = 0; k < addressIds.Count; k++)
        {
            if (invalidIndexes[k] != 1)
            {
                combinedList.Add(new Tuple<string, float>(possibleStoreNames[k], finalPrices[k]));
            }
        }

        combinedList = combinedList.OrderBy(x => x.Item2).ToList();

        for (int l = 0; l < combinedList.Count; l++)
        {
            storeNames.Add(combinedList[l].Item1);
            prices.Add(combinedList[l].Item2);
        }
    }

    public String hexToString(string hexString)
    {
        byte[] raw = new byte[hexString.Length / 2];
        for (int i = 0; i < raw.Length; i++)
        {
            raw[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }
        return Encoding.ASCII.GetString(raw);
    }

    public String stringToHex(string stringToConvert)
    {
        byte[] ba = Encoding.Default.GetBytes(stringToConvert);
        return BitConverter.ToString(ba).Replace("-", "");
    }

    public IEnumerator SceneSwitchCoroutine(string oldSceneName, string newSceneName)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(oldSceneName));
        AsyncOperation load = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
        yield return load;
    }

    public void SceneSwitch(string oldSceneName, string newSceneName)
    {
        StartCoroutine(SceneSwitchCoroutine(oldSceneName, newSceneName));
    }
}