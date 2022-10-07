//BSEF19A033 MUHAMMAD HAMID IQBAL
//(Note:Assalam O Alaikum Kindly Note that each if else or while loop in presentation layer purpose 
//is to get input or call function. No loop or if else is used for logic purpose Jaza ka Allah)

using ATM_BLL;
using ATM_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ATM_view
{
    public class ATMview
    {
        public void choseLogin()                                                        //Function use to get input what kind of login he want
        {
            Console.WriteLine("1----Login as customer"+ "\n2----Login as admin");         
            int chose = 0;
            do                                                                           //loop which forces user to enter only integer value
            {
                Console.WriteLine("Select one of the above options ");
            } while (!int.TryParse(Console.ReadLine(), out chose));
            while (chose < 1 || chose > 2)                                               //Getting input again if it is other than 1,2(options)
            {
                Console.WriteLine("Invalid option");
                do
                {
                    Console.WriteLine("Select one of the above options ");
                } while (!int.TryParse(Console.ReadLine(), out chose));
            }
            if (chose==1)                                                                //Calling Functions according to user requirment
            {
                getInputCustomer();
            } else if(chose==2) {
                getInputAdmin();
            }
        }

        public void getInputCustomer()                                                  //Customer login function
        {
            ATMbll bll = new ATMbll();                                                  //Making an object of business logic layer

            Console.WriteLine("Please Enter User Id:");                                 //Getting input for login
            string userId = null;
            userId = Console.ReadLine();
            int pin = 0;
            do
            {
              Console.WriteLine("Enter 5 digit pin code: ");
            } while (!int.TryParse(Console.ReadLine(), out pin));
            ATMbo bo = new ATMbo { userIdName = userId, userIdPin = pin };              //initializing methods of business object dll file

            int success = 5;
            if(userId!=null)                                                            //Calling an login of Business logic layer
            {
                 success = bll.customerLogin(bo);
            }
            if (success == 0)                                                           ////Showing console messages
            {
                Console.WriteLine("Invalid UserName or Password:");
            }else if (success == 1) {                                                    //in case login successfull getting him to customer menu
                Console.WriteLine("Login Successful:"); 
                customerMenu(bo);
            } else {
                Console.WriteLine("You have entered invalid pin 3 time your account has been disabled\n Please contact Manager:");
            }

        }

        public void customerMenu(ATMbo bo)                                               //function to show user customer menu and get his/her choise
        {
            Console.WriteLine("1----Withdraw Cash\n2----Cash Transfer\n3----Deposit Cash");
            Console.WriteLine("4----Display Balance\n5----Exit");
            int check = 0;                                                               //loop which forces user to enter only integer value
            do
            {
               Console.WriteLine("\nPlease select one ofthe above Options");
            } while (!int.TryParse(Console.ReadLine(), out check));
            if (check==1)                                                                 //calling functions according to user requirment
            {
                withDrawCash(bo);
            } else if(check==2) {
                cashTransfer(bo);
            } else if(check==3) {
                cashDeposit(bo);
            }else if(check==4) {
                displayBalance(bo);
            }  else if(check==5) {
                Console.WriteLine("Thanks for using our service");
            } else {
                Console.WriteLine("You have Entered invalid option");
            }
        }

       public void withDrawCash(ATMbo bo)                                                   //withdraw function only used to get customer choice either he wants
        {                                                                                   //fast cash or normal cash
            int dr = 0;
            while (dr != 1)
            {
                Console.WriteLine("1----Fast Cash\n2----Normal Cash\n3----Back to main menu");
                int check = 0;
                do                                                                          //loop which forces user to enter only integer value
                {
                    Console.WriteLine("\nPlease select one ofthe above Options");
                } while (!int.TryParse(Console.ReadLine(), out check));
                if (check == 1)                                                             //Calling function according to user requirment
                {
                    fastCash(bo);
                    dr = 1;
                } else if(check==2) {
                    normalCash(bo);
                    dr = 1;
                } else if(check==3) {
                    customerMenu(bo);
                    dr = 1;
                } else  {
                    Console.WriteLine("PLease Select from given options\n");
                }
            }
        }

        public void fastCash(ATMbo bo)                                                       //Functions shows fast cash menu get user value send that value 
        {                                                                                    //to BLL layer and tells user about results of his/her transaction
            int dr = 0,check=0;
            while (dr != 1)
            {
                Console.WriteLine("1----500\n2----1000\n3----2000\n4----5000\n5----10000\n6----15000\n7----20000");
                do                                                                            //loop which forces user to enter only integer value
                {
                  Console.WriteLine("\nPlease select one of the denominations of money\n");
                } while (!int.TryParse(Console.ReadLine(), out check));
                if (check > 0 && check < 8)
                {
                    dr = 1;
                } else  {
                    Console.WriteLine("Please Select valid option\n");
                }
            }
            ATMbll bll = new ATMbll();                                                          //BLL layer object
            bo.cash=bll.selectOption(check);                                                    //function used to store rupee to variable according to user input
            bo = bll.isValidLimit(bo);                                                          //Function checks is user out of daily limit or not 

            Console.Write("Are ypu sure you want to withdraw Rs."+ bo.cash+ "(Y/N)");
            string answer;
            dr = 0;
            while(dr!=1)                                                                        //Loop will not terminate until user enters y,Y,N,n
            {
                answer=Console.ReadLine();
                if (answer == "Y" || answer == "y")
                {
                    if (bo.limit >= bo.cash)
                    {
                        if (bll.isValidBalance(bo) == true)                                     //If user have balance equal to or more then the given amount then 
                        {                                                                       //he/she would be allowed for transaction
                            bo.limit -= bo.cash;
                            bo = bll.fastCash(bo);                                              //function to decrement in balance and do other 
                            bll.transcriptInsertion(bo);                                        //function to store transcript in database
                            bll.updateLimit(bo);                                                //function which will update the daily limit
                            Console.WriteLine("Cash Successfully Withdrawn ");
                        } else {
                            Console.WriteLine("You have insuficient balance for this transaction:");
                            bo.cash = 0;
                            bo = bll.fastCash(bo);
                        }
                        dr = 1;
                    }  else {
                        Console.WriteLine("Sorry You are out of daily limit. You cannot withdraw more then " + bo.limit + " today");
                        bo.cash = 0;
                        bo = bll.fastCash(bo);
                        dr = 1;
                    }
                }  else if (answer == "N" || answer == "n") {
                    Console.WriteLine("Cash withdrawn Unsuccessfull");
                    dr = 1;
                } else {
                    Console.WriteLine("Please select valid option (Y/N)");
                }
            }
            bo.transactionType = "withdrawn:";
            printReciept(bo);
        }

        public void normalCash(ATMbo bo)                                                            //Function get input of withdrawal amount and call bll layer functions
        {
            ATMbll bll = new ATMbll();
            bo = bll.isValidLimit(bo);

            int check = 0;
            do                                                                                       //loop which forces user to enter only integer value
            {
              Console.Write("Enter the withdrawal amount: ");
            } while (!int.TryParse(Console.ReadLine(), out check));
            bo.cash = check;

            if (bo.limit >= bo.cash)                                                                  //if limit is more then cash then tranaction will be allowed
            {
                if (bll.isValidBalance(bo) == true)                                                   //checking is balance greater or equal to cash or not
                {
                    bo.limit -= bo.cash;
                    bo = bll.normalCash(bo);                                                          //Calling BLL functions
                    bll.transcriptInsertion(bo);
                    bll.updateLimit(bo);
                    Console.WriteLine("Cash Successfully Withdrawn ");
                }else {                                                                               //error message in case of low balance
                    Console.WriteLine("You have insuficient balance for this transaction:");
                    bo.cash = 0;
                    bo = bll.normalCash(bo);
                }
            } else {
                Console.WriteLine("Sorry You are out of daily limit." +                               //error message in case amount to be withdrawn is more
                    " You cannot withdraw more then " + bo.limit + " today");                         //then limit
                bo.cash = 0;
                bo = bll.normalCash(bo);
            }
            bo.transactionType = "withdrawn:";
            printReciept(bo);
        }
        public void cashTransfer(ATMbo bo)                                                            //function used to transfer cash from one account to other
        {
            int dr = 0;
            ATMbll bll = new ATMbll();
            while(dr!=1)                                                                              //Loop will not terminate untill user enter amount in
            {                                                                                         //multiple of 500
                int check = 0;
                do                                                                                    //loop which forces user to enter only integer value
                {
                  Console.WriteLine("Enter Amount in multiples of 500: ");
                } while (!int.TryParse(Console.ReadLine(), out check));
                bo.cash = check;
                if ((bo.cash%500)==0)
                {
                    dr = 1;
                } else{
                    Console.WriteLine("\nPlease Enter amount in multiple of 500\n");
                }
            }
            if (bll.isValidBalance(bo) == true)                                                      //checking balance is more or equel to amount to be transfered
            {                                                                                        //or not
                int check = 0;
                do                                                                                   //loop which forces user to enter only integer value
                {
                  Console.WriteLine("Enter the account number to which you want to transfer");
                } while (!int.TryParse(Console.ReadLine(), out check));
                bo.accountNo = check;
                bo = bll.verifyAccount(bo);                                                          //verify is given account no exists or not and if yes then
                                                                                                     //provide info of that account
                if (bo.check == 1)
                {
                    Console.Write("You wish to deposit RS: "+ bo.cash+ " in account held by "+       //showing info on console
                        bo.holderName+ ". If this information is correct");
                    do                                                                               //loop which forces user to enter only integer value
                    {
                      Console.WriteLine(" please re-enter the account number: ");
                    } while (!int.TryParse(Console.ReadLine(), out dr));
                    if (dr == bo.accountNo)                                                          //cheking entered account number and re-entered account numbers matches
                    {
                        bo = bll.transfer(bo);                                                       //transfering amount
                        Console.WriteLine("Transaction Confirmed");
                    }else   {
                        Console.WriteLine("Your re-entered account no does no match \n \n Transaction Unsuccessfull");
                    }
                }else  {
                    Console.WriteLine("Given Account No is invalid ");
                }
            }else  {
                Console.WriteLine("Your account has insuficient balance");                           //in case if invalid account number then showing error message
                bo.cash = 0;                                                                         //here call of fast cash function purpose is to get info 
                bo = bll.transfer(bo);                                                               //from databse for reciept. here this function will not transfer
            }                                                                                        //any amount
            bo.transactionType = "Transfered:";
            printReciept(bo);
        }
        public void cashDeposit(ATMbo bo)                                                             //Function if user wants to deposit cash
        {
            ATMbll bll = new ATMbll();
            int check = 0;
            do                                                                                         //loop which forces user to enter only integer value
            {
              Console.WriteLine("Enter the cash amount to deposit: ");
            } while (!int.TryParse(Console.ReadLine(), out check));
            bo.cash = check;

            bo = bll.deposit(bo);                                                                      //Sending amount to BLL layer
            Console.WriteLine("\nCash deposited successfully.");
            bo.transactionType = "Deposited:";
            printReciept(bo);
        }
 
        public void displayBalance(ATMbo bo)                                                           //If user wants his/her balance .this function show them
        {
            ATMbll bll = new ATMbll();
            bo = bll.display(bo);
            Console.WriteLine("Account #" + bo.accountNo + "\nDate: " + DateTime.Now.ToShortDateString() + "\nBalance: " + bo.balance);
        }

        public void printReciept(ATMbo bo)                                                             //Function used to print reciepts of all kind of transactions
        {                                                                                              //on console
            Console.Write("Do you wish to print receipt(Y/N)");
            int  dr = 0; string answer;
            while (dr != 1)                                                                            //Loop will not terminate untill user enters y,Y,N,n 
            {
                answer = Console.ReadLine();
                if (answer == "Y" || answer == "y")
                {
                    Console.WriteLine("Account #" + bo.accountNo + "\nDate: " + DateTime.Now.ToShortDateString() +
                                      "\n"+bo.transactionType+": " + bo.cash + "\nBalance: " + bo.balance);
                    dr = 1;
                }else if (answer == "N" || answer == "n"){
                    Console.WriteLine("Thank you for using our ATM sevices");
                    dr = 1;
                }else{
                    Console.WriteLine("Please select valid option (Y/N)");
                }
            }
        }

        /////Admin Portion
        public void getInputAdmin()                                                     //Function gets userlogin id and pin and send for verification
        {
            ATMbll bll = new ATMbll();                                                  //Making an object of business logic layer
            Console.WriteLine("Please Enter User Id:");                                 //Getting input for login
            string userId = null;
            userId = Console.ReadLine();
            int pin = 0;
            do                                                                          //loop which forces user to enter only integer value
            {
                Console.WriteLine("Enter 5 digit pin code: ");
            } while (!int.TryParse(Console.ReadLine(), out pin));
            ATMbo bo = new ATMbo { userIdName = userId, userIdPin = pin };              //initializing methods of business object dll file
            int success = 5;
            if (userId != string.Empty)                                                 //Calling an login of Business logic layer
            {
               success = bll.adminLogin(bo);
            }

            if (success == 0)                                                           //Showing console messages
            {
                Console.WriteLine("Invalid UserName or Password:");
            }else if (success == 1){
                Console.WriteLine("Login Successful:");
                adminMenu(bo);                                                          //if login successfull then call admin menu
            }else{
                Console.WriteLine("You have entered invalid pin 3 time your account has been disabled\n Please contact Manager:");
            }
        }

        public void adminMenu(ATMbo bo)                                                  //A function used to show Admin menu and gets his/her choice
        {
            Console.WriteLine("1----Create New Account\n2----Delete Existing Account\n3----Update Account Information");
            Console.WriteLine("4----Search for Accounts\n5----View Reports");
            int check = 0;
            do                                                                            //loop which forces user to enter only integer value
            {
              Console.WriteLine("\nPlease select one ofthe above Options");
            } while (!int.TryParse(Console.ReadLine(), out check));
            if (check == 1)                                                               //Calling Functions according to user input
            {
                createNewAccount(bo);
            }else if (check == 2){
                deleteExistingAccount(bo);
            }else if (check == 3){
                updateAccountInformation(bo);
            }else if (check == 4){
                searchForAccount(bo);
            }else if (check == 5){
                viewReports(bo);
            }else{
                Console.WriteLine("You have Entered invalid option");
            }
        }

        public void createNewAccount(ATMbo bo)                                             //A function used to create new account of customer
        {
            ATMbll bll = new ATMbll();
            Console.WriteLine("\nPlease fill up all the given fields\n");                  //Getting all required inputs from Admin
            Console.Write("Login: ");
            bo.userIdName = null;
            bo.userIdName = Console.ReadLine();
            while (string.IsNullOrEmpty(bo.userIdName))                                    //In case user do not fill login then getting again input
            {
                Console.WriteLine("Login cannot be empty)");
                bo.userIdName = Console.ReadLine();
            }
            int check = 0;
            do                                                                            //loop which forces user to enter only integer value
            {
                 Console.Write("Pin Code: ");
            } while (!int.TryParse(Console.ReadLine(), out check));
            bo.userIdPin = check;
            Console.Write("Holders Name: ");
            bo.holderName = null;
            bo.holderName = Console.ReadLine();
            while (string.IsNullOrEmpty(bo.holderName))                                    //if value not given getting again input
            {
                Console.WriteLine("Holder name cannot be empty)");
                bo.holderName = Console.ReadLine();
            }
            Console.Write("Type(savings/current): ");
            bo.type = null;
            string b = "current", c = "savings";
            bo.type = Console.ReadLine();
            while (!string.Equals(bo.type, b) && !string.Equals(bo.type, c))                //It will only recieve 2 inputs current/savings otherwise loop goes on
            {
                Console.WriteLine("You have Entered invalid type.PLEASE Enter correct one(savings/current)");
                bo.type = Console.ReadLine();
            }
            do                                                                               //loop which forces user to enter only integer value
            {
                Console.Write("Starting Balance: ");
            } while (!int.TryParse(Console.ReadLine(), out check));
            bo.balance = check;
            Console.Write("Status(disable/active): ");
            bo.status = null;
            string b1 = "disable", c1 = "active";
            bo.status = Console.ReadLine();
            while (!string.Equals(bo.status, b1) && !string.Equals(bo.status, c1))           //will only recieve 2 inputs disable/active
            {
                Console.WriteLine("You have Entered invalid status.PLEASE Enter correct one(savings/current)");
                bo.status = Console.ReadLine();
            }
            bo=bll.createNewAccount(bo);
            Console.WriteLine("Account created Successfully - the account number assigned is: "+bo.accountNo);

        }
        public void deleteExistingAccount(ATMbo bo)                                           //Function allows admin to delete an customer account
        {
            ATMbll bll = new ATMbll();
            int dr = 0, accountno = 0;
            do                                                                                 //loop which forces user to enter only integer value
            {
                Console.Write("Enter Account no you want to delete: ");
            } while (!int.TryParse(Console.ReadLine(), out accountno));                        //Getting account number
            bo.accountNo = accountno;

            bo = bll.verifyAccount(bo);                                                        //verifying account number
            if (bo.check == 1)
            {
                Console.Write("You wish to delete the account held by "+ bo.holderName+ ". If this information is correct");
                do
                {
                    Console.Write(" please re-enter the account number: ");
                } while (!int.TryParse(Console.ReadLine(), out dr));                            //getting account number again to verify
                if (dr == bo.accountNo)                                                         //if re-enter account No is correct then delete account
                {
                    bll.deleteExistingAccount(bo);
                    Console.WriteLine("Account Deleted Successfully");
                }else {
                    Console.WriteLine("Your re-entered account no does no match \n \n Transaction Unsuccessfull");
                }
            }else{
                Console.WriteLine("Given Account No is invalid ");
            }
        }
        public void updateAccountInformation(ATMbo bo)                                           //Function allows admin to update any info of any account number
        {
            ATMbll bll = new ATMbll();
            int accountno = 0;
            do                                                                                   //loop which forces user to enter only integer value
            {
                Console.Write("Enter the Account Number: ");
            } while (!int.TryParse(Console.ReadLine(), out accountno));
            bo.accountNo = accountno;
            bo = bll.verifyAccount(bo);                                                          //verifying account number
            if(bo.check==1)
            {
                bo = bll.updateAccountGetData(bo);                                               //getting data of verifyed account and displaying
                Console.WriteLine("Account #"+bo.accountNo+ "\nType: " + bo.type+ "\nHolder Name: " + bo.holderName);
                Console.WriteLine("Balance: "+bo.balance+ "\nStatus: " + bo.status);
                Console.WriteLine("\nPlease Enter in the fields you wish to update (leave blank otherwise)");
                Console.Write("User Name(Login) ");
                bo.userIdName = Console.ReadLine();                                              //getting values if he/she wants ro give otherwise go on
                int pin = 0;
                Console.Write("Pin Code: ");
                int.TryParse(Console.ReadLine(), out pin);
                bo.userIdPin = pin;
                Console.Write("Holder Name: ");
                bo.holderName = Console.ReadLine();
                Console.Write("Status(disable/active): ");
                bo.status = Console.ReadLine();
                string b1 = "disable", c1 = "active";                                            //only recive 3 kind of inputs disable/active/empty value
                while (!string.Equals(bo.status, b1) && !string.Equals(bo.status, c1) && !string.Equals(bo.status, string.Empty))
                {
                    Console.WriteLine("You have Entered invalid status.PLEASE Enter correct one(disable/active)");
                    bo.status = Console.ReadLine();
                }

                bll.updateAccount(bo);                                                         //updating account number 
                Console.WriteLine("Your Account has successfully updated");
            }else{
                Console.WriteLine("Given Account No is invalid ");
            }
        }
        public void searchForAccount(ATMbo bo)                                                     //Function used for searching the accounts and showing data
        {
            ATMbll bll = new ATMbll();
            ATMbo[] bo1 = new ATMbo[20];
            for (int k = 0; k < 20; k++)
            {
                bo1[k] = new ATMbo();
            }
            Console.Write("SEARCH MENU:\nAccount ID: ");                                           //User can search with any kind of data from given options he/she want
            int account ;
            int.TryParse(Console.ReadLine(), out account);
            bo.accountNo = account;
                                                                                                  //Following are all input getters
            Console.Write("User Name: ");
            bo.userIdName = Console.ReadLine();
            Console.Write("Holder Name: ");
            bo.holderName = Console.ReadLine();
            Console.Write("Type: ");
            bo.type = null;
            bo.type = Console.ReadLine();
            string b = "current", c = "savings";
            while (!string.Equals(bo.type, b) && !string.Equals(bo.type, c)&&!string.Equals(bo.type,string.Empty))
            {
                Console.WriteLine("You have Entered invalid type.PLEASE Enter correct one(savings/current)");
                bo.type = Console.ReadLine();
            }
            int balance ;
            Console.Write("Balance: ");
            int.TryParse(Console.ReadLine(), out balance);
            bo.balance = balance;
            Console.Write("Status(disable/active): ");
            bo.status = Console.ReadLine();
            string b1 = "disable", c1 = "active";
            while (!string.Equals(bo.status, b1) && !string.Equals(bo.status, c1) && !string.Equals(bo.status, string.Empty))
            {
                Console.WriteLine("You have Entered invalid status.PLEASE Enter correct one(disable/active)");
                bo.status = Console.ReadLine();
            }
            bo1 = bll.searchForAccount(bo);                                                            //After inputs calling search method of BLL layer
              bo = bo1[19];
            int check = bo.check;
            Console.WriteLine("======= SEARCH RESULTS=======");                                        //Showing results on console
            Console.WriteLine("Account ID    User Name    Holders Name    Type      Balance    Status    ");
            for (int i = 0; i < check; i++)
            {
                bo = bo1[i];
                Console.WriteLine(bo.accountNo + "             " + bo.userIdName + "     " + bo.holderName + "        " + 
                    bo.type + "   " + bo.balance + "    " + bo.status);
            }
        }
        public void viewReports(ATMbo bo)                                                              //Function get customer choice for report search
        {
            Console.WriteLine("1----Accounts By Amount\n2----Accounts By Date");
            int check = 0;
            do                                                                                         //loop which forces user to enter only integer value
            {
                Console.WriteLine("\nPlease select one ofthe above Options");
            } while (!int.TryParse(Console.ReadLine(), out check));
            if (check == 1)                                                                            //Calling functions according to user choice
            {
                viewReportsByAmount(bo);  
            }else if (check == 2) {
                viewReportsByDate(bo);
            }else {
                Console.WriteLine("You have entered invalid option");
            }
        }
        public void viewReportsByAmount(ATMbo bo)                                                     //This function shows report if user selects by amount
        {
            ATMbll bll = new ATMbll();
            ATMbo[] bo1 = new ATMbo[20];                                                              //An array of objects used to store and display results
            for (int k = 0; k < 20; k++)
            {
                bo1[k] = new ATMbo();
            }
            int min = 0, max = 0;
            do                                                                                       //loop which forces user to enter only integer value
            {
                Console.Write("Enter the minimum amount: ");
            } while (!int.TryParse(Console.ReadLine(), out min));
            bo.min = min;
            do                                                                                       //loop which forces user to enter only integer value
            {
                Console.Write("Enter the maximum amount: ");
            } while (!int.TryParse(Console.ReadLine(), out max));
            bo.max = max;
            bo1 = bll.viewReportsByAmount(bo);                                                       //Getting data from BLL layer
            bo = bo1[19];
            int check = bo.check;
            Console.WriteLine("======= SEARCH RESULTS=======");                                      //Showing data on console
            Console.WriteLine("Account ID    User Name    Holders Name    Type      Balance    Status    ");
            for(int i=0;i<check;i++)
            {
                bo = bo1[i];
                Console.WriteLine(bo.accountNo + "             " + bo.userIdName + "     " + bo.holderName + "        " 
                    + bo.type + "   " + bo.balance + "    " + bo.status);
            }
        }
        public void viewReportsByDate(ATMbo bo)                                                         //function used to display reports if user enters reports by date
        {
            ATMbll bll = new ATMbll();
            ATMbo[] bo1 = new ATMbo[30];                                                                //Array used to store and display results
            for (int k = 0; k < 30; k++)
            {
                bo1[k] = new ATMbo();
            }
            DateTime start , end ;
            do                                                                                          //loop which forces user to enter only integer value
            {
                Console.Write("Enter the starting date(MM/DD/YYYY): ");
            } while (!DateTime.TryParse(Console.ReadLine(), out start));
            bo.startDate = start;
            do                                                                                          //loop which forces user to enter only integer value
            {
                Console.Write("Enter the End Date(MM/DD/YYYY): ");
            } while (!DateTime.TryParse(Console.ReadLine(), out end));
            bo.endDate = end;
            bo1 = bll.viewReportsByDate(bo);                                                            //getting data from BLL layer
            bo = bo1[29];
            int check = bo.check;
            Console.WriteLine("======= SEARCH RESULTS=======");                                         //Showing results in console
            Console.WriteLine("Transaction type    User Name    Holders Name      Balance    Date    ");
            for (int i = 0; i < check; i++)
            {
                bo = bo1[i];
                Console.WriteLine(bo.transactionType + "        " + bo.userIdName + "     " + bo.holderName + "      " 
                    + bo.balance + "    " + bo.outdate.ToShortDateString());
            }
        }
     }
}