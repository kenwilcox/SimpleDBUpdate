using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleDBUpdate.Data
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  internal class SimpleDBVersionAttribute : Attribute
  {
    protected string _version;

    public SimpleDBVersionAttribute(string version)
    {
      _version = version;
    }

    public string Version
    {
      get { return _version; }
      set { _version = value; }
    }
  }

  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
  internal class SimpleDBErrorAttribute : Attribute
  {
    protected string _version;

    public SimpleDBErrorAttribute(string version)
    {
      _version = version;
    }

    public string Version
    {
      get { return _version; }
      set { _version = value; }
    }
  }
}
