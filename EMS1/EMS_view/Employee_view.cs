using EMS_BLL;
using EMS_bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_view
{
    public class Employee_view
    {
        public void GetInput()
        {
            Console.WriteLine("Enter Employee Name; ");
            string empName = Console.ReadLine();
            Console.WriteLine("Enter Employee Age: ");
            int age = System.Convert.ToInt32(Console.ReadLine());
            Employee_bo bo = new Employee_bo { Name = empName, Age = age };
            Employee_BLL bll = new Employee_BLL();
            bll.SaveEmployee(bo);
        }
        public void display()
        {
            Employee_BLL bll = new Employee_BLL();
            List<Employee_bo> list = bll.ReadEmployee();
            foreach(Employee_bo e in list)
            {
                Console.WriteLine($"Name :{e.Name} Age :{e.Age} Salary :{e.salary}");
            }
        }
    }
}
