using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;

namespace RealTimeWebAnalytics
{
    public static class Utils
    {
        public static string UnwrapQuotes(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length >= 2 && str[0] == '"' && str[str.Length - 1] == '"')
            {
                return str.Substring(1, str.Length - 2);
            }
            return str;
        }

        public static IPAddress ParseIP(string str)
        {
            return IPAddress.Parse(str);
        }

        public static uint GetIntRepresentation(this IPAddress address)
        {
            var bytes = address.GetAddressBytes();
            uint result = 0;
            for (int i = 0; i < 4; i++)
            {
                result <<= 8;
                result += bytes[i];
            }
            return result;
        }

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = new RouteValueDictionary(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }
    }
}