using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ChaosGameScreensaver
{
    class RegistryStorage
    {
        public static void SaveData(List<RenderData> obj)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            
            formatter.Serialize(ms, obj);
            var data = ms.ToArray();
            
            Microsoft.Win32.RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ChaosGameScreenSaver");
            key.SetValue("data", data, RegistryValueKind.Binary);

        }
        
        public static List<RenderData> GetData()
        {
            List<RenderData> list = new List<RenderData>();
            RegistryKey key;

            try
            {
                key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ChaosGameScreenSaver");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return list;
            }

            if (key == null)
                return list;


            object data = key.GetValue("data", null);
            if (data != null)
            {
                
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream((byte[])data);
                list = (List<RenderData>)formatter.Deserialize(ms);
                Console.WriteLine(list.Count);
            }

            return list;
        }

    }
}
