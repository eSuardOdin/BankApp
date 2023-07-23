namespace Entities {
    using Db;
    using Microsoft.Data.Sqlite;
    public class AppUser 
    {
        private int IdUser {get; set;}
        private string LoginUser {get; set;}
        private string PwdUser {get; set;}
        private Db.DbManager DbManager {get; set;}
        private List<Account> Accounts {get; set;} 
        private List<AppType> AppTypes {get; set;} 

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
            AppTypes = new List<AppType>();
            DbManager = new Db.DbManager();
            GetAllAccounts();
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
                            this.IdUser = reader.GetInt32(0);
                            this.LoginUser = reader.GetString(1);
                            this.PwdUser = reader.GetString(2);
                            // Get the accounts for the user
                            GetAllAccounts();
                            // Get all types for the user
                            GetAllTypes();
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
        /// Gets all accounts of an user.
        /// Used to init List<Account>
        /// </summary>
        private void GetAllAccounts() 
        {   
            Console.WriteLine($"Getting all accounts of user id : {IdUser}");
             // Open connection to the database
            using (var connection = DbManager.OpenConnection())
            {
                // Setting the parameters to insert in query
                var parameters = new Dictionary<string, object>();
                string query = "SELECT * FROM Accounts WHERE id_user_fkaccount = @id_user";
                parameters["@id_user"] = this.IdUser;
                var command = new SqliteCommand(query, connection);
                try
                {
                    // Replace parameters to prevent injection
                    foreach(var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    using (var reader = command.ExecuteReader())
                    {
                        // Push every Accounts in the list without duplication
                        while(reader.Read())
                        {
                            var account = new Account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                            bool isPresent = Accounts.Any(acc => acc.LibelleAccount == account.LibelleAccount );
                            if (!isPresent) { this.Accounts.Add(account); }  
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
        /// 
        /// I'm commenting possibility to stop entering accounts after a delimited amount -> 
        /// // if(this.Accounts.Count >= 10) { return ;}
        /// </summary>
        /// <param name="libelle">Name of the account</param>
        public void CreateAccount (string libelle)
        {
            // Creating the account
            var account = new Account(libelleAccount: libelle, idUserFkAccount: this.IdUser);
            
            // Checking if it's already present in List<Account>
            bool isPresent = Accounts.Any(acc => acc.LibelleAccount == account.LibelleAccount );
            
            // If not present we create it
            if (!isPresent) 
            { 
                this.Accounts.Add(account); 
                account.AddAccountToDb();      
            }
            else 
            { 
                Console.WriteLine("Cannot add two accounts with same name on same user"); 
            }
        }





        /// <summary>
        /// Gets all accounts of an user.
        /// Used to init List<Account>
        /// </summary>
        private void GetAllTypes() 
        {   
            Console.WriteLine($"Getting all types of user id : {IdUser}");
             // Open connection to the database
            using (var connection = DbManager.OpenConnection())
            {
                // Setting the parameters to insert in query
                var parameters = new Dictionary<string, object>();
                string query = "SELECT * FROM AppTypes WHERE id_user_fktype = @id_user";
                parameters["@id_user"] = this.IdUser;
                var command = new SqliteCommand(query, connection);
                try
                {
                    // Replace parameters to prevent injection
                    foreach(var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    using (var reader = command.ExecuteReader())
                    {
                        // Push every Types in the list without duplication
                        while(reader.Read())
                        {
                            var newType = new AppType(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                            bool isPresent = AppTypes.Any(listType => listType.LibelleType == newType.LibelleType );
                            if (!isPresent) { this.AppTypes.Add(newType); }  
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void CreateType (string libelle)
        {
            // Creating the account
            var newType = new AppType(libelleType: libelle, idUserFkType: this.IdUser);
            
            // Checking if it's already present in List<Types>
            bool isPresent = AppTypes.Any(listType => listType.LibelleType == newType.LibelleType );
            
            // If not present we create it
            if (!isPresent) 
            { 
                this.AppTypes.Add(newType); 
                newType.AddAppTypeToDb();      
            }
            else 
            { 
                Console.WriteLine("Cannot add two accounts with same name on same user"); 
            }
        }

        public void CreateTransaction (int idAccount, decimal amount)
        {

        }
    }    
}
