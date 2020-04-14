using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HueAPI;

namespace API_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define constants
            const string app_id = "nW1Hl7-KTG3ScjHp0alY-Md6b2o5l6qdsfiMzH-D";
            const string ip = "192.168.0.15";

            // Get lights
            Console.Out.WriteLine("Beginning light class test");

            Light.Update(ip, app_id);
            Console.Out.WriteLine("Light Count: " + Light.Count());
            for (int i=0;i<Light.Count();i++)
            {
                Light l = Light.Index(i);
                Console.Out.WriteLine("Light_" + i + ".name = " + l.name + '(' + l.modelid + ')');
            }
        }
    }
}
