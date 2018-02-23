using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeMiner2InfluxDb.Model
{
    public class AwesomeInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string hostname { get; set; }
        public int groupId { get; set; }
        public string pool { get; set; }
        public string temperature { get; set; }
        public object hardware { get; set; }
        public Statusinfo statusInfo { get; set; }
        public Progressinfo progressInfo { get; set; }
        public Speedinfo speedInfo { get; set; }
        public Coininfo coinInfo { get; set; }
        public DateTime updatedUtc { get; set; }
        public string updated { get; set; }
        public Poollist[] poolList { get; set; }
        public Gpulist[] gpuList { get; set; }
        public object[] pgaList { get; set; }
        public object[] asicList { get; set; }
        public bool hasPool { get; set; }
        public bool hasGpu { get; set; }
        public bool hasPga { get; set; }
        public bool hasAsic { get; set; }
        public bool canReboot { get; set; }
        public bool canStop { get; set; }
        public bool canRestart { get; set; }
        public bool canStart { get; set; }
        public bool canPool { get; set; }
        public bool hasValidStatus { get; set; }
        public Metadata metaData { get; set; }
    }

    public class Statusinfo
    {
        public string statusDisplay { get; set; }
        public string statusLine3 { get; set; }
    }

    public class Progressinfo
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
    }

    public class Speedinfo
    {
        public int logInterval { get; set; }
        public string hashrate { get; set; }
        public int hashrateValue { get; set; }
        public string avgHashrate { get; set; }
        public string workUtility { get; set; }
        public object line2 { get; set; }
        public object line3 { get; set; }
    }

    public class Coininfo
    {
        public string displayName { get; set; }
        public string revenuePerDay { get; set; }
        public float revenuePerDayValue { get; set; }
        public string revenuePerMonth { get; set; }
        public float profitPerDayValue { get; set; }
        public string profitPerDay { get; set; }
        public string profitPerMonth { get; set; }
    }

    public class Metadata
    {
        public string updated { get; set; }
        public string edition { get; set; }
        public string version { get; set; }
        public object[] infoList { get; set; }
        public object[] warningList { get; set; }
    }

    public class Poollist
    {
        public int id { get; set; }
        public string name { get; set; }
        public PoolStatusinfo statusInfo { get; set; }
        public Additionalinfo additionalInfo { get; set; }
        public Priorityinfo priorityInfo { get; set; }
        public PoolProgressinfo progressInfo { get; set; }
        public string coinName { get; set; }
        public int minerID { get; set; }
        public string minerName { get; set; }
        public bool canRemove { get; set; }
        public bool canDisable { get; set; }
        public bool canEnable { get; set; }
        public bool canPrioritize { get; set; }
    }

    public class PoolStatusinfo
    {
        public string statusDisplay { get; set; }
        public object statusLine3 { get; set; }
    }

    public class Additionalinfo
    {
        public string displayUrl { get; set; }
        public string worker { get; set; }
    }

    public class Priorityinfo
    {
        public int priority { get; set; }
        public int quota { get; set; }
    }

    public class PoolProgressinfo
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
    }

    public class Gpulist
    {
        public string name { get; set; }
        public GpuStatusinfo statusInfo { get; set; }
        public Deviceinfo deviceInfo { get; set; }
        public GpuProgressinfo progressInfo { get; set; }
        public GpuSpeedinfo speedInfo { get; set; }
    }

    public class GpuStatusinfo
    {
        public string statusDisplay { get; set; }
        public string statusLine3 { get; set; }
    }

    public class Deviceinfo
    {
        public string deviceType { get; set; }
        public int gpuActivity { get; set; }
        public string intensity { get; set; }
        public object name { get; set; }
        public int gpuClock { get; set; }
        public int gpuMemoryClock { get; set; }
        public string gpuVoltage { get; set; }
        public int gpuPowertune { get; set; }
        public int fanSpeed { get; set; }
        public int fanPercent { get; set; }
        public int temperature { get; set; }
    }

    public class GpuProgressinfo
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
    }

    public class GpuSpeedinfo
    {
        public int logInterval { get; set; }
        public string hashrate { get; set; }
        public int hashrateValue { get; set; }
        public string avgHashrate { get; set; }
        public string workUtility { get; set; }
        public object line2 { get; set; }
        public object line3 { get; set; }
    }

}
