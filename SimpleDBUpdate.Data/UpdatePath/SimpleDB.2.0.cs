using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDBUpdate.Data
{
  public partial class SimpleDB
  {
    #region Version 2.0
    [SimpleDBError("2.0")]
    public string CreateDatabase3Error
    {
      get { return "Error - Failed to create _foobarbas20!"; }
    }

    [SimpleDBVersion("2.0")]
    public string CreateDatabase3()
    {
      // Create the table(s) I need...
      _cmd.CommandText = "create table _foobarbas20(foo, bar, bas)";
      _cmd.ExecuteNonQuery();

      // set the version and return it HAS to be the same version as the attribute
      //Info.Version = "2.0";
      //return "2.0";
      return Info.Version = "2.0";
    }
    #endregion
  }
}
