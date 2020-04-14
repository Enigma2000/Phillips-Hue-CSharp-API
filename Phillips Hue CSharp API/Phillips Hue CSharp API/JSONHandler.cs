using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace HueAPI
{
    public class JSONHandler
    {
        public static List<Light> getLights(string ip, string appId)
        {
            // Initialize our return list, and our json string
            List<Light> lights = new List<Light>();
            string json;

            // Fetch the json as bytes
            using (WebClient wc = new WebClient())
            {
                string url = "http://" + ip + "/api/" + appId + "/lights";
                json = wc.DownloadString(url);
            }
            
            // Convert the string to a JObject
            JObject jObj = JObject.Parse(json);

            // Iterate over the high-level tokens, or what our lights should be
            var e = jObj.GetEnumerator();
            while (e.MoveNext())
            {
                // Deserialize the light and add it to the list to return
                var cur = e.Current;
                var l = JsonConvert.DeserializeObject<Light>(cur.Value.ToString());
                l._setHueID(ushort.Parse(cur.Key));
                lights.Add(l);
            }
            
            return lights;
        }
    }
}
