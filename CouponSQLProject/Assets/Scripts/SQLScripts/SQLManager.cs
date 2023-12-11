using Firebase.Database;
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SQLManager : MonoBehaviour
{
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

    public List<object> ReadSQLValues(string command, int index)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = command;
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();
        List<object> items = new List<object>();
        while (dataReader.Read()) { items.Add(dataReader.GetValue(index)); }
        dbConnection.Close();
        return items;
    }

    public void ReadSQLValues(string command, List<string> items, int index)
    {
        List<object> objects = ReadSQLValues(command, index);
        for (int i = 0; i < objects.Count; i++) { items.Add(objects[i].ToString()); }
    }

    public void ReadSQLValues(string command, List<int> items, int index)
    {
        List<object> objects = ReadSQLValues(command, index);
        for (int i = 0; i < objects.Count; i++) { items.Add(Convert.ToInt32(objects[i].ToString())); }
    }

    public void ReadSQLValues(string command, List<float> items, int index)
    {
        List<object> objects = ReadSQLValues(command, index);
        for (int i = 0; i < objects.Count; i++) { items.Add(Convert.ToSingle(objects[i].ToString())); }
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

    public string ExecuteSQLCommand(string command)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();
        dbCommandInsertValue.CommandText = "PRAGMA foreign_keys=ON";
        dbCommandInsertValue.ExecuteNonQuery();
        dbCommandInsertValue.CommandText = command;
        try { dbCommandInsertValue.ExecuteReader(); }
        catch (Exception myException) { return myException.Message; }
        dbConnection.Close();
        return null;
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

    public void CompareStores(List<string> storeNames, List<float> prices)
    {
        List<string> itemNames = new List<string>();
        List<int> counts = new List<int>();
        List<int> addressIds = new List<int>();
        List<string> possibleStoreNames = new List<string>();
        ReadSQLValues("SELECT itemname FROM GroceryList", itemNames, 0);
        ReadSQLValues("SELECT count FROM GroceryList", counts, 0);
        ReadSQLValues("SELECT addressid FROM StoresTable ORDER BY addressid", addressIds, 0);
        ReadSQLValues("SELECT storename FROM StoresTable ORDER BY addressid", possibleStoreNames, 0);
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

    public string storeName;
    public int addressId;
    public DatabaseReference DBreference;
    public float time;
    public static bool userDeleteData;

    public void Awake()
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        time = 0f;
        userDeleteData = false;
        if (!IsLoaded("SQLManagerScene"))
        {
            CreateAndOpenTable("StoresTable");
            CreateAndOpenTable("Addresses");
            CreateAndOpenTable("ItemPrices");
            CreateAndOpenTable("Items");
            CreateAndOpenTable("GroceryList");
            ExecuteSQLCommand("CREATE UNIQUE INDEX idx_itemprices_addressid_itemname ON ItemPrices(addressid, itemname)");
            ReadAllData();
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

    public void ConvertAndSendToFirebase(string command, int columns, string tableName)
    {
        IDbConnection dbConnection = CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = command;
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();
        object[] testArray = new object[columns];
        int currentRow = 0;
        while (dataReader.Read())
        {
            dataReader.GetValues(testArray);
            StartCoroutine(sendData(tableName, currentRow, string.Join("|", testArray)));
            currentRow++;
        }
        dbConnection.Close();
    }

    private IEnumerator sendData(string tableName, int currentRow, string data)
    {
        var DBTask = DBreference.Child(tableName+"/"+currentRow).SetValueAsync(data);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to update with {DBTask.Exception}");
        }
    }

    private IEnumerator readData(string tableName)
    {
        var DBTask = DBreference.Child(tableName).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to update with {DBTask.Exception}");
        }
        DataSnapshot snapshot = DBTask.Result;
        List<object> testList = (List<object>)snapshot.Value;
        if (testList != null)
        {
            for (int i = 0; i < testList.Count; i++)
            {
                string[] currentRow = testList[i].ToString().Split('|');
                switch (tableName)
                {
                    case "StoresTable":
                        ExecuteSQLCommand("INSERT INTO StoresTable (addressid, storename, distance) VALUES ( " + int.Parse(currentRow[0]) + ", " + "\'" + currentRow[1] + "\', " + float.Parse(currentRow[2]) + ")");
                        break;
                    case "Addresses":
                        ExecuteSQLCommand("INSERT INTO Addresses (addressid, streetnum, streetname, city, state, country, zipcode) VALUES ( "
                        + int.Parse(currentRow[0]) + ", " + int.Parse(currentRow[1]) + ", " + "\'" + currentRow[2] + "\', " + "\'" + currentRow[3] + "\', "
                        + "\'" + currentRow[4] + "\', " + "\'" + currentRow[5] + "\', " + int.Parse(currentRow[6]) + ")");
                        break;
                    case "ItemPrices":
                        ExecuteSQLCommand("INSERT INTO ItemPrices (itemname, addressid, price) VALUES ( \'" + currentRow[0] + "\', " + int.Parse(currentRow[1]) + ", " + float.Parse(currentRow[2]) + ")");
                        break;
                    case "Items":
                        ExecuteSQLCommand("INSERT INTO Items (itemname) VALUES ( \'" + currentRow[0] + "\')");
                        break;
                    case "GroceryList":
                        ExecuteSQLCommand("INSERT INTO GroceryList (itemname, count) VALUES ( \'" + currentRow[0] + "\', " + int.Parse(currentRow[1]) + ")");
                        break;
                }
            }
        }
    }

    private IEnumerator deleteData()
    {
        var DBTask = DBreference.RemoveValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    public void Update()
    {
        if (userDeleteData == false)
        {
            time += Time.deltaTime;
            if (time >= 5f)
            {
                time = 0f;
                WriteAllData();
            }
        }
    }

    public void DeleteAllData()
    {
        userDeleteData = true;
        StartCoroutine(deleteData());
        ExecuteSQLCommand("DROP TABLE IF EXISTS GroceryList");
        ExecuteSQLCommand("DROP TABLE IF EXISTS ItemPrices");
        ExecuteSQLCommand("DROP TABLE IF EXISTS Items");
        ExecuteSQLCommand("DROP TABLE IF EXISTS StoresTable");
        ExecuteSQLCommand("DROP TABLE IF EXISTS Addresses");
    }

    public void ReadAllData()
    {
        StartCoroutine(readData("Addresses"));
        StartCoroutine(readData("StoresTable"));
        StartCoroutine(readData("Items"));
        StartCoroutine(readData("ItemPrices"));
        StartCoroutine(readData("GroceryList"));
    }

    public void WriteAllData()
    {
        StartCoroutine(deleteData());
        ConvertAndSendToFirebase("SELECT * FROM StoresTable", 3, "StoresTable");
        ConvertAndSendToFirebase("SELECT * FROM Addresses", 7, "Addresses");
        ConvertAndSendToFirebase("SELECT * FROM ItemPrices", 3, "ItemPrices");
        ConvertAndSendToFirebase("SELECT * FROM Items", 1, "Items");
        ConvertAndSendToFirebase("SELECT * FROM GroceryList", 2, "GroceryList");
    }
}