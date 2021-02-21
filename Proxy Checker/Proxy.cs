using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Proxy_Checker
{
    public class Proxy
    {
        public string Path;
        public List<string> List = new List<string>();
        private int current = 0;
        public bool IsChecking { get; private set; }
        public bool IsProxyFileSetted { get; private set; }

        private int GoodProxiesCount = 0;
        public int CurrentProxy = 0;

        public bool SetProxyFilePath(string path)
        {
            IsProxyFileSetted = false;
            if (File.Exists(path))
            {
                Path = path;
                List = GetProxyList();
                if (List.Count > 0)
                    IsProxyFileSetted = true;
            }
            return IsProxyFileSetted;
        }

        public void CheckProxies()
        {
            if (!IsChecking)
            {
                List.Clear();
                IsChecking = true;
                GoodProxiesCount = 0;
                CurrentProxy = 0;
                Parallel.ForEach(File.ReadAllLines(Path).ToList(), new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount * 10 }, (proxy) =>
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://google.com");
                    request.Timeout = 10000;
                    var p = proxy.Split(':');
                    try
                    {
                        request.Proxy = new WebProxy(p[0], Convert.ToInt32(p[1]));
                        CurrentProxy++;
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        GoodProxiesCount++;
                        List.Add(proxy);
                    }
                    catch (Exception)
                    {

                    }
                });
                IsChecking = false;
            }
        }

        public int GetNextProxyIndex()
        {
            if (current == GetProxyCount()) current = 0;
            return current++;
        }

        public int GetProxyCount()
        {
            return File.ReadLines(Path).Count();
        }

        public List<string> GetProxyList()
        {
            return File.ReadAllLines(Path).ToList();
        }

        public void SaveProxy(string path)
        {
            File.WriteAllLines(path, List);
        }

        public override string ToString()
        {
            return "Size: " + GetProxyCount() + "\nTotal handled: " + CurrentProxy + "\nGood: " + GoodProxiesCount + "\nBad: " + (CurrentProxy - GoodProxiesCount) + "\n";
        }
    }
}
