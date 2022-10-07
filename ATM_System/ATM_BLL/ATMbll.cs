//BSEF19A033 MUHAMMAD HAMID IQBAL
//(Assalam O Alaikum Kindly Note that no console used in BLL layer neither access of databse Jaza ka Allah)

using ATM_BO;
using ATM_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_BLL
{
    public class ATMbll
    {
        public int customerLogin(ATMbo bo)
        {
            //Data access layer object

            ATMdal dal = new ATMdal();

            //encryption of user name

            string name = encryption(bo.userIdName);
            bo.userIdName = name;

            //encryption is user id

            string id = encryption(Convert.ToString(bo.userIdPin));
            bo.userIdPin = int.Parse(id);

            //data access layer Login Function

            bool success=dal.customerLogin(bo);

            //function call to check weather it is disabled or not(on mor then 3 attempts)

              bo = dal.disabledLoginCustomer(bo);

            //if it is both enabled account and login information correct then return 1

            if (bo.check<=3)
            {
                if (success == true)
                {
                    //return 1 means login successfuly
                     return 1;
                }
                else
                {
                    //in case if incorrect pin make an count of incorrect attempts

                    dal.updateCheck(bo);
                }
            }
            else if(bo.check==10)
            {
                return 0;
            }
            else
            {
                //it will tells account is blocked

                return 2;
            }
            //it will tells invalid user name or possword

            return 0;
        }

        //Function to encrypt/decrypt username and password

        public string encryption(string code)
        {
            int a = code.Length;
            //an array to store ascii values

            int[] ci = new int[20];

            //an array to store chracters against ascii values
            string[] ci1 = new string[20];

            //Converting string to ascii of each character
            for (int k = 0; k < a; k++)
            {
                ci[k] = (int)code[k];
            }

            //Logic to convert ascii to encrypted word ascii
            for (int k = 0; k < a; k++)
            {
                if ((ci[k] <= 90) && (ci[k] >= 65))
                {
                    int i = ci[k] - 65;
                    ci[k] = 90 - i;
                }
                else if ((ci[k] <= 122) && (ci[k] >= 97))
                {
                    int i = ci[k] - 97;
                    ci[k] = 122 - i;
                }
                else if ((ci[k] <= 57) && (ci[k] >= 48))
                {
                    int i = ci[k] - 48;
                    ci[k] = 57 - i;
                }
            }
            //Converting ascii to characters
            for (int k = 0; k < a; k++)
            {
                if ((ci[k] <= 90) && (ci[k] >= 65))
                {
                    ci1[k] = char.ToString((char)ci[k]);
                }
                else if ((ci[k] <= 122) && (ci[k] >= 97))
                {
                    ci1[k] = char.ToString((char)ci[k]);
                }
                else if ((ci[k] <= 57) && (ci[k] >= 48))
                {
                    ci1[k] = Convert.ToString((char)ci[k]);
                }
            }

            //making string of array string
            string result = String.Concat(ci1);
            return result;
        }

        public int selectOption(int check)                                         //A function perform action against input of user in fast cash
        {
            int rupee = 0;
            if(check==1)                                                           //Storing value in variable against choice
            {
                rupee = 500;
            }
            else if(check==2)
            {
                rupee = 1000;
            }
            else if(check==3)
            {
                rupee = 2000;
            }
            else if(check==4)
            {
                rupee = 5000;
            }
            else if(check==5)
            {
                rupee = 10000;
            }
            else if(check==6)
            {
                rupee = 15000;
            }
            else if (check==7)
            {
                rupee = 20000;
            }
            return rupee;
        }
        public bool isValidBalance(ATMbo bo)                                     //Function checks is balance valid or not(greater then or equal to user value)
        {
            ATMdal dal = new ATMdal();
            bo = dal.isValidBalance(bo);
            if (bo.balance > bo.cash)                                            //return true if balance is valid
            {
                return true;
            }
            return false;
        }
        public ATMbo fastCash(ATMbo bo)                                           //Fast cash function perform only call to withdrawal function of DAL layer
        {
            bo.transactionType = "Cash Withdraw";
            ATMdal dal = new ATMdal();
            bo=dal.withdraw(bo);
            return bo;
        }
        public ATMbo normalCash(ATMbo bo)                                         //Normal cash function perform only call to withdrawal function of DAL layer
        {
            bo.transactionType = "Cash Withdraw";
            ATMdal dal = new ATMdal();
            bo = dal.withdraw(bo);
            return bo;
        }
        public ATMbo isValidLimit(ATMbo bo)                                      //function used to check is daily limit matches or not 
        {
            ATMdal dal = new ATMdal();
            bo.transactionType = "Cash Withdraw";
            bo =dal.isValidLimit(bo);                                            //In case no data found agains user name and transaction type and today date
            if (bo.limit==0)                                                     //then setting value to full limit value
            {
                bo.limit = 20000;
            }
            return bo;
        }
        public void transcriptInsertion(ATMbo bo)                                //Function used to call transcript insertion function of data access layer 
        {                                                                        //and passing value
            ATMdal dal = new ATMdal();
            dal.transcriptInsertion(bo);
        }
        public void updateLimit(ATMbo bo)                                          //Function used to call updatelimit function of data access layer and passing value
        {
            ATMdal dal = new ATMdal();
            dal.updateLimit(bo);
        }
        public ATMbo verifyAccount(ATMbo bo)                                       //Function used to call verifyaccount function of data access layer and passing value
        {
            ATMdal dal = new ATMdal();
            bo=dal.verifyAccount(bo);
            return bo;
        }

        public ATMbo transfer(ATMbo bo)                                            //Function used to call transfer function of data access layer and passing value
        {
            bo.transactionType = "Cash Transfer";
            ATMdal dal = new ATMdal();
            dal.transfer_money(bo);                                                 //function adding value to other account(account where value to be tranfered)
            bo = dal.transfer(bo);                                                  //Function subtraction value from user account
            return bo;
        }
        public ATMbo deposit(ATMbo bo)                                              //Function used to call updatelimit function of data access layer and passing value
        {
            bo.transactionType = "Cash Deposit";
            ATMdal dal = new ATMdal();
            bo = dal.depositCash(bo);
            return bo;
        }
        public ATMbo display(ATMbo bo)                                               ////Function used to call display function of data access layer and passing value
        {
            ATMdal dal = new ATMdal();
            bo = dal.display(bo);
            return bo;
        }


        //Admin Portion
        public int adminLogin(ATMbo bo)                                             //Admin login 
        {
            //Data access layer object

            ATMdal dal = new ATMdal();

            //encryption of user name

            string name = encryption(bo.userIdName);
            bo.userIdName = name;

            //encryption is user id

            string id = encryption(Convert.ToString(bo.userIdPin));
            bo.userIdPin = int.Parse(id);
            //data access layer Login Function

            bool success = dal.adminLogin(bo);

            //function call to check weather it is disabled or not(on mor then 3 attempts)

            bo = dal.disabledLoginAdmin(bo);
            //if it is both enabled account and login information correct then return 1

            if (bo.check <= 3)
            {
                if (success == true)
                {
                    //return 1 means login successfuly
                    return 1;
                }
                else
                {
                    //in case if incorrect pin make an count of incorrect attempts
                    dal.updateCheckAdmin(bo);
                }
            }
            else if (bo.check == 10)
            {
                return 0;
            }
            else
            {
                //it will tells account is blocked
                return 2;
            }
            //it will tells invalid user name or password
            return 0;
        }

        public ATMbo createNewAccount(ATMbo bo)                                         //In creating new account encrypt the user name and pin
        {
            bo.userIdName = encryption(bo.userIdName);
            bo.userIdPin =int.Parse( encryption(Convert.ToString(bo.userIdPin)));
            string b1 = "disable";                                                     
            string c1 = "active";
            if(string.Equals(bo.status, c1))                                            //Assigning value to control according to give satus
            {
                bo.control = 0;                                                          //0 means user have 3 attemps(0 for active accaount)

            }else if(string.Equals(bo.status, b1))
            {
                bo.control = 4;                                                          //4 means account disabled no attemps allowed
            }
            ATMdal dal = new ATMdal();
            bo = dal.createNewAccount(bo);
            return bo;
        }

        public void deleteExistingAccount(ATMbo bo)                                    //Function used to call delete existing account function of data access layer and passing value
        {
            ATMdal dal = new ATMdal();
            dal.deleteExistingAccount(bo);
        }

        public ATMbo updateAccountGetData(ATMbo bo)                                    ////Function used to get value from data access layer and passing values
        {
            ATMdal dal = new ATMdal();
            bo = dal.updateAccountGetData(bo);
            bo.userIdName = encryption(bo.userIdName);                                  //decrypting user name
            bo.userIdPin= int.Parse(encryption(Convert.ToString(bo.userIdPin)));
            if (bo.control <= 3)                                                        //Updating status according to value of control active for less or equal to 3
            {
                bo.status = "active";
            }
            else if (bo.control > 3)                                                    //Updating status according to value of control disable for greater then 3   
            {
                bo.status = "disable";
            }
            return bo;
        }
        public void updateAccount(ATMbo bo)                                   //Function will check which values given and assign old data if it is not given
        {
            ATMdal dal = new ATMdal();

            string userIdName = bo.userIdName;
            string holderName = bo.holderName;
            string status = bo.status;
            int userIdPin = bo.userIdPin;

            bo = updateAccountGetData(bo);                                        //getting previous data

            if (string.IsNullOrEmpty(userIdName))                                  //checking user name given otherwise assign the old one
            {
                userIdName = bo.userIdName;
            }

            if (userIdPin == 0)                                                   //checking user id given otherwise assign the old one
            {
                userIdPin = bo.userIdPin;
            }

            if (string.IsNullOrEmpty(holderName))                                 //checking holder name given otherwise assign the old one
            {
                holderName =bo.holderName;
            }

            if (string.IsNullOrEmpty(status))                                     //checking status given otherwise assign the old one
            {
                status = bo.status;
            }
            bo.userIdName = userIdName;
            bo.holderName = holderName;
            bo.status = status;
            bo.userIdPin = userIdPin;

            bo.userIdName = encryption(bo.userIdName);                               //encrypting values
            bo.userIdPin = int.Parse(encryption(Convert.ToString(bo.userIdPin)));

            string b1 = "disable";
            string c1 = "active";
            if (string.Equals(bo.status, c1))                                         //Updating control according to value of status if active then control set to 0 
            {
                bo.control = 0;

            }
            else if (string.Equals(bo.status, b1))                                     //Updating control according to value of status if disable then control set to 4      
            {
                bo.control = 4;
            }


            dal.updateAccount(bo);
        }

        public ATMbo[] searchForAccount(ATMbo bo)                                     //Search for account is account exists or not
        {
            ATMdal dal = new ATMdal();
            ATMbo[] bo1 = new ATMbo[25];
            for (int k = 0; k < 20; k++)
            {
                bo1[k] = new ATMbo();
            }
            int d = 0;

            if (string.IsNullOrEmpty(bo.userIdName))                                 //These all if else for if user dose not give value then store space
            {                                                                        //which is used in DAL 
                bo.userIdName = " ";                                                 //Logic Knows either username have value or a space
            }                                                                        //if value then take it in search otherwise neglect the space
            if (string.IsNullOrEmpty(bo.holderName))
            {
                bo.holderName = " ";
            }
            if (string.IsNullOrEmpty(bo.type))
            {
                bo.type = " ";
            }
            if (!string.IsNullOrEmpty(bo.userIdName))
            {
                bo.userIdName = encryption(bo.userIdName);
            }
            if (string.IsNullOrEmpty(bo.status))
            {                                                                          //50 did exactly same work as space does
                bo.control =d= 50;
            }
            if (!string.IsNullOrEmpty(bo.status))
            {
                string b1 = "disable";
                string c1 = "active";
                if (string.Equals(bo.status, c1))
                {
                    bo.control =d= 0;
                }
                else if (string.Equals(bo.status, b1))
                {
                    bo.control =d= 4;
                }
            }

            bo1 = dal.searchForAccount(bo);                                            //Calling search for account of dal layer 

            int i = 0;
            bo = bo1[19];
            int j,l = j=bo.check;
            while (j != 0)
            {
                bo = bo1[i];
                bo.userIdName = encryption(bo.userIdName);                              //after getting results one by decrypt them all and set value of status
                if (bo.control <= 3)
                {
                    bo.status = "active";
                }
                else if (bo.control > 3)
                {
                    bo.status = "disable";
                }
                bo1[i] = bo;
                i++;
                j--;
            }
            int m = 0;
            for (int k = 0; k < l; k++)
            {                                                                           //This logic filters data according to status demanded
                
                if (d == 50)
                {
                    bo1[m].accountNo = bo1[k].accountNo;
                    bo1[m].userIdName = bo1[k].userIdName;
                    bo1[m].status = bo1[k].status;
                    bo1[m].holderName = bo1[k].holderName;
                    bo1[m].balance = bo1[k].balance;
                    bo1[m].type = bo1[k].type;
                    m++;
                }
                else if (d == 0)
                {
                    bo.control = bo1[k].control;
                    if (bo.control <= 3)
                    {
                        bo1[m].accountNo = bo1[k].accountNo;
                        bo1[m].userIdName = bo1[k].userIdName;
                        bo1[m].status = bo1[k].status;
                        bo1[m].holderName = bo1[k].holderName;
                        bo1[m].balance = bo1[k].balance;
                        bo1[m].type = bo1[k].type;
                        m++;
                    }
                }
                else if (d == 4)
                {
                    bo.control = bo1[k].control;
                    if (bo.control > 3)
                    {
                        bo1[m].accountNo = bo1[k].accountNo;
                        bo1[m].userIdName = bo1[k].userIdName;
                        bo1[m].status = bo1[k].status;
                        bo1[m].holderName = bo1[k].holderName;
                        bo1[m].balance = bo1[k].balance;
                        bo1[m].type = bo1[k].type;
                        m++;
                    }
                }
            }
            bo.check = m;
            bo1[19]=bo;
            return bo1;
        }
        public ATMbo[] viewReportsByAmount(ATMbo bo)                                     //Getting accounts searched by amount
        {
            ATMdal dal = new ATMdal();
            ATMbo[] bo1 = new ATMbo[20];                                                //An array of objects used to store values comes from dal 
            for (int k = 0; k < 20; k++)
            {
                bo1[k] = new ATMbo();
            }
            bo1 =dal.viewReportsByAmount(bo);                                           //calling function of DAL layer
            int i = 0;
            bo = bo1[19];
            int j = bo.check;
            while (j != 0)
            {
                bo = bo1[i];
                bo.userIdName= encryption(bo.userIdName);                                //after getting results one by decrypt them all and set value of status
                if (bo.control <= 3)
                {
                    bo.status = "active";
                }
                else if (bo.control > 3)
                {
                    bo.status = "disable";
                }
                bo1[i] = bo;
                i++;
                j--;
            }
            return bo1;
        }
        public ATMbo[] viewReportsByDate(ATMbo bo)                                        //Getting accounts searched by date
        {
            ATMdal dal = new ATMdal();
            ATMbo[] bo1 = new ATMbo[30];                                                 //An array of objects used to store values comes from dal 
            for (int k = 0; k < 30; k++)
            {
                bo1[k] = new ATMbo();
            }
            DateTime startdate = bo.startDate;
            DateTime enddate = bo.endDate;
            bo1 = dal.viewReportsByDate(bo);                                            //function get all the records of transcript following logic filter them
            int i = 0;
            bo = bo1[29];
            int j,l =j= bo.check;
            while (j != 0)
            {
                bo = bo1[i];
                bo.userIdName = encryption(bo.userIdName);                              //decrypting user name
                bo1[i] = bo;
                i++;
                j--;
            }
            int m = 0;
            DateTime date;
            for(int k=0;k<l;k++)                                                         
            {                                                                           //This logic filters data according to given logic

                date= DateTime.Parse(bo1[k].date);
                if (date<=enddate&&date>=startdate)
                {
                    bo1[m].accountNo = bo1[k].accountNo;
                    bo1[m].transactionType = bo1[k].transactionType;
                    bo1[m].holderName = bo1[k].holderName;
                    bo1[m].userIdName = bo1[k].userIdName;
                    bo1[m].balance = bo1[k].balance;
                    bo1[m].outdate = DateTime.Parse( bo1[k].date);
                    m++;
                }
            }

            bo.check = m;
            bo1[29] = bo;
            return bo1;
        }
    }
}
