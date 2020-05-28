using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models
{
    //Class to hold all data from random data in CSV
    class MasterCustomer
    {
        public long Id { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int Accounts { get; set; }
        public long Account1 { get; set; }
        public long Account2 { get; set; }
        public long Account3 { get; set; }

        public static List<MasterCustomer> GenerateCustomers(int amount, string optionalDomain = null, int optionalIdstart = 0)
        {
            List<MasterCustomer> Customers = new List<MasterCustomer>();
            string optdom = "-gen";
            if (optionalDomain != null) optdom = optionalDomain;
            Random ageRandomizer = new Random(1947385);
            Random nameRandomizer = new Random(46567665);
            Random accountRandomizer = new Random(38574);
            Random moneyRandomizer = new Random(395873);
            Random emailDomainRandomizer = new Random(4563);
            string[] domains = new[] { $"{optdom}@gmail.com", $"{optdom}@hotmail.com", $"{optdom}@hotmail.dk", $"{optdom}@mail.com" };
            string[] customersnamegenders = System.IO.File.ReadAllText(@"Customers_utf8.csv").Split("\n");


            for (int i = 1; i <= amount; i++)
            {
                string[] nameGender = customersnamegenders[nameRandomizer.Next(1, customersnamegenders.Length - 1)].Split(",");
                int accounts = accountRandomizer.Next(1, 3);
                Customers.Add(new MasterCustomer
                {
                    Id = i + optionalIdstart,
                    Name = nameGender[0],
                    Email = nameGender[0] + $"-{i}-" + domains[emailDomainRandomizer.Next(0, 3)],
                    Gender = nameGender[1],
                    Accounts = accounts,
                    Account1 = moneyRandomizer.Next(10000, 100000000),
                    Account2 = (accounts > 1 ? moneyRandomizer.Next(10000, 100000000) : 0),
                    Account3 = (accounts > 2 ? moneyRandomizer.Next(10000, 100000000) : 0),
                    Age = ageRandomizer.Next(18, 80)
                });
            }
            return Customers;
        }
    }
}
