using System.Reflection;

namespace Server.Models;

public class RouteValueDictionary : Dictionary<string, object>
{
    public RouteValueDictionary Load(object values)
    {
        if (values != null)
        {
            foreach (var prop in values.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                this[prop.Name] = prop.GetValue(values);
            }
        }

        return this;
    }
}