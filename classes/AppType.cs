namespace Entities {
    using Db;
    using Microsoft.Data.Sqlite;
    public class AppType 
    {
        private int IdType { get; set; }
        public string LibelleType { get; private set; }
        private int IdUserFkType { get; set; }
        private Db.DbManager DbManager { get; set; } 

        public AppType(int idType = 0, string libelleType = "", int idUserFkType = 0) 
        {
            IdType = idType;
            LibelleType = libelleType;
            IdUserFkType = idUserFkType;
            DbManager = new Db.DbManager();
        }

        /// <summary>
        /// Adds a type to database
        /// </summary>
        public void AddAppTypeToDb ()
        {
            // Throw exception if empty string
            if(this.LibelleType == "") { this.LibelleType = null; }
            
            // Setting the parameters to insert in query
            var parameters = new Dictionary<string, object> {
                { "@libelle_type", this.LibelleType },
                { "@id_user_fktype", this.IdUserFkType },
            };
            if(IdUserFkType == 0) 
            {
                parameters["@id_user_fktype"] = "NULL";
            }
            // Open connection to database
            using (var connection = DbManager.OpenConnection())
            {
                var query = $"INSERT INTO AppTypes (libelle_type, id_user_fktype) VALUES (@libelle_type, @id_user_fktype);";
                var command = new SqliteCommand(query, connection);
                try 
                {
                    // Prevent sql injection
                    foreach(var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Type {this.LibelleType} inserted in DB");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}