using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace SimpleDBUpdate.Data
{
  public partial class SimpleDB
  {
    private static string _dbName = "Simple.db";
    private static string _conString = "Data Source=" + _dbName + ";Version=3";
    internal SQLiteConnection _con;
    private SQLiteCommand _cmd;

    public SimpleDB()
    {
      // User of the code should never have to worry about the database
      OpenDatabase();
    }

    private void OpenDatabase()
    {
      _con = new SQLiteConnection(_conString);
      _con.Open();
      _cmd = _con.CreateCommand();
    }

    private bool TableExists(string tableName)
    {
      bool ret = false;
      SQLiteCommand cmd = _con.CreateCommand();
      cmd.CommandText = "select name from sqlite_master where type='table' and name = '" + tableName + "'";
      // sqlite_master = type, name, tbl_name, rootpage, sql
      object name = cmd.ExecuteScalar();
      ret = ((null != name) & (tableName.Equals((string)name)));
      return ret;
    }
  }
}
