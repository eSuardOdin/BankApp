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

        /// <summary>
        /// Adds a transact
        /// </summary>
        /// <param name="idType"></param>
        public void AddTransacToDb(List<int> typesId)
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

                    // Retrieve the last inserted ID
                    query = "SELECT last_insert_rowid();";
                    command.CommandText = query;
                    IdTransac = Convert.ToInt32(command.ExecuteScalar());

                    // Now let's link types to transaction
                    foreach(int id in typesId) 
                    {
                        var secondParameters = new Dictionary<string, object> {
                            {"@id_type_fktypetransac", id},
                            {"@id_transac_fktypetransac", IdTransac}
                        };
                        var secondQuery = @"INSERT INTO Types_Transactions (id_type_fktypetransac, id_transac_fktypetransac) VALUES (@id_type_fktypetransac, @id_transac_fktypetransac)";
                        var secondCommand = new SqliteCommand(secondQuery, connection);

                        try
                        {
                            foreach(var secondParameter in secondParameters)
                            {
                                secondCommand.Parameters.AddWithValue(secondParameter.Key, secondParameter.Value);
                            }
                            secondCommand.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        
    }
}