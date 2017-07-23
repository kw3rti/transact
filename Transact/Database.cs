using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Mono.Data.Sqlite;

namespace Transact
{
    public class Database : Activity, Java.IO.ISerializable
    {
        //directory to store database (data/data/com.paulhollar.transact/files)
        private static string docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //database name
        private static string databaseName = "transact.db";

        //table names
        private string accountTableName = "tblAccounts";
        private string transactionTableName = "tblTransactions";
        
        //full path to database (directory + name)
        private static string pathToDatabase = Path.Combine(docsFolder, databaseName);
        //sqlite connection string
        private string connectionString = string.Format("Data Source={0};Version=3;", pathToDatabase);

        //class constructor (is called when class is created)
        public Database()
        {
            initializeDatabase();
        }

        //initialize database (create database and table)
        private void initializeDatabase()
        {
            createDatabase();
            createAccountTable();
            createTransactionTable();
        }

        //create database (will only create if the database doesn't already exist)
        private void createDatabase(){
            Console.WriteLine("Start: CreateDatabase");
			try
			{
                //check to see if the database already exists (if it doesn't, it is allowed to create)
				if (!File.Exists(pathToDatabase))
				{
					SqliteConnection.CreateFile(pathToDatabase);
                    Console.WriteLine("The database was successfuly created @" + pathToDatabase);
				}
				else
				{
                    Console.WriteLine("The database failed to create - reason: The database already exists");
				}
			}
			catch (IOException ex)
			{
                Console.WriteLine("The database failed to create - reason: " + ex.Message);
			}
            Console.WriteLine("End: CreateDatabase");
        }

        //create account table (will only create the table if it doesn't already exist)
        private async void createAccountTable()
        {
            Console.WriteLine("Start: CreateAccountTable");

            //if table doesn't already exists - create table
            if (!tableExists(accountTableName))
            {
                try
                {
                    var conn = new SqliteConnection(connectionString);
                    {
                        await conn.OpenAsync();
                        using (var command = conn.CreateCommand())
                        {
                            command.CommandText = "CREATE TABLE " + accountTableName + " (PK INTEGER PRIMARY KEY AUTOINCREMENT, Name ntext, Type ntext, Note ntext)";
                            command.CommandType = CommandType.Text;
                            await command.ExecuteNonQueryAsync();

                            Console.WriteLine("The table: " + accountTableName + " was successfully created");
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to create table in the database - reason: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Failed to create table in the database - reason: The table already exists");
            }
            Console.WriteLine("End: CreateAccountTable");
        }

        //create transaction table (will only create the table if it doesn't already exist)
        private async void createTransactionTable()
		{
            Console.WriteLine("Start: CreateTransactionTable");

            //if table doesn't already exists - create table
            if (!tableExists(transactionTableName))
            {
                try
                {
                    var conn = new SqliteConnection(connectionString);
                    {
                        await conn.OpenAsync();
                        using (var command = conn.CreateCommand())
                        {
                            command.CommandText = "CREATE TABLE " + transactionTableName + " (PK INTEGER PRIMARY KEY AUTOINCREMENT, AccountPK integer, Date date, Title ntext, Amount decimal(10,2), Category ntext, Type_ToAccount ntext, Notes ntext)";
                            command.CommandType = CommandType.Text;
                            await command.ExecuteNonQueryAsync();

                            Console.WriteLine("The table: " + transactionTableName + " was successfully created");
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to create table in the database - reason: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Failed to create table in the database - reason: The table already exists");
            }
            Console.WriteLine("End: CreateTransactionTable");
		}

        //check if a table already exists
        private bool tableExists(String tableName)
        {
            try
            {
                var conn = new SqliteConnection(connectionString);
                conn.Open();
                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = @name";
                cmd.Parameters.Add("@name", DbType.String).Value = tableName;

                if(cmd.ExecuteScalar() != null)
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to check if table exists - reason: " + ex.Message);
            }
            return false;
        }

        public async Task<bool> addAccount(string name, string note, string type, decimal amount, DateTime date, string category, string type_toaccount, string notes)
        {
            initializeDatabase();
            Console.WriteLine("Start: AddAccount");

            // create a connection string for the database
            var connectionString = string.Format("Data Source={0};Version=3;", pathToDatabase);
            try
            {
                using (var conn = new SqliteConnection((connectionString)))
                {
                    await conn.OpenAsync();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO " + accountTableName + " (Name, Type, Note) VALUES (@name, @type, @note); SELECT last_insert_rowid();";
                        command.Parameters.Add("@name", DbType.String).Value = name;
                        command.Parameters.Add("@type", DbType.String).Value = type;
                        command.Parameters.Add("@note", DbType.String).Value = note;
                        command.CommandType = CommandType.Text;
                        var accountPK = command.ExecuteScalar();

                        MainActivity.accounts.Add(new Account() { PK = Convert.ToInt32(accountPK), Name = name, Type = type, Note = note, Balance = amount });

                        await addTransaction(Convert.ToInt32(accountPK), date, "Initial Balance", amount, category,type_toaccount, notes);

                        Console.WriteLine("The record was inserted successfully");
                    }
                    conn.Close();
                }
                Console.WriteLine("End: AddAccount");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to insert record - reason: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> addTransaction(int accountPK, DateTime date, string title, decimal amount, string category, string type_toaccount, string notes){
            initializeDatabase();
			Console.WriteLine("Start: AddTransaction");

            // create a connection string for the database
			var connectionString = string.Format("Data Source={0};Version=3;", pathToDatabase);
			try
			{
				using (var conn = new SqliteConnection((connectionString)))
				{
					await conn.OpenAsync();
					using (var command = conn.CreateCommand())
					{
                        command.CommandText = "INSERT INTO " + transactionTableName + " (AccountPK, Date, Title, Amount, Category, Type_ToAccount, Notes) VALUES (@accountPK, @date, @title, @amount, @category, @type_toaccount, @notes); SELECT last_insert_rowid();";
                        command.Parameters.Add("@accountPK", DbType.Int32).Value = accountPK;
                        command.Parameters.Add("@date", DbType.Date).Value = date;
                        command.Parameters.Add("@title", DbType.String).Value = title;
                        command.Parameters.Add("@amount", DbType.Decimal).Value = amount;                        
                        command.Parameters.Add("@category", DbType.String).Value = category;
                        command.Parameters.Add("@type_toaccount", DbType.String).Value = type_toaccount;
                        command.Parameters.Add("@notes", DbType.String).Value = notes;
                        command.CommandType = CommandType.Text;
						var pk = command.ExecuteScalar();

                        Transactions.transactions.Add(new Transaction() { PK = Convert.ToInt32(pk), AccountPK = accountPK, Date = date, Title = title, Amount = amount, Category = category, Type_ToAccount = type_toaccount, Notes = notes });

                        //notify the list of transactions that the data set has changed (to update the list of transactions)
                        Transactions.transactionAdapter.NotifyDataSetChanged();

                        //get new account balance after transaction add
                        var newAccountBalance = getAccountBalance(accountPK);

                        //go through each account and update the account with the new balance
                        foreach (Account act in MainActivity.accounts)
                        {
                            if(act.PK == accountPK)
                            {
                                act.Balance = newAccountBalance;
                                MainActivity.accountAdapter.NotifyDataSetChanged();
                                break;
                            }
                        }
						Console.WriteLine("The record was inserted successfully");
					}
                    conn.Close();
				}
                Console.WriteLine("End: AddTransaction");
                return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to insert record - reason: " + ex.Message);
                return false;
			}
        }

        public async Task<string> readTransactionRecords(int accountPK){
			Console.WriteLine("Start: ReadTransactiionRecord");
			// create a connection string for the database
			var connectionString = string.Format("Data Source={0};Version=3;", pathToDatabase);
			try
			{
				using (var conn = new SqliteConnection((connectionString)))
				{
					await conn.OpenAsync();
					using (var command = conn.CreateCommand())
					{
						command.CommandText = "SELECT PK, AccountPK, Date, Title, Amount, Category, Type_ToAccount, Notes FROM " + transactionTableName + " WHERE AccountPK = @accountPK";
                        command.Parameters.Add("@accountPK", DbType.Int32).Value = accountPK;
                        command.CommandType = CommandType.Text;
						var r = command.ExecuteReader();
						Console.WriteLine("Reading data");
                        while (r.Read())
                        {
                            Console.WriteLine("PK={0}; AccountPK={1}, Date={2}; Title={3}; Amount={4}; Category={5}; Type_ToAccount={6}; Notes={7};",
                                              r["PK"].ToString(),
                                              r["AccountPK"].ToString(),
                                              r["Date"].ToString(),
                                              r["Title"].ToString(),
                                              r["Amount"].ToString(),
                                              r["Category"].ToString(),
                                              r["Type_ToAccount"].ToString(),
                                              r["Notes"].ToString());

                            Transactions.transactions.Add(new Transaction() { PK = Convert.ToInt32(r["PK"].ToString()), AccountPK = Convert.ToInt32(r["AccountPK"].ToString()), Date = Convert.ToDateTime(r["Date"].ToString()), Title = r["Title"].ToString(), Amount = Convert.ToDecimal(r["Amount"].ToString()), Category = r["Category"].ToString(), Type_ToAccount = r["Type_ToAccount"].ToString(), Notes = r["Notes"].ToString() });
                        }
						Console.WriteLine("The records were read successfully");
					}
                    conn.Close();
				}
				Console.WriteLine("End: ReadTransactiionRecord");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to read record - reason: " + ex.Message);
			}
            return "";
        }

		public async Task readAccounts()
		{
			Console.WriteLine("Start: ReadAccounts");
            //MainActivity.accounts = new System.Collections.Generic.List<Account>();
            //MainActivity.lstAccounts = FindViewById<Android.Widget.ListView>(Resource.Id.lstAccounts);
            // create a connection string for the database
            var connectionString = string.Format("Data Source={0};Version=3;", pathToDatabase);
            decimal sum = 0;
			try
			{
                //MainActivity.accounts = new System.Collections.Generic.List<Account>();

                using (var conn = new SqliteConnection((connectionString)))
				{
					await conn.OpenAsync();
					using (var command = conn.CreateCommand())
					{
						command.CommandText = "SELECT PK, Name, Type, Note FROM " + accountTableName;
						command.CommandType = CommandType.Text;
						var r = command.ExecuteReader();
						Console.WriteLine("Reading data");
                        while (r.Read())
                        {
                            Console.WriteLine("PK={0}; Name={1}, Type={2}; Note={3}",
                                              r["PK"].ToString(),
                                              r["Name"].ToString(),
                                              r["Type"].ToString(),
                                              r["Note"].ToString());

                            sum = getAccountBalance(Convert.ToInt32(r["PK"]));
                            
                            MainActivity.accounts.Add(new Account() { PK = Convert.ToInt32(r["PK"]), Name = r["Name"].ToString(), Type = r["Type"].ToString(), Note = r["Note"].ToString(), Balance = sum });
                            //MainActivity.accountAdapter.NotifyDataSetChanged();
                            //MainActivity.lstAccounts.Adapter = MainActivity.accountAdapter;
                        }
                        //MainActivity.mLayoutManager = new Android.Support.V7.Widget.LinearLayoutManager(this);
                        //MainActivity.mRecyclerView.SetLayoutManager(MainActivity.mLayoutManager);
                        //MainActivity.accountAdapter = new AccountListViewAdapter(MainActivity.accounts);
                        //MainActivity.accountAdapter.NotifyDataSetChanged();
                        Console.WriteLine("The records were read successfully");
					}
					conn.Close();
				}
				Console.WriteLine("End: ReadAccounts");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Failed to read record - reason: " + ex.Message);
			}
		}

        private decimal getAccountBalance(int accountPK)
        {
            Console.WriteLine("Start: ReadAccounts");
            //MainActivity.accounts = new System.Collections.Generic.List<Account>();
            //MainActivity.lstAccounts = FindViewById<Android.Widget.ListView>(Resource.Id.lstAccounts);
            // create a connection string for the database
            var connectionString = string.Format("Data Source={0};Version=3;", pathToDatabase);
            decimal sum = 0;
            try
            {
                using (var conn = new SqliteConnection((connectionString)))
                {
                    conn.OpenAsync();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "SELECT sum(Amount) balance FROM " + transactionTableName + " WHERE AccountPK = @accountPK";
                        command.Parameters.Add("@accountPK", DbType.Int32).Value = accountPK;
                        command.CommandType = CommandType.Text;
                        sum = Convert.ToDecimal(command.ExecuteScalar());

                        Console.WriteLine("The records were read successfully");                                            
                    }
                    conn.Close();                    
                }
                Console.WriteLine("End: ReadAccounts");

                return sum;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read record - reason: " + ex.Message);
                return sum;
            }
        }

        public async Task<bool> deleteAccount(int accountPK){
            Console.WriteLine("Start: DeleteAccount");

            // create a connection string for the database
            var connectionString = string.Format("Data Source={0};Version=3;", pathToDatabase);
            try
            {
                using (var conn = new SqliteConnection((connectionString)))
                {
                    await conn.OpenAsync();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM " + accountTableName + " WHERE PK = @accountPK; DELETE FROM " + transactionTableName + " WHERE AccountPK = @accountPK;";
                        command.Parameters.Add("@accountPK", DbType.Int32).Value = accountPK;
                        command.CommandType = CommandType.Text;
                        await command.ExecuteNonQueryAsync();

                        //go through each account and remove the account we just deleted from the database
                        foreach (Account act in MainActivity.accounts)
                        {
                            if (act.PK == accountPK)
                            {
                                MainActivity.accounts.Remove(act);
                                MainActivity.accountAdapter.NotifyDataSetChanged();
                                break;
                            }
                        }
                        Console.WriteLine("The account was deleted successfully");
                    }
                    conn.Close();
                }
                Console.WriteLine("End: DeleteAccount");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to delete account - reason: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> deleteTransaction(int accountPK, int transactionPK)
        {
            Console.WriteLine("Start: DeleteTransaction");

            // create a connection string for the database
            var connectionString = string.Format("Data Source={0};Version=3;", pathToDatabase);
            try
            {
                using (var conn = new SqliteConnection((connectionString)))
                {
                    await conn.OpenAsync();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM " + transactionTableName + " WHERE PK = @transactionPK;";
                        command.Parameters.Add("@transactionPK", DbType.Int32).Value = transactionPK;
                        command.CommandType = CommandType.Text;
                        await command.ExecuteNonQueryAsync();

                        //go through each account and remove the account we just deleted from the database
                        foreach (Transaction trans in Transactions.transactions)
                        {
                            if (trans.PK == transactionPK)
                            {
                                Transactions.transactions.Remove(trans);
                                Transactions.transactionAdapter.NotifyDataSetChanged();
                                break;
                            }
                        }

                        //get new account balance after transaction add
                        var newAccountBalance = getAccountBalance(accountPK);

                        //go through each account and update the account with the new balance
                        foreach (Account act in MainActivity.accounts)
                        {
                            if (act.PK == accountPK)
                            {
                                act.Balance = newAccountBalance;
                                MainActivity.accountAdapter.NotifyDataSetChanged();
                                break;
                            }
                        }
                        Console.WriteLine("The transaction was deleted successfully");
                    }
                    conn.Close();
                }
                Console.WriteLine("End: DeleteTransaction");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to delete transaction - reason: " + ex.Message);
                return false;
            }
        }
    }
}