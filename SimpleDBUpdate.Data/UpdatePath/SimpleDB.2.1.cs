using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDBUpdate.Data
{
  public partial class SimpleDB
  {
    #region Version 2.1
    [SimpleDBError("2.1")]
    public string CreateDatabase21Error
    {
      get { return "Error - Failed to create _foobarbas21"; } // or other more descriptive name
    }

    [SimpleDBVersion("2.1")]
    public string CreateDatabase21()
    {
      // Create the table(s) I need...
      _cmd.CommandText = "create table _foobarbas21(foo, bar, bas)";
      _cmd.ExecuteNonQuery();

      // set the version and return it HAS to be the same version as the attribute
      //Info.Version = "2.1";
      //return "2.1";
      return Info.Version = "2.1";
    }
    #endregion
  }
}
