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
        public static string Path = AppDomain.CurrentDomain.BaseDirectory + "proxy.txt";
        public List<string> List = new List<string>();
        public string LastBackupPath = "";
        public bool IsBackup = true;
        private int current = 0;
        public bool IsChecking { get; private set; }
        private int GoodProxiesCount = 0;
        public int CurrentProxyIndex = 0;

        public Proxy()
        {
            List = GetProxyList();
        }
        public void CheckProxies()
        {
            if (!IsChecking)
            {
                List.Clear();
                IsChecking = true;
                GoodProxiesCount = 0;
                CurrentProxyIndex = 0;
                Parallel.ForEach(File.ReadAllLines(Proxy.Path).ToList(), new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount * 10 }, (proxy) =>
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://google.com");
                    request.Timeout = 10000;
                    var p = proxy.Split(':');
                    try
                    {
                        request.Proxy = new WebProxy(p[0], Convert.ToInt32(p[1]));
                        CurrentProxyIndex++;
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        GoodProxiesCount++;
                        List.Add(proxy);
                        SaveGoodProxy();
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
            return File.ReadLines(Proxy.Path).Count();
        }

        public static List<string> GetProxyList()
        {
            return File.ReadAllLines(Proxy.Path).ToList();
        }
        public void SaveProxies()
        {
            LastBackupPath = GetBackupPath();
            File.WriteAllLines(LastBackupPath, File.ReadLines(Path));
            File.WriteAllLines(Path, List);
        }
        public void SaveGoodProxy()
        {
            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "goodproxy.txt", List);
        }
        public static string GetBackupPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "proxy" + DateTime.Today + ".txt";
        }

        public override string ToString()
        {
            return "Size: " + GetProxyCount() + "\nTotal handled: " + CurrentProxyIndex + "\nGood: " + GoodProxiesCount + "\nBad: " + (CurrentProxyIndex - GoodProxiesCount) + "\n";
        }
    }
}
