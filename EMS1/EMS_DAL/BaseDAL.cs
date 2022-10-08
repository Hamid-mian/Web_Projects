using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EMS_bo;

namespace EMS_DAL
{
   public class BaseDAL
    {
        public void Save(string text,string fileName)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamWriter sw = new StreamWriter(filePath, append: true);
            sw.WriteLine(text);
            sw.Close();
        }
        internal List<String> Read(string fileName)
        {
            List<string> list = new List<string>();
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            string line = string.Empty;
            while ((line=sr.ReadLine()) != null)
            {
                list.Add(line);
            }
            return list;
        }
    }
}
