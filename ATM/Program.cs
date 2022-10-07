using ATM_view;
using System;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*********************Welcome to ATM Management System*********************");

            //Making an object of presentation layer

            ATMview view = new ATMview();

            //Calling input function of presentation layer

            view.choseLogin();

        }
    }
}
