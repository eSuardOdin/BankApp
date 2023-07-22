namespace Db
{
    using Microsoft.Data.Sqlite;

    public class DbManager {
        private string DbPath {get; init;}

        /// <summary>
        /// Constructor of DbManager
        /// </summary>
        public DbManager() 
        {
            DbPath = "bank_app.db.sqlite";
            bool isDbNew = !System.IO.File.Exists(DbPath);
            if (isDbNew)
            {
                ConstructDB();
            }

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
    /// Closes connection to the database
    /// </summary>
    public void CloseConnection(SqliteConnection connection)
    {
        connection.Close();
    }

    /// <summary>
    /// Execute a sql query and print an
    /// error if Exception
    /// </summary>
    /// <param name="query">The query to print</param>
    public void ExecuteNonQuery(string query)
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


    public void ExecuteInsertQuery(string query, Dictionary<string, object> parameters)
    {
        using var connection = OpenConnection();
        {
            var cmd = new SqliteCommand(query, connection);
            try {
                foreach(var parameter in parameters) 
                {
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                cmd.ExecuteNonQuery();

            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
        }
        connection.Close();
    }


    public SqliteDataReader ExecuteSelectQuery(string query, Dictionary<string, object> parameters) 
    {
        using var connection = OpenConnection();
        connection.Open();
        using var cmd = new SqliteCommand(query, connection);
        try 
        {
            foreach(var parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            return cmd.ExecuteReader();
        } 
        catch (Exception e) 
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }
    /// <summary>
    /// Creates the bankapp database
    /// </summary>
    private void ConstructDB()
    {
        ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS Utilisateurs (
                    id_user INTEGER PRIMARY KEY, 
                    login_user TEXT NOT NULL UNIQUE, 
                    pwd_user TEXT NOT NULL
                );");

        ExecuteNonQuery(@"
            CREATE TABLE IF NOT EXISTS Comptes (
                id_compte INTEGER PRIMARY KEY, 
                libelle_compte TEXT NOT NULL,
                id_user_fkcompte INTEGER NOT NULL,
                FOREIGN KEY(id_user_fkcompte) REFERENCES Utilisateurs(id_user)
            );");
    
        ExecuteNonQuery(@"
            CREATE TABLE IF NOT EXISTS Types (
                id_type INTEGER PRIMARY KEY, 
                libelle_type TEXT NOT NULL, 
                id_user_fktype INTEGER, 
                FOREIGN KEY(id_user_fktype) REFERENCES Utilisateurs(id_user)
            );");

        ExecuteNonQuery(@"
            CREATE TABLE IF NOT EXISTS Transactions (
                id_transac INTEGER PRIMARY KEY, 
                id_compte_fktransac INTEGER NOT NULL, 
                montant_transac REAL NOT NULL, 
                date_transac DATE NOT NULL, 
                libelle_transac TEXT NOT NULL, 
                description_transac TEXT, 
                FOREIGN KEY(id_compte_fktransac) REFERENCES Comptes(id_compte)
            );");
        
        ExecuteNonQuery(@"
            CREATE TABLE IF NOT EXISTS Types_Transactions (
                id_type_fktypetransac INTEGER NOT NULL,
                id_transac_fktypetransac INTEGER NOT NULL,
                PRIMARY KEY(id_type_fktypetransac, id_transac_fktypetransac),
                FOREIGN KEY(id_type_fktypetransac) REFERENCES Types(id_type),
                FOREIGN KEY(id_transac_fktypetransac) REFERENCES Transactions(id_transac)
            );
        ");

        ExecuteNonQuery(@"
            CREATE TABLE IF NOT EXISTS Projets (
                id_projet INTEGER PRIMARY KEY,
                id_user_fkprojet INTEGER NOT NULL,
                libelle_projet TEXT NOT NULL,
                date_projet DATE,
                FOREIGN KEY(id_user_fkprojet) REFERENCES Utilisateurs(id_user) 
            );");

        ExecuteNonQuery(@"
            CREATE TABLE IF NOT EXISTS Couts (
                id_cout INTEGER PRIMARY KEY,
                id_projet_fkcout INTEGER NOT NULL,
                libelle_cout TEXT NOT NULL,
                description_cout TEXT,
                FOREIGN KEY(id_projet_fkcout) REFERENCES Projets(id_projet)
            );");

        ExecuteNonQuery(@"
            CREATE TABLE IF NOT EXISTS Types_Couts (
                id_type_fktypecout INTEGER NOT NULL,
                id_cout_fktypecout INTEGER NOT NULL,
                PRIMARY KEY(id_type_fktypecout, id_cout_fktypecout),
                FOREIGN KEY(id_type_fktypecout) REFERENCES Types(id_type),
                FOREIGN KEY(id_cout_fktypecout) REFERENCES Couts(id_cout) 
            );");
        }

    }

}
