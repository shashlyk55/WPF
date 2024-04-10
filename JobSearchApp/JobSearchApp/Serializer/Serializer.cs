using JobSearchApp.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearchApp.Serializer
{
    public class Serializer
    {
        public static void SerializationList(ObservableCollection<Vacancy> list, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
            string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            
            //File.WriteAllText(projectFolder + filePath, jsonData);
            File.WriteAllText(System.IO.Path.Combine(projectFolder, filePath), jsonData);
        }

        public static ObservableCollection<Vacancy> DeserializationList(string filePath)
        {
            ObservableCollection<Vacancy> list = null;
            string json = File.ReadAllText(filePath);
            list = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<Vacancy>>(json);
            return list;
        }

    }
}
