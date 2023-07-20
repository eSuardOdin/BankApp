namespace Db
{
    using Microsoft.Data.Sqlite;

    public class DbManager {
        private string DbPath {get; init;}

        /// <summary>
        /// Constructor of DbManager
        /// </summary>

        public DbManager(string path) 
        {
            DbPath = path;
            bool isDbNew = !System.IO.File.Exists(DbPath);
            if (isDbNew)
            {
                ConstructDB();
            }

        }

    /// <summary>
    /// Creates the bankapp database
    /// </summary>
        private void ConstructDB()
        {
            var connection = new SqliteConnection($"Data Source = {DbPath}");
            connection.Open();
            var createUser = new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS Utilisateurs (
                        id_user INTEGER PRIMARY KEY, 
                        login_user TEXT NOT NULL, 
                        pwd_user TEXT NOT NULL
                    );", connection);
                    createUser.ExecuteNonQuery();


                var createComptes = new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS Comptes (
                        id_compte INTEGER PRIMARY KEY, 
                        libelle_compte TEXT NOT NULL,
                        id_user_fkcompte INTEGER NOT NULL,
                        FOREIGN KEY(id_user_fkcompte) REFERENCES Utilisateurs(id_user)
                    );", connection);
                createComptes.ExecuteNonQuery();
            

                var createTypes = new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS Types (
                        id_type INTEGER PRIMARY KEY, 
                        libelle_type TEXT NOT NULL, 
                        id_user_fktype INTEGER, 
                        FOREIGN KEY(id_user_fktype) REFERENCES Utilisateurs(id_user)
                    );", connection);
                createTypes.ExecuteNonQuery();


                var createTransactions = new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS Transactions (
                        id_transac INTEGER PRIMARY KEY, 
                        id_compte_fktransac INTEGER NOT NULL, 
                        montant_transac REAL NOT NULL, 
                        date_transac DATE NOT NULL, 
                        libelle_transac TEXT NOT NULL, 
                        description_transac TEXT, 
                        FOREIGN KEY(id_compte_fktransac) REFERENCES Comptes(id_compte)
                    );", connection);
                createTransactions.ExecuteNonQuery();

                
                var createTypesTransactions = new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS Types_Transactions (
                        id_type_fktypetransac INTEGER NOT NULL,
                        id_transac_fktypetransac INTEGER NOT NULL,
                        PRIMARY KEY(id_type_fktypetransac, id_transac_fktypetransac),
                        FOREIGN KEY(id_type_fktypetransac) REFERENCES Types(id_type),
                        FOREIGN KEY(id_transac_fktypetransac) REFERENCES Transactions(id_transac)
                    );
                ",connection);
                createTypesTransactions.ExecuteNonQuery();


                var createProjects = new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS Projets (
                        id_projet INTEGER PRIMARY KEY,
                        id_user_fkprojet INTEGER NOT NULL,
                        libelle_projet TEXT NOT NULL,
                        date_projet DATE,
                        FOREIGN KEY(id_user_fkprojet) REFERENCES Utilisateurs(id_user) 
                    );", connection);
                createProjects.ExecuteNonQuery();


                var createCouts = new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS Couts (
                        id_cout INTEGER PRIMARY KEY,
                        id_projet_fkcout INTEGER NOT NULL,
                        libelle_cout TEXT NOT NULL,
                        description_cout TEXT,
                        FOREIGN KEY(id_projet_fkcout) REFERENCES Projets(id_projet)
                    );", connection);
                createCouts.ExecuteNonQuery();


                var createTypesCouts = new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS Types_Couts (
                        id_type_fktypecout INTEGER NOT NULL,
                        id_cout_fktypecout INTEGER NOT NULL,
                        PRIMARY KEY(id_type_fktypecout, id_cout_fktypecout),
                        FOREIGN KEY(id_type_fktypecout) REFERENCES Types(id_type),
                        FOREIGN KEY(id_cout_fktypecout) REFERENCES Couts(id_cout) 
                    );
                ",connection);
                createTypesCouts.ExecuteNonQuery();

            connection.Close();
        }

        public string GetEntityById(string table, string id_column, int id) 
        {
            string res = "";
            var connection = new SqliteConnection($"Data Source = {DbPath}");
            connection.Open();

            string query = $"SELECT * FROM {table} WHERE {id_column}={id}";
            using var command = new SqliteCommand(query, connection);
            using SqliteDataReader reader = command.ExecuteReader();


            while(reader.Read())
            {
                res +=
            } 
            return res;
        }

    }

}
