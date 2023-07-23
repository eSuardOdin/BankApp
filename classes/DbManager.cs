using Microsoft.Data.Sqlite;
using System;
using System.Threading;
namespace Db
{

    public class DbManager {
        private string DbPath {get; set;}
        
        
        public DbManager() 
        {
            DbPath = "bank_app.db.sqlite";
        }

        /// <summary>
        /// Opens connection to the database
        /// </summary>
        public SqliteConnection OpenConnection()
        {
            var connection = new SqliteConnection($"Data Source = {DbPath}");
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Execute a sql query and print an
        /// error if Exception
        /// </summary>
        /// <param name="query">The query to print</param>
        private void ExecuteNonQuery(string query)
        {
            using var connection = OpenConnection();
            {
                var cmd = new SqliteCommand(query, connection);
                try {
                    cmd.ExecuteNonQuery();
                } catch(Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
            connection.Close();
        }


        /// <summary>
        /// Creates the bankapp database
        /// </summary>
        public void ConstructDb()
        {
            bool isDbNew = !System.IO.File.Exists(DbPath);
            if(isDbNew) 
            {
                ExecuteNonQuery(@"
                        CREATE TABLE IF NOT EXISTS AppUsers (
                            id_user INTEGER PRIMARY KEY, 
                            login_user TEXT NOT NULL UNIQUE, 
                            pwd_user TEXT NOT NULL
                        );");

                ExecuteNonQuery(@"
                    CREATE TABLE IF NOT EXISTS Accounts (
                        id_account INTEGER PRIMARY KEY, 
                        libelle_account TEXT NOT NULL,
                        id_user_fkaccount INTEGER NOT NULL,
                        FOREIGN KEY(id_user_fkaccount) REFERENCES AppUsers(id_user)
                    );");
            
                ExecuteNonQuery(@"
                    CREATE TABLE IF NOT EXISTS AppTypes (
                        id_type INTEGER PRIMARY KEY, 
                        libelle_type TEXT NOT NULL, 
                        id_user_fktype INTEGER
                    );");

                ExecuteNonQuery(@"
                    CREATE TABLE IF NOT EXISTS Transactions (
                        id_transac INTEGER PRIMARY KEY, 
                        id_account_fktransac INTEGER NOT NULL, 
                        amount_transac REAL NOT NULL, 
                        date_transac DATE NOT NULL, 
                        libelle_transac TEXT NOT NULL, 
                        description_transac TEXT, 
                        FOREIGN KEY(id_account_fktransac) REFERENCES Accounts(id_account)
                    );");
                
                ExecuteNonQuery(@"
                    CREATE TABLE IF NOT EXISTS Types_Transactions (
                        id_type_fktypetransac INTEGER NOT NULL,
                        id_transac_fktypetransac INTEGER NOT NULL,
                        PRIMARY KEY(id_type_fktypetransac, id_transac_fktypetransac),
                        FOREIGN KEY(id_type_fktypetransac) REFERENCES AppTypes(id_type),
                        FOREIGN KEY(id_transac_fktypetransac) REFERENCES Transactions(id_transac)
                    );
                ");

                // ExecuteNonQuery(@"
                //     CREATE TABLE IF NOT EXISTS Projects (
                //         id_project INTEGER PRIMARY KEY,
                //         id_user_fkproject INTEGER NOT NULL,
                //         libelle_project TEXT NOT NULL,
                //         date_project DATE,
                //         FOREIGN KEY(id_user_fkproject) REFERENCES AppUsers(id_user) 
                //     );");

                // ExecuteNonQuery(@"
                //     CREATE TABLE IF NOT EXISTS Costs (
                //         id_cost INTEGER PRIMARY KEY,
                //         id_project_fkcost INTEGER NOT NULL,
                //         libelle_cost TEXT NOT NULL,
                //         description_cost TEXT,
                //         FOREIGN KEY(id_project_fkcost) REFERENCES Projects(id_project)
                //     );");

                // ExecuteNonQuery(@"
                //     CREATE TABLE IF NOT EXISTS Types_Costs (
                //         id_type_fktypecost INTEGER NOT NULL,
                //         id_cost_fktypecost INTEGER NOT NULL,
                //         PRIMARY KEY(id_type_fktypecost, id_cost_fktypecost),
                //         FOREIGN KEY(id_type_fktypecost) REFERENCES Types(id_type),
                //         FOREIGN KEY(id_cost_fktypecost) REFERENCES Costs(id_cost) 
                //     );");
            }
        }

    }

}
