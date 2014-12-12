using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace SimpleDBUpdate.Data
{
  public partial class SimpleDB
  {
    // See that - were part of this class and this class ONLY
    public class SimpleDBInfo
    {
      private SimpleDB _db;
      private SQLiteCommand _insCmd;
      private SQLiteCommand _updCmd;
      private SQLiteCommand _selCmd;
      private bool __controlLock;
      private const string VERSION = "_version";
      private const string APPLIED = "_versionsApplied";

      internal SimpleDBInfo(SimpleDB db)
      {
        _db = db;

        // I don't want three command objects, but this is all this class does
        _insCmd = _db._con.CreateCommand();
        _insCmd.CommandText = "insert into _info(name, value) values (@name, @value)";
        _insCmd.Parameters.Add("name", System.Data.DbType.String);
        _insCmd.Parameters.Add("value", System.Data.DbType.String);

        _selCmd = _db._con.CreateCommand();
        _selCmd.CommandText = "select value from _info where name = @name";
        _selCmd.Parameters.Add("name", System.Data.DbType.String);

        _updCmd = _db._con.CreateCommand();
        _updCmd.CommandText = "update _info set value = @value where name = @name";
        _updCmd.Parameters.Add("name", System.Data.DbType.String);
        _updCmd.Parameters.Add("value", System.Data.DbType.String);

        if (!_db.TableExists("_info"))
          CreateInfoTable();
      }

      private void CreateInfoTable()
      {
        SQLiteCommand cmd = _db._con.CreateCommand();
        cmd.CommandText = "create table _info(name varchar(20), value varchar(50))";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "create unique index uniq_name on _info (name);";
        cmd.ExecuteNonQuery();

        _insCmd.Parameters["name"].Value = VERSION;
        _insCmd.Parameters["value"].Value = "0";
        _insCmd.ExecuteNonQuery();
      }

      private bool KeyExists(string name)
      {
        _selCmd.Parameters["name"].Value = name;
        object obj = _selCmd.ExecuteScalar();
        return obj != null;
      }

      // In case someone else cares, but they can't change it
      public string GetVersion()
      {
        return GetValue(VERSION);
      }

      public string[] GetAppliedVersions(char delimiter = ' ')
      {
        string[] sort = { "" };
        string ret = GetValue(APPLIED).Trim();
        if (!ret.Equals(String.Empty))
        {
          sort = ret.Split(delimiter);
          sort = sort.OrderBy(s => s.Split('.').Aggregate(0, (n, v) => n * 100 + Int32.Parse(v))).ToArray();
        }
        return sort;
      }

      internal string Version
      {
        get { return GetValue(VERSION); }
        // Whenever we set the version append it to the applied ones
        set { ControlLock(VERSION, value); ControlLockAppend(APPLIED, value); }
      }

      internal void ControlLock(string name, string version)
      {
        __controlLock = true;
        SetValue(name, version);
        __controlLock = false;
      }

      internal void ControlLockAppend(string name, string version, char delimiter = ' ')
      {
        __controlLock = true;
        AppendValue(name, version, delimiter);
        __controlLock = false;
      }

      public string GetValue(string name, string defaultValue = "")
      {
        string ret = defaultValue;
        _selCmd.Parameters["name"].Value = name;
        object obj = _selCmd.ExecuteScalar();
        if (obj != null) ret = (string)obj;
        return ret;
      }

      public void SetValue(string name, string value)
      {
        // If you want to set an _ value, you have to go through ControlLock
        if (name.StartsWith("_") & !__controlLock) return;

        if (KeyExists(name))
          UpdateValue(name, value);
        else
          InsertValue(name, value);
      }

      public void AppendValue(string name, string value, char delimiter)
      {
        string current = GetValue(name);
        if (current.Equals(String.Empty))
          SetValue(name, value);
        else
          SetValue(name, current + delimiter + value);
      }

      private void InsertValue(string name, string value)
      {
        _insCmd.Parameters["name"].Value = name;
        _insCmd.Parameters["value"].Value = value;
        _insCmd.ExecuteNonQuery();
      }

      private void UpdateValue(string name, string value)
      {
        _updCmd.Parameters["name"].Value = name;
        _updCmd.Parameters["value"].Value = value;
        _updCmd.ExecuteNonQuery();
      }
    }
  }
}
