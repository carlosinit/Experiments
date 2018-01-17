using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlow.Console
{
    internal class Program
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);

        private static void Main()
        {
            var executionOptions = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 100,
                SingleProducerConstrained = true
            };

            var bufferBlock = new BufferBlock<BankAccount>();
            var alertBlock = new ActionBlock<BankAccount>(b => Alert(b), executionOptions);
            var feesBlock = new TransformBlock<BankAccount, BankAccount>(b => ApplyFees(b), executionOptions);
            var sendStatementsBlock = new TransformBlock<BankAccount, BankAccount>(b => SendStatements(b), executionOptions);
            var alertAfterStatementsBlock = new ActionBlock<BankAccount>(b => AlertAfterStatements(b), executionOptions);

            bufferBlock.LinkTo(alertBlock, b => b.Balance < 0);
            bufferBlock.LinkTo(feesBlock, b => b.Balance >= 0);
            feesBlock.LinkTo(sendStatementsBlock);
            sendStatementsBlock.LinkTo(alertAfterStatementsBlock, b => b.Balance < 0);
            sendStatementsBlock.LinkTo(DataflowBlock.NullTarget<BankAccount>(), b => b.Balance >= 0);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            //foreach (var bankAccount in GetBankAccounts())
            //{
            //    bufferBlock.Post(bankAccount);
            //}

            Parallel.ForEach(GetBankAccounts(), b => bufferBlock.Post(b));
            stopWatch.Stop();

            System.Console.WriteLine("Waiting for closure");
            System.Console.ReadLine();
            System.Console.WriteLine("Working for {0:N}ms", stopWatch.ElapsedMilliseconds);
            System.Console.ReadLine();
        }

        private static IEnumerable<BankAccount> GetBankAccounts()
        {
            foreach (var filePath in Directory.GetFiles("Data", "*.csv"))
            {
                var index = new Dictionary<string, int>();
                var headersLineProcessed = false;
                using (var streamReader = new StreamReader(filePath))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        if (!headersLineProcessed)
                        {
                            var headers = line.Split(',');
                            index = headers.ToDictionary(
                                chunk => chunk.Trim('"'),
                                chunk => Array.IndexOf(headers, chunk));
                            headersLineProcessed = true;
                        }
                        else
                        {
                            var data = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                            yield return new BankAccount(
                                new Person(
                                    data[index["first_name"]].Trim('"'),
                                    data[index["last_name"]].Trim('"'),
                                    data[index["email"]].Trim('"')),
                                Guid.NewGuid().ToString(),
                                Convert.ToDecimal(random.Next(-5000, 5000)));
                        }
                    }
                }
            }
        }

        private async static Task<BankAccount> SendStatements(BankAccount account)
        {
            await System.Console.Out.WriteLineAsync($"Balance of account {account.Id} is {account.Balance}");
            return account;
        }

        private static BankAccount ApplyFees(BankAccount account)
        {
            return account.Transact(-123.45m);
        }

        private static Task Alert(BankAccount account)
        {
            return System.Console.Out.WriteLineAsync($"No funds ({account.Balance:F}) on {account.Id} for bank account of {account.Owner.FullName}");
        }

        private static void AlertAfterStatements(BankAccount account)
        {
            System.Console.WriteLine(
                "No more funds ({2:F}) on {0} for bank account of {1}",
                account.Id,
                account.Owner.FullName,
                account.Balance);
        }
    }

    internal class Person
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public string Email { get; private set; }

        public Person(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }

    internal class BankAccount
    {
        public string Id { get; private set; }
        public Person Owner { get; private set; }
        public decimal Balance { get; private set; }

        public BankAccount(Person owner, string id, decimal balance)
        {
            Id = id;
            Owner = owner;
            Balance = balance;
        }

        public BankAccount Transact(decimal value)
        {
            Balance += value;
            return this;
        }
    }
}