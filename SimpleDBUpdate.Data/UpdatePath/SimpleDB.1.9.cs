using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDBUpdate.Data
{
  public partial class SimpleDB
  {
    #region Version 1.9
    [SimpleDBError("1.9")]
    public string CreateDatabase19Error
    {
      get { return "Error - Failed to create _foobarbas19"; }
    }

    [SimpleDBVersion("1.9")]
    public string CreateDatabase19()
    {
      // Create the table(s) I need...
      _cmd.CommandText = "create table _foobarbas19(foo, bar, bas)";
      _cmd.ExecuteNonQuery();

      // set the version and return it HAS to be the same version as the attribute
      //Info.Version = "1.9";
      //return "1.9";
      return Info.Version = "1.9";
    }
    #endregion
  }
}
