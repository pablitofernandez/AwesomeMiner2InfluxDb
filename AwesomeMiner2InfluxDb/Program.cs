using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdysTech.InfluxDB.Client.Net;
using AwesomeMiner2InfluxDb.Model;

namespace AwesomeMiner2InfluxDb
{
    class Program
    {
        static void Main(string[] args)
        {
            string sSource;
            string sLog;
            string sEvent;

            sSource = "AwesomeMiner2InfluxDb";
            sLog = "Application";

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var host = appSettings["Host"];
                var minerId = appSettings["MinerId"];
                var url = $"http://{host}/api/miners/{minerId}";
                var request = WebRequest.Create(url);
                request.ContentType = "application/json; charset=utf-8";
                string text;
                var response = (HttpWebResponse)request.GetResponse();
                Console.WriteLine("Reading json data from AwesomeMiner api");
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();
                }

                AwesomeInfo awesomeInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<AwesomeInfo>(text);
                Console.WriteLine("Json received and parsed");
                var influxUrl = appSettings["InfluxDbHost"];
                var influxDbName = appSettings["InfluxDbName"];

                Console.WriteLine($"Connecting to Influxdb @ {influxUrl} DBName: {influxDbName}");
                InfluxDBClient client = new InfluxDBClient($"http://{influxUrl}");
                client.CreateDatabaseAsync(influxDbName).Wait();
                var utcNow = DateTime.UtcNow;
                var valMixed = new InfluxDatapoint<InfluxValueField>();
                valMixed.UtcTimestamp = utcNow;
                valMixed.Tags.Add("MinerId", awesomeInfo.id.ToString());
                valMixed.Tags.Add("Algo", awesomeInfo.coinInfo.displayName);
                valMixed.Tags.Add("Pool", awesomeInfo.poolList[0].name);
                var hashRate = awesomeInfo.speedInfo.hashrate.Substring(0, awesomeInfo.speedInfo.hashrate.IndexOf(" "));
                var hashRateDouble = double.Parse(hashRate);
                valMixed.Fields.Add("HashRate", new InfluxValueField(hashRateDouble));
                var profitPerday = double.Parse(awesomeInfo.coinInfo.profitPerDay.Substring(1));
                var profitPerMonth = double.Parse(awesomeInfo.coinInfo.profitPerMonth.Substring(1));
                var revenuePerDay = double.Parse(awesomeInfo.coinInfo.revenuePerDay.Substring(1));
                var revenuePerMonth = double.Parse(awesomeInfo.coinInfo.revenuePerMonth.Substring(1));
                valMixed.Fields.Add("ProfitPerDay", new InfluxValueField(profitPerday));
                valMixed.Fields.Add("ProfitPerMonth", new InfluxValueField(profitPerMonth));
                valMixed.Fields.Add("RevenuePerDay", new InfluxValueField(revenuePerDay));
                valMixed.Fields.Add("RevenuePerMonth", new InfluxValueField(revenuePerMonth));


                valMixed.MeasurementName = $"Miner.{awesomeInfo.id}";

                Console.WriteLine("Trying to write miner data to DB");
                client.PostPointAsync(influxDbName, valMixed).Wait();
                EventLog.WriteEntry(sSource, "Miner data written successfully", EventLogEntryType.Information);
                Console.WriteLine("Miner data written successfully");

                foreach (var gpuInfo in awesomeInfo.gpuList)
                {
                    var gpuToInflux = new InfluxDatapoint<InfluxValueField>();
                    gpuToInflux.UtcTimestamp = utcNow;
                    gpuToInflux.Tags.Add("GPUName", gpuInfo.name);
                    gpuToInflux.Fields.Add("Temperature", new InfluxValueField(gpuInfo.deviceInfo.temperature));
                    gpuToInflux.Fields.Add("FanPercent", new InfluxValueField(gpuInfo.deviceInfo.fanPercent));
                    gpuToInflux.Fields.Add("FanSpeed", new InfluxValueField(gpuInfo.deviceInfo.fanSpeed));
                    gpuToInflux.Fields.Add("GpuClock", new InfluxValueField(gpuInfo.deviceInfo.gpuClock));
                    gpuToInflux.Fields.Add("MemoryClock", new InfluxValueField(gpuInfo.deviceInfo.gpuMemoryClock));
                    var intensity = double.Parse(gpuInfo.deviceInfo.intensity);
                    gpuToInflux.Fields.Add("Intensity", new InfluxValueField(intensity));
                    var gpuHashrate = gpuInfo.speedInfo.hashrate.Substring(0, gpuInfo.speedInfo.hashrate.IndexOf(" "));
                    var gpuHashrateDouble = double.Parse(gpuHashrate);
                    gpuToInflux.Fields.Add("Speed", new InfluxValueField(gpuHashrateDouble));

                    gpuToInflux.MeasurementName = gpuInfo.name.Replace(' ', '_');

                    Console.WriteLine("Trying to write GPU info to DB");
                    client.PostPointAsync(influxDbName, gpuToInflux).Wait();
                    Console.WriteLine("GPU info written successfully");
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                EventLog.WriteEntry(sSource, e.Message, EventLogEntryType.Error);
            }
        }
    }
}
