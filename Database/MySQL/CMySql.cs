using UnityEngine;
using System;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;

public class CMySql : MonoBehaviour
{
    public static MySqlConnection dbConnection;//Just like MyConn.conn in StoryTools before  
    public static string host = "whatbug.cn";
    static string id = "root";  //***不要变***
    static string pwd = "FuckUnityEED";  //密码
    static string database = "unity_good_job";//数据库名  
    static string result = "";

    private string strCommand = "Select ID from unity ;";
    public static DataSet MyObj;

    public static string connectionString = "Server = {0}; Database = {1}; User ID = {2}; Password = {3};";

    private void Awake()
    {

        connectionString = string.Format(connectionString, host, database, id, pwd);
        openSqlConnection(connectionString);
    }
    // On quit  
    public static void OnApplicationQuit()
    {
        closeSqlConnection();
    }

    // Connect to database  
    private static bool openSqlConnection(string connectionString)
    {
        dbConnection = new MySqlConnection(connectionString);
        dbConnection.Open();
        result = dbConnection.ServerVersion;  //获得MySql的版本
        return true;
    }

    // Disconnect from database  
    private static void closeSqlConnection()
    {
        dbConnection.Close();
        dbConnection = null;
    }
    public static DataSet Query(string v)
    {
        //if (!checkDBStatus()) { return null; }

        //DataSet dataset = new DataSet();

        //using (MySqlCommand cmd = new MySqlCommand(v, dbConnection))
        //{
        //    Debug.Log(cmd.CommandText);
        //    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //    adapter.Fill(dataset);
        //}
        return Query(v,null);
    }
    internal static DataSet Query(string v, MySqlParameter[] parameters)
    {
        if (!checkDBStatus()){ return null;}

        DataSet dataset = new DataSet();
        
        using (MySqlCommand cmd = new MySqlCommand(v, dbConnection))
        {
            if (null!=parameters)
            {
                foreach (MySqlParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dataset);
        }
        return dataset;
    }

    internal static bool Exists(string v, MySqlParameter[] parameters)
    {
        DataTable dt= Query(v, parameters).Tables[0];
        if (dt==null|| dt.Rows.Count<=0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// update , insert,delete
    /// </summary>
    /// <param name="v"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static int ExecuteSql(string v, MySqlParameter[] parameters)
    {
        int res = 0;
        using (MySqlCommand cmd = new MySqlCommand(v, dbConnection))
        {
            foreach (MySqlParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }
            try
            {
                res = cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Debug.Log("ERROR:" + e.Message);
                res = -1;
            }
        }
        return res;
    }

   

    static bool checkDBStatus()
    {

        if (dbConnection.State == System.Data.ConnectionState.Open || openSqlConnection(connectionString))
        {
            return true;
        }
        return false;
    }

    }