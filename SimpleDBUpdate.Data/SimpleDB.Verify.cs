using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SimpleDBUpdate.Data
{
  public partial class SimpleDB
  {
    private void VerifyDatabase()
    {
      // This one doesn't need any "crap", it figures it all out
      Assembly assembly = Assembly.GetExecutingAssembly();
      Dictionary<string, string> methodList = new Dictionary<string, string>();
      Dictionary<string, string> properties = new Dictionary<string, string>();
      List<string> sort = new List<string>();

      SimpleDBVersionAttribute version;
      SimpleDBErrorAttribute error;

      Type type = typeof(SimpleDB);
      MethodInfo[] methods = type.GetMethods();
      foreach (MethodInfo method in methods)
      {
        foreach (Attribute attr in method.GetCustomAttributes(typeof(SimpleDBVersionAttribute), false))
        {
          version = attr as SimpleDBVersionAttribute;
          if (null != version)
          {
            methodList.Add(version.Version, method.Name);
            sort.Add(version.Version);
          }
        }
      }

      foreach (PropertyInfo property in type.GetProperties())
      {
        foreach (Attribute attr in property.GetCustomAttributes(typeof(SimpleDBErrorAttribute), false))
        {
          error = attr as SimpleDBErrorAttribute;
          if (null != error)
            properties.Add(error.Version, property.Name);
        }
      }

      // Figure out the "sort" order...
      sort = sort.OrderBy(s => s.Split('.').Aggregate(0, (n, v) => n * 100 + Int32.Parse(v))).ToList();

      // This just removes the versions that are already applied, leaving the ones that need to be done
      // This should allow for a patch value of 1.9.1 to still be applied normally for new users
      // and still be applied for existing ones.
      string[] versions = Info.GetAppliedVersions();
      foreach (string v in versions)
        sort.Remove(v);

      foreach (string ver in sort)
      {
        MethodInfo meth = type.GetMethod(methodList[ver]);
        PropertyInfo prop = type.GetProperty(properties[ver]);
        string msg = prop.GetValue(this, null).ToString();
        try
        {
          object obj = meth.Invoke(this, null);
          if ((string)obj != ver) throw new SimpleDBException(msg);
        }
        catch
        {
          // Assume the table was created...?
        }
      }
    }
  }
}
