namespace Entities {
    using Db;
    using System;
    using Microsoft.Data.Sqlite;
    public class Transaction
    {
        private int IdTransac { get; set; }
        private int IdAccountFkTransac { get; set; }
        public decimal AmountTransac { get; private set; }
        public DateTime DateTransac { get; private set; }
        public string LibelleTransac { get; private set; }
        public string? DescriptionTransac { get; private set; }
        private List<AppType> AppTypes { get; set; }
        private DbManager DbManager { get; set; } 

        public Transaction(int idAccountFkTransac, decimal amountTransac, DateTime dateTransac, string libelleTransac, string descriptionTransac = null, int idTransac = 0 ) 
        {
            IdTransac = idTransac;
            IdAccountFkTransac = idAccountFkTransac;
            AmountTransac = amountTransac;
            DateTransac = dateTransac;
            LibelleTransac = libelleTransac;
            DescriptionTransac = descriptionTransac;
            AppTypes = new List<AppType>();
            DbManager = new DbManager();
        }

        public void AddTransacToDb()
        {
            var parameters = new Dictionary<string, object>() {
                { "@id_account_fktransac", IdAccountFkTransac},
                {"@amout_transac", AmountTransac},
                {"@date_transac", DateTransac.ToString("dd MMM yyyy")},
                {"@libelle_transac", LibelleTransac},
                {"@description_transac", DescriptionTransac}
            };

            using (var connection = DbManager.OpenConnection())
            {
                var query = $"INSERT INTO Transactions (id_account_fktransac, amount_transac, date_transac, libelle_transac, description_transac) VALUES (@id_account_fktransac, @amout_transac, @date_transac, @libelle_transac, @description_transac);";

                var command = new SqliteCommand(query, connection);

                try
                {
                    foreach(var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Transaction '{this.LibelleTransac}' inserted in DB");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        
    }
}