using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using RealTimeWebAnalytics.Models;

namespace RealTimeWebAnalytics.Services
{
    public class GeoLocator
    {
        private readonly List<IpCountryInfo> _countryInfos = new List<IpCountryInfo>();

        private GeoLocator(string ipCountry, string ipLocation, string ipCity)
        {
            ParseCountry(ipCountry);
            // todo: add cities
        }

        private void ParseCountry(string ipCountry)
        {
            using (var reader = new StreamReader(ipCountry))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    var atoms = str.Split(new[] {','});
                    if (atoms.Length == 6)
                    {
                        _countryInfos.Add(new IpCountryInfo
                                              {
                                                  Code = atoms[4].UnwrapQuotes(),
                                                  Name = atoms[5].UnwrapQuotes(),
                                                  StartAddress = Utils.ParseIP(atoms[0].UnwrapQuotes()),
                                                  EndAddress = Utils.ParseIP(atoms[1].UnwrapQuotes())
                                              });
                    }
                }
            }
        }

        public static GeoLocator Instance { get; private set; }

        public static void Init(string ipCountry, string ipLocation, string ipCity)
        {
            Instance = new GeoLocator(ipCountry, ipLocation, ipCity);
        }

        public async Task Locate(Visit visit)
        {
            var addr = visit.IP;
            int l = 0, h = _countryInfos.Count - 1, m = 0;
            bool found = false;
            while (h - l > 1)
            {
                m = (l + h)/2;
                var comparisonRes = _countryInfos[m].CompareWithRange(addr);
                if (comparisonRes == 0)
                {
                    found = true;
                    break;
                }
                else if (comparisonRes < 0) l = m;
                else h = m;
            }
            if (!found)
            {
                if (_countryInfos[l].CompareWithRange(addr) == 0)
                {
                    found = true;
                    m = l;
                }
                else if (_countryInfos[h].CompareWithRange(addr) == 0)
                {
                    found = true;
                    m = h;
                }
            }

            if (found)
            {
                visit.Country = _countryInfos[m].Name;
            }
        }

        private class IpCountryInfo
        {
            public IPAddress StartAddress { get; set; } 
            public IPAddress EndAddress { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }

            public int CompareWithRange(IPAddress address)
            {
                var start = StartAddress.GetIntRepresentation();
                var end = EndAddress.GetIntRepresentation();
                var addr = address.GetIntRepresentation();
                if (addr < start) return -1;
                if (addr > end) return 1;
                return 0;
            }
        }
    }
}