using Common.Infrastructure.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.API
{
    public class HelperHttpLog
    {
        private string requestToken;
        private Stopwatch stopWatch;
        public HelperHttpLog()
        {
            this.requestToken = Guid.NewGuid().ToString();
            this.stopWatch = new Stopwatch();
        }
        public void LogRequestIni(string layer = "")
        {
            stopWatch.Start();
            var url = HttpContext.Current.Request.Url;
            var warn = string.Format("[{0}] - Start - {1} - {2}", layer, this.requestToken, url);
            FactoryLog.GetInstace().Debug(warn);

        }

        public void LogRequestEnd(string layer = "")
        {
            stopWatch.Stop();
            var url = HttpContext.Current.Request.Url;
            var ts = stopWatch.Elapsed;
            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
            var warn = string.Format("[{0}] - End - {1} - {2} - Tempo :{3}", layer , this.requestToken, url, elapsedTime);
            FactoryLog.GetInstace().Debug(warn);

        }
        public void LogSerialize(object model)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSaveJson"]))
            {
                var resultSerialize = JsonConvert.SerializeObject(model);
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format("log-{0}-{1}.log", "serialize", DateTime.Now.ToString("dd-MM-yyyy")));
                File.WriteAllText(path, resultSerialize);
            }
        }

    }
}
