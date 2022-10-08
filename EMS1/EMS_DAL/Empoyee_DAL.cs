using EMS_bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_DAL
{
    public class Empoyee_DAL:BaseDAL
    {
        public void SaveEmployee(Employee_bo bo)
        {
            string text = $"{bo.Name},{bo.Age},{bo.salary}";
            Save(text, "EmployeeData.csv");

        }
        public List<Employee_bo> ReadEmployee()
        {
            List<String> stringList = Read("EmployeeData.csv");
            List<Employee_bo> emplist = new List<Employee_bo>();
            foreach(string s in stringList)
            {
                string[] data = s.Split(",");
                Employee_bo e = new Employee_bo();
                e.Name = data[0];
                e.Age = int.Parse(data[1]);
                e.salary = decimal.Parse(data[2]);
                emplist.Add(e);
            }
            return emplist;
        }
    }
}
