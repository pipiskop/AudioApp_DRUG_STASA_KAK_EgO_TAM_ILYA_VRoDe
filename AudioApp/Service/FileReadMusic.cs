using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace AudioApp.Service
{
   
    class FileReadMusic
    {
        readonly string sourceDir;
        public FileReadMusic(string sourceDir)
        {
            this.sourceDir = sourceDir;
        }
        public List<string> ToLoad()
        {
            bool fileexist = File.Exists(sourceDir);
            if (!fileexist)
            {
                File.CreateText(sourceDir).Dispose();
                return new List<string>();
            }
            using(StreamReader read = File.OpenText(sourceDir))
            {
                var filetext = read.ReadToEnd();
                return JsonConvert.DeserializeObject<List<string>>(filetext);
            }
        } 
        public void ToSave(List<string> pa)
        {
            using(StreamWriter writer = File.CreateText(sourceDir))
            {
                string ser = JsonConvert.SerializeObject(pa);
                writer.Write(ser);
            }
        }
    }
}
