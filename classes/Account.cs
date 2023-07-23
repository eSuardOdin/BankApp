using Db;
using Microsoft.Data.Sqlite;
namespace Entities 
{
    public class Account
    {
        public int IdAccount { get; private set; }
        public string LibelleAccount { get; set; }
        private int IdUserFkAccount { get; set; }
        private Db.DbManager DbManager {get; set;}

        /// <summary>
        /// Constructor for Account
        /// </summary>
        /// <param name="idAccount">The id of the account</param>
        /// <param name="libelleAccount">The libelle of the account</param>
        /// <param name="idUserFkAccount">Account owner's id</param>
        public Account (int idAccount = 0, string libelleAccount = "", int idUserFkAccount = 0)
        {
            IdAccount = idAccount;
            LibelleAccount = libelleAccount;
            IdUserFkAccount = idUserFkAccount;
            DbManager = new Db.DbManager();
        }
        /// <summary>
        /// Add an account to database
        ///  
        /// Possible update : Prevent duplication of libelle for same user 
        /// </summary>
        public void AddAccountToDb ()
        {
            // Throw exception if empty string
            if(this.LibelleAccount == "") { this.LibelleAccount = null; }

            // Setting the parameters to insert in query
            var parameters = new Dictionary<string, object> {
                { "@libelle_account", this.LibelleAccount },
                { "@id_user_fkaccount", this.IdUserFkAccount },
            };
            // Open connection to database
            using (var connection = DbManager.OpenConnection())
            {
                var query = $"INSERT INTO Accounts (libelle_account, id_user_fkaccount) VALUES (@libelle_account, @id_user_fkaccount);";
                var command = new SqliteCommand(query, connection);
                try 
                {
                    // Prevent sql injection
                    foreach(var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Account {this.LibelleAccount} inserted in DB");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}