using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleDBUpdate.Data
{
  public class SimpleDBException: Exception
  {
    public string ErrorMessage
    {
      get { return base.Message.ToString(); }
    }
    public SimpleDBException(string errorMessage) : base(errorMessage) { }
    public SimpleDBException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
  }
}
