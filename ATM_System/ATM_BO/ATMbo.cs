using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_BO
{
    public class ATMbo
    {
        public string userIdName { get; set; }
        public string transactionType { get; set; }
        public string holderName { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public int userIdPin { get; set; }
        public int cash { get; set; }
        public int balance { get; set; }
        public int check { get; set; }
        public int accountNo { get; set; }
        public int control { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string  date { get; set; }
        public DateTime outdate { get; set; }
        public int limit { get;set; }

    }
}