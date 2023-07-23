namespace Entities {
    using Db;
    using Microsoft.Data.Sqlite;
    public class AppUser 
    {
        private int IdUser {get; set;}
        private string LoginUser {get; set;}
        private string PwdUser {get; set;}
        private Db.DbManager DbManager {get; set;}
        private List<Account> Accounts {get; set;} // Not used for now, will see if useful.


        /// <summary>
        /// Construct a whole User when all attributes are defined
        /// </summary>
        /// <param name="idUser">User's id</param>
        /// <param name="loginUser">User's login</param>
        /// <param name="passwordUser">User's password</param>
        public AppUser(int idUser = 0, string loginUser = "", string passwordUser = "") 
        {
            IdUser = idUser;
            LoginUser = loginUser;
            PwdUser = passwordUser;
            Accounts = new List<Account>();
            DbManager = new Db.DbManager();
        }

        /// <summary>
        /// Adding an user to the database
        /// </summary>
        public void AddUserToDb()
        {
            // Setting the parameters to insert in query
            var parameters = new Dictionary<string, object> {
                { "@login_user", this.LoginUser },
                { "@pwd_user", this.PwdUser },
            };

            // Open connection to database
            using (var connection = DbManager.OpenConnection())
            {
                var query = $"INSERT INTO AppUsers (login_user, pwd_user) VALUES (@login_user, @pwd_user);";
                var command = new SqliteCommand(query, connection);
                try
                {
                    // No injection
                    foreach(var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    command.ExecuteNonQuery();
                    Console.WriteLine($"User {this.LoginUser} inserted in DB");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            };
        }

        /// <summary>
        /// Find a user by id or login
        /// </summary>
        /// <param name="login"></param>
        /// <param name="id"></param>
        public void SelectUser(string login = null, int id = 0)
        {

            // Open connection to the database
            using (var connection = DbManager.OpenConnection())
            {
                // Setting the parameters to insert in query
                var parameters = new Dictionary<string, object>();
                string query;
                if (!string.IsNullOrEmpty(login))
                {
                    query = "SELECT * FROM AppUsers WHERE login_user = @login_user";
                    parameters["@login_user"] = login;
                }
                else if (id != 0)
                {
                    query = "SELECT * FROM AppUsers WHERE id_user = @id_user";
                    parameters["@id_user"] = id;
                }
                else
                {
                    // Both login and id are empty, no valid search condition provided
                    Console.WriteLine("Please provide either login or id to search for a user.");
                    return;
                }

                var command = new SqliteCommand(query, connection);
                try
                {
                    foreach(var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    using (var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Console.WriteLine(@$"
                            User Id : {reader.GetInt32(0)}
                            User Login : {reader.GetString(1)}
                            User Password : {reader.GetString(2)}
                            ");
                            this.IdUser = reader.GetInt32(0);
                            this.LoginUser = reader.GetString(1);
                            this.PwdUser = reader.GetString(2);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }


        /// <summary>
        /// Create an account for the user
        /// </summary>
        public void CreateAccount (string libelle)
        {
            var account = new Account(libelleAccount: libelle, idUserFkAccount: this.IdUser);
            account.AddAccountToDb();
        }

        public void CreateTransaction (int idAccount, decimal amount)
        {

        }
    }    
}
