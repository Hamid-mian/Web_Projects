//BSEF19A033 MUHAMMAD HAMID IQBAL
//(Assalam O Alaikum Note:In this layer no use of console and no logic used only
//getting,updating,inserting,deleting of data involved Jaza Ka Allah)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using ATM_BO;
namespace ATM_DAL
{
    public class ATMdal
    {
        //login for customer
        public bool customerLogin(ATMbo bo)
        {
            //Accessing Database
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            //Select query 
            string query = $"Select * from Customer where userName=@u and pin = @p and control<=3" ;
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            SqlParameter p2 = new SqlParameter("@p", bo.userIdPin);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            SqlDataReader dr = cmd.ExecuteReader();
            //Return true if info correct
            while (dr.Read())
            {
                return true;
            }
            con.Close();
            return false;
        }
        //An Function which update database if invalid pin entered
        public void updateCheck(ATMbo bo)
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            //Update Query
            string query = $"update Customer set control+=@c where userName=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            SqlParameter p2 = new SqlParameter("@c", "1");
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Function to chech either the function is disabled or not
        public ATMbo disabledLoginCustomer(ATMbo bo)
        {
            //variable to store value of control
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            //Selecting control from table. Control have count of invalid entered password
            string query = $"select control from Customer where userName=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            cmd.Parameters.Add(p1);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
               bo.check = int.Parse($"{dr[0]}");
            }
            if (!dr.HasRows)
            {
                bo.check = 10;
            }
            con.Close();
            return bo;
        }
        public ATMbo isValidBalance(ATMbo bo)                              //A  function which gets the balance of given username
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            string query = $"select balance from Customer where userName=@u";      //Select query
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            cmd.Parameters.Add(p1);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
               bo.balance = int.Parse($"{dr[0]}");
            }
            con.Close();
            return bo;
        }
        public ATMbo withdraw(ATMbo bo)                                                      //A function which decrement balance in case of withdrawal  
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            string query = $"update Customer set balance-=@b where userName=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            SqlParameter p2 = new SqlParameter("@b", bo.cash);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.ExecuteNonQuery();
            con.Close();

            con.Open();                                                                        //get all values of customer table
            string query1 = $"select * from Customer where userName=@y";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p3 = new SqlParameter("@y", bo.userIdName);
            cmd1.Parameters.Add(p3);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())                                                                  //storing values which required
            {
                bo.accountNo= int.Parse($"{dr[0]}");
                bo.holderName = $"{dr[4]}";
                bo.balance = int.Parse($"{dr[5]}");
            }
            con.Close();
            return bo;
        }

        public ATMbo isValidLimit(ATMbo bo)                                                    //A function used to check daily limit of the user
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
          
            //getting holder name from the customer table
            con.Open();
            string query1 = $"select * from Customer where userName=@y";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p4 = new SqlParameter("@y", bo.userIdName);
            cmd1.Parameters.Add(p4);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                bo.holderName = $"{dr1[4]}";
            }
            con.Close();

            //Getting those values from transcript which matches all  the three requirments
            con.Open();
            string query = $"select * from transcript where transactionType = @t AND holderName = @h AND date=@d";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@t", bo.transactionType);
            SqlParameter p2 = new SqlParameter("@h", bo.holderName);
            SqlParameter p3 = new SqlParameter("@d", DateTime.Now.ToShortDateString());
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                bo.limit = int.Parse($"{dr[7]}");
            }
            if(!dr.HasRows)
            {
                bo.limit = 0;
            }
            con.Close();
            return bo;
        } 
        //A function used update limit after the transcation
        public void updateLimit(ATMbo bo)                                             
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            bo.limit = 20000 - bo.limit;
            string query = $"update transcript set limit-=@b where transactionType = @t AND holderName = @h AND date=@d ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@b", bo.limit);
            SqlParameter p2 = new SqlParameter("@t", bo.transactionType);
            SqlParameter p3 = new SqlParameter("@h", bo.holderName);
            SqlParameter p4 = new SqlParameter("@d", DateTime.Now.ToShortDateString());
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Searching in database is the given account no exists or not?
        public ATMbo verifyAccount(ATMbo bo)
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);

            con.Open();
            string query1 = $"select * from Customer where accountNo=@y";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p3 = new SqlParameter("@y", bo.accountNo);
            cmd1.Parameters.Add(p3);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                bo.holderName = $"{dr[4]}";
                bo.check = 1;                                                       //setting value in check if the info readed
            }
            if(!dr.HasRows)
            {
                bo.check = 0;                                                       //setting value if the table don't have any row
            }
            con.Close();
            return bo;
        }
        public ATMbo transfer(ATMbo bo)                                            //Function first decrement the balance then get values from database whoc are required for reciept purpose
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            string query = $"update Customer set balance-=@b where userName=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            SqlParameter p2 = new SqlParameter("@b", bo.cash);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.ExecuteNonQuery();
            con.Close();

            con.Open();
            string query1 = $"select * from Customer where userName=@y";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p3 = new SqlParameter("@y", bo.userIdName);
            cmd1.Parameters.Add(p3);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                bo.accountNo = int.Parse($"{dr[0]}");
                bo.holderName = $"{dr[4]}";
                bo.balance = int.Parse($"{dr[5]}");
            }
            con.Close();
            transcriptInsertion(bo);                                                             //generating an trascript against the transaction
            return bo;
        }
        public void transfer_money(ATMbo bo)                                                      //this function adds the money to given account no
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            string query = $"update Customer set balance+=@b where accountNo=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.accountNo);
            SqlParameter p2 = new SqlParameter("@b", bo.cash);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public ATMbo depositCash(ATMbo bo)                                                      //Function first update the database and then get values from table for reciept and transcript
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            string query = $"update Customer set balance+=@b where userName=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            SqlParameter p2 = new SqlParameter("@b", bo.cash);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.ExecuteNonQuery();
            con.Close();

            con.Open();
            string query1 = $"select * from Customer where userName=@y";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p3 = new SqlParameter("@y", bo.userIdName);
            cmd1.Parameters.Add(p3);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                bo.accountNo = int.Parse($"{dr[0]}");
                bo.holderName = $"{dr[4]}";
                bo.balance = int.Parse($"{dr[5]}");
            }
            con.Close();
            transcriptInsertion(bo);                                                              //Generating transcript against the transaction
            return bo;
        }
        public ATMbo display(ATMbo bo)
        {
            string constring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);

            con.Open();
            string query1 = $"select * from Customer where userName=@y";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p3 = new SqlParameter("@y", bo.userIdName);
            cmd1.Parameters.Add(p3);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                bo.accountNo = int.Parse($"{dr[0]}");
                bo.holderName = $"{dr[4]}";
                bo.balance = int.Parse($"{dr[5]}");
            }
            con.Close();
            return bo;
        }
        public void transcriptInsertion(ATMbo bo)                                                 //Inserting values to the transcript table
        {
            string constring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string date = DateTime.Now.ToShortDateString();
            string query = $"insert into transcript (transactionType,accountNo,holderName,balance,date,userName) values(@t,@a,@h,@b,@d,@u)";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@t", bo.transactionType);
            SqlParameter p2 = new SqlParameter("@a", bo.accountNo);
            SqlParameter p3 = new SqlParameter("@h", bo.holderName);
            SqlParameter p4 = new SqlParameter("@b", bo.balance);
            SqlParameter p5 = new SqlParameter("@d", date);
            SqlParameter p6 = new SqlParameter("@u", bo.userIdName);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            cmd.Parameters.Add(p6);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //Admin Portion
        public bool adminLogin(ATMbo bo)                                                          //varifying username and pin also no of invalid pin attempts 
        {
            //Accessing Database
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            //Select query 
            string query = $"Select * from Admin where userName=@u and pin = @p and control<=3";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            SqlParameter p2 = new SqlParameter("@p", bo.userIdPin);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            SqlDataReader dr = cmd.ExecuteReader();

            //Return true if info correct
            while (dr.Read())
            {
                return true;
            }
            con.Close();
            return false;
        }
        
        public void updateCheckAdmin(ATMbo bo)                                               //in case wrong pin update the database and add one to check of invalid attempts
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            //Update Query
            string query = $"update Admin set control+=@c where userName=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            SqlParameter p2 = new SqlParameter("@c", "1");
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public ATMbo  disabledLoginAdmin(ATMbo bo)                                              //this function tells that username is blocked or not
        {
            
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            //Selecting control from table. Control have count of invalid entered password
            string query = $"select control from Admin where userName=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            cmd.Parameters.Add(p1);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                bo.check = int.Parse($"{dr[0]}");
            }
            if(!dr.HasRows)
            {
                bo.check = 10;
            }
            con.Close();
            //else mean account is blocked
            return bo;
        }

        public ATMbo createNewAccount(ATMbo bo)                                                 //Function simply insert a new row in the customer table and returning account no which is granted
        {
            string constring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string query = $"insert into Customer (userName,pin,control,holderName,balance,type) values('" + bo.userIdName + "','" + bo.userIdPin + "','" + bo.control + "','" + bo.holderName + "','" + bo.balance + "','" + bo.type + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();

            con.Open();
            string query1 = $"select accountNo from Customer where userName=@u";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p2 = new SqlParameter("@u", bo.userIdName);
            cmd1.Parameters.Add(p2);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                bo.accountNo = int.Parse($"{dr[0]}");
            }
            con.Close();
            return bo;
        }

        public void deleteExistingAccount(ATMbo bo)                                               //funtion simply get the account no and delete that row from table
        {
            string constring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            string query = $"delete from Customer  where accountNo=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p2 = new SqlParameter("@u", bo.accountNo);
            cmd.Parameters.Add(p2);
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public ATMbo updateAccountGetData(ATMbo bo)                                               //This function gets data to show it on screen before updation
        {
            string constring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);

            con.Open();
            string query1 = $"select * from Customer where accountNo=@a";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p3 = new SqlParameter("@a", bo.accountNo);
            cmd1.Parameters.Add(p3);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                bo.accountNo = int.Parse($"{dr[0]}");
                bo.userIdName= $"{dr[1]}";
                bo.userIdPin= int.Parse($"{dr[2]}");
                bo.control = int.Parse($"{dr[3]}");
                bo.holderName = $"{dr[4]}";
                bo.balance = int.Parse($"{dr[5]}");
                bo.type = $"{dr[6]}";
            }
            con.Close();
            return bo;
        }

        public void updateAccount(ATMbo bo)                                                      //after getting data from user this function updates the table
        {
            string constring = @" Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            //Update Query
            string query = $"update Customer set control=@c,holderName=@h,userName=@u,pin=@p where accountNo=@a";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter p1 = new SqlParameter("@u", bo.userIdName);
            SqlParameter p2 = new SqlParameter("@c", bo.control);
            SqlParameter p3 = new SqlParameter("@h", bo.holderName);
            SqlParameter p4 = new SqlParameter("@a", bo.accountNo);
            SqlParameter p5 = new SqlParameter("@p", bo.userIdPin);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public ATMbo[] searchForAccount(ATMbo bo)                                                //searching data from the database on the bases of user requirment
        {
            ATMbo[] bo1 = new ATMbo[20];
            for (int k = 0; k < 20; k++)
            {
                bo1[k] = new ATMbo();
            }
            int d = bo.control, i = 0;
            string constring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);
                                                                                                 //Query and logic designed such a way that if user give value then anding
            con.Open();                                                                          //otherwise ignore and search according to others(which are given)
            string query1 = $"select * from Customer where (@h = ' ' OR holderName=@h) AND ( @u = ' ' OR userName=@u) AND (@a = '0' OR accountNo=@a) AND (@b = '0' OR balance=@b ) AND (@t = ' ' OR type = @t)";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p1 = new SqlParameter("@h", bo.holderName);
            SqlParameter p2 = new SqlParameter("@u", bo.userIdName);
            SqlParameter p3 = new SqlParameter("@a", bo.accountNo);
            SqlParameter p4 = new SqlParameter("@b", bo.balance);
            SqlParameter p5 = new SqlParameter("@t", bo.type);
            cmd1.Parameters.Add(p1);
            cmd1.Parameters.Add(p2);
            cmd1.Parameters.Add(p3);
            cmd1.Parameters.Add(p4);
            cmd1.Parameters.Add(p5);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                bo1[i].accountNo = int.Parse($"{dr[0]}");
                bo1[i].userIdName = $"{dr[1]}";
                bo1[i].control = int.Parse($"{dr[3]}");
                bo1[i].holderName = $"{dr[4]}";
                bo1[i].balance = int.Parse($"{dr[5]}");
                bo1[i].type = $"{dr[6]}";
                i++;
            }
            bo.check = i;
            bo1[19] = bo;
            return bo1;
        }
        public ATMbo[] viewReportsByAmount(ATMbo bo)                                              //function used to get all data according to range of amount
        {
            ATMbo[] bo1 = new ATMbo[20];
            for(int k=0;k<20;k++)
            {
                bo1[k] = new ATMbo();
            }
            int i = 0;
            string constring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);

            con.Open();
            string query1 = $"select * from Customer where balance<=@m and balance>=@n";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlParameter p3 = new SqlParameter("@m", bo.max);
            SqlParameter p4 = new SqlParameter("@n", bo.min);
            cmd1.Parameters.Add(p3);
            cmd1.Parameters.Add(p4);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                bo1[i].accountNo = int.Parse($"{dr[0]}");
                bo1[i].userIdName = $"{dr[1]}";
                bo1[i].control = int.Parse($"{dr[3]}");
                bo1[i].holderName = $"{dr[4]}";
                bo1[i].balance = int.Parse($"{dr[5]}");
                bo1[i].type = $"{dr[6]}";
                i++;
            }
            bo.check = i;
            bo1[19] = bo;
            con.Close();
            return bo1;                                                                            
        }

        public ATMbo[] viewReportsByDate(ATMbo bo)                                                //Function get all data from the transcript table
        {
            ATMbo[] bo1 = new ATMbo[30];
            for (int k = 0; k < 30; k++)
            {
                bo1[k] = new ATMbo();
            }
            int i = 0;
            string constring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ATM;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(constring);

            con.Open();
            string query1 = $"select * from transcript";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                bo1[i].accountNo = int.Parse($"{dr[2]}");
                bo1[i].transactionType = $"{dr[1]}";
                bo1[i].holderName = $"{dr[3]}";
                bo1[i].userIdName = $"{dr[6]}";
                bo1[i].balance = int.Parse($"{dr[4]}");
                bo1[i].date= $"{dr[5]}";
                i++;
            }
            bo.check = i;
            bo1[29] = bo;
            con.Close();
            return bo1;
        }
    }
}
