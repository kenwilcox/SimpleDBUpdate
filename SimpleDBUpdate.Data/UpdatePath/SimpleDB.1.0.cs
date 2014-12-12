using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleDBUpdate.Data
{
  public partial class SimpleDB
  {
    #region Version 1.0
    [SimpleDBError("1.0")]
    public string CreateDatabase1Error
    {
      get { return "Error - Failed to create Base Table"; }
    }

    [SimpleDBVersion("1.0")]
    public string CreateDatabase1()
    {
      // Create the table(s) I need...
      _cmd.CommandText = "create table _foobarbaz(foo, bar, baz)";
      _cmd.ExecuteNonQuery();

      // set the version and return it HAS to be the same version as the attribute
      //Info.Version = "1.0";
      //return "1.0";
      return Info.Version = "1.0";
    }
    #endregion
  }
}
