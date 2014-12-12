using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDBUpdate.Data
{
  public partial class SimpleDB
  {
    #region Version 1.1
    [SimpleDBError("1.1")]
    public string CreateDatabase2Error
    {
      get { return "Error - Failed to create _foobarbas"; }
    }

    [SimpleDBVersion("1.1")]
    public string CreateDatabase2()
    {
      // Create the table(s) I need...
      _cmd.CommandText = "create table _foobarbas(foo, bar, bas)";
      _cmd.ExecuteNonQuery();

      // set the version and return it HAS to be the same version as the attribute
      //Info.Version = "1.1";
      //return "1.1";
      return Info.Version = "1.1";
    }
    #endregion
  }
}
