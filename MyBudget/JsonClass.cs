using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget
{
    internal class JsonClass<T>
    {
        public static void Serialize<T>(T notes, string fileName)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string json = JsonConvert.SerializeObject(notes);
            File.WriteAllText(desktop + "\\" + fileName, json);
        }
        public static T Deserialize<T>(string fileName)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string json = File.ReadAllText(desktop + "\\" + fileName);
            T notes = JsonConvert.DeserializeObject<T>(json);
            return notes;
        }
    }
}
