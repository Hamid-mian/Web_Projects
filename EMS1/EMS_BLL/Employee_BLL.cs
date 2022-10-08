using EMS_bo;
using EMS_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BLL
{
   public class Employee_BLL
    {
        public void SaveEmployee(Employee_bo bo)
        {
            if (bo.Age <= 20)
            {
                bo.salary = 10000;
            }
            else if(bo.Age>20&&bo.Age<50)
            {
                bo.salary = 40000;
            }
            else 
            {
                bo.salary = 70000;
            }

            Empoyee_DAL dal = new Empoyee_DAL();
            dal.SaveEmployee(bo);
        }
        public List<Employee_bo> ReadEmployee()
        {
            Empoyee_DAL dal = new Empoyee_DAL();
            return dal.ReadEmployee();
        }
    }
}
