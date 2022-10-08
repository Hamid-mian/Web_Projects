using EMS_view;
using System;

namespace EMS1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome To Employee Management System");
            Employee_view view = new Employee_view();
            //  view.GetInput();
            view.display();
        }
    }
}
