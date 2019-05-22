using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Mid_Pro
{
    public class OpenWeatherMapProxy
    {
        public async static Task<RootObject> getweather(string city)
        {
            var http = new HttpClient();
            string url = "https://www.apiopen.top/weatherApi?city=";
            url += city;
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(ms);
            return data;
        }
    }
    [DataContract]
    public class Yesterday
    {
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string high { get; set; }
        [DataMember]
        public string fx { get; set; }
        [DataMember]
        public string low { get; set; }
        [DataMember]
        public string fl { get; set; }
        [DataMember]
        public string type { get; set; }
    }
    [DataContract]
    public class Forecast
    {
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string high { get; set; }
        [DataMember]
        public string fengli { get; set; }
        [DataMember]
        public string low { get; set; }
        [DataMember]
        public string fengxiang { get; set; }
        [DataMember]
        public string type { get; set; }
    }
    [DataContract]
    public class Data
    {
        [DataMember]
        public Yesterday yesterday { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string aqi { get; set; }
        [DataMember]
        public List<Forecast> forecast { get; set; }
        [DataMember]
        public string ganmao { get; set; }
        [DataMember]
        public string wendu { get; set; }
    }
    [DataContract]
    public class RootObject
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public Data data { get; set; }
    }
}
