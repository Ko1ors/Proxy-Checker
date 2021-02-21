using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Proxy_Checker
{
    public class Proxy
    {
        private string Path;
        private List<string> List = new List<string>();
        private int current = 0;
        public bool IsChecking { get; private set; }
        public bool IsProxyFileSetted { get; private set; }

        private int GoodProxiesCount = 0;
        private int CurrentProxy = 0;

        private int SecondsBetweenMsg = 3;

        public delegate void OnProxyCheckUpdate(string msg);
        public event OnProxyCheckUpdate Notify;

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

        public bool Start()
        {
            if(!IsChecking && IsProxyFileSetted)
            {
                var thread = new Thread(new ThreadStart(CheckProxies));
                thread.Start();
                return true;
            }
            return false;
        }

        private void CheckProxies()
        {
            if (!IsChecking)
            {
                List.Clear();
                IsChecking = true;
                GoodProxiesCount = 0;
                CurrentProxy = 0;
                SendMessage();
                var msgTime = DateTime.Now;
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
                        if (msgTime.AddSeconds(SecondsBetweenMsg) < DateTime.Now)
                        {
                            SendMessage();
                            msgTime = DateTime.Now;
                        }
                    }
                    catch (Exception)
                    {

                    }
                });
                IsChecking = false;
            }
        }

        private void SendMessage()
        {
            Notify?.Invoke(ToString());
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
            return "Size: " + GetProxyCount() + " Total handled: " + CurrentProxy + " Good: " + GoodProxiesCount + " Bad: " + (CurrentProxy - GoodProxiesCount);
        }
    }
}
