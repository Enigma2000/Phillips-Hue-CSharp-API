using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HueAPI
{
    [Serializable()]
    public class Light : ISerializable
    {
        // Static members
        private static List<Light> lights = new List<Light>();
        public static string IP_ADDRESS = null;
        public static string USER_ID = null;

        // Product info
        public string name = null;
        public string modelid = null;
        public string bulbtype = null;
        public string manufacturername = null;
        public string productname = null;
        public string uniqueid = null;
        public string productid = null;

        // Light state info
        public bool active = false;
        public ushort brightness = 0;
        public int hue = -1;
        public ushort saturation = 0;
        public string mode = null;

        // Internal info
        public bool reachable = false;
        public bool initialized = false;
        private ushort hueid;
        private bool setID = false;

        // Deserialization method to simplify JSONHandler fetch
        public Light(SerializationInfo info, StreamingContext ctxt)
        {
            name = info.GetString("name");
            modelid = info.GetString("modelid");
            bulbtype = info.GetString("type");
            manufacturername = info.GetString("manufacturername");
            productname = info.GetString("productname");
            uniqueid = info.GetString("uniqueid");
            productid = info.GetString("productid");

            // Get the "state" key
            Dictionary<string, JToken> state = 
                (Dictionary<string, JToken>)(info.GetValue("state", typeof(Dictionary<string,JToken>)));

            // Convert internal json to our members
            active = (bool)(state["on"]);
            brightness = (ushort)(state["bri"]);
            hue = (int)(state["hue"]);
            saturation = (ushort)(state["sat"]);
            reachable = (bool)(state["reachable"]);
            mode = (string)(state["mode"]);

            // Set our initialized to true
            initialized = true;
        }

        // Serialization method
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            Dictionary<string, JToken> state = new Dictionary<string, JToken>();
            state.Add("on", active);
            state.Add("bri", brightness);
            state.Add("hue", hue);
            state.Add("sat", saturation);
            state.Add("reachable", reachable);
            state.Add("mode", mode);

            info.AddValue("name",name);
            info.AddValue("modelid", modelid);
            info.AddValue("type", bulbtype);
            info.AddValue("manufacturername", manufacturername);
            info.AddValue("productname", productname);
            info.AddValue("productid", productid);
            info.AddValue("uniqueid", uniqueid);
            info.AddValue("state", state);
        }

        public bool update()
        {
            return true;
        }

        public static bool Update()
        {
            // Updates the lights internally. Requires IP_ADDRESS and USER_ID to be non-null.
            if (IP_ADDRESS!=null || USER_ID!=null)
            {
                return false;
            }
            return Update(IP_ADDRESS, USER_ID);
        }

        public static bool Update(string ip, string user)
        {
            // Update internal IP and user id
            IP_ADDRESS = ip;
            USER_ID = user;

            // Clear lights
            lights.Clear();

            // Try to update the lights through the JSON Handler
            try
            {
                lights = JSONHandler.getLights(ip,user);
            }
            catch (Exception)
            {
                return false;
            }
            
            // Return true if everything went through, false otherwise
            return true;
        }

        // Return the number of lights in the internal List
        public static int Count()
        {
            return lights.Count;
        }

        // Return the light corresponding to the index in the internal List
        public static Light Index(int index)
        {
            return lights[index];
        }

        public void _setHueID(ushort id)
        {
            // Prevent overwriting of ID once it is set
            if (!setID)
            {
                setID = true;
                hueid = id;
            }
        }
    }
}
