namespace Entities {
    using Db;
    public class Utilisateur 
    {
        private int IdUser {get; set;}
        private string LoginUser {get; set;}
        private string PwdUser {get; set;}

        /// <summary>
        /// Construct a whole User when all attributes are defined
        /// </summary>
        /// <param name="idUser">User's id</param>
        /// <param name="loginUser">User's login</param>
        /// <param name="passwordUser">User's password</param>
        public Utilisateur(int idUser = 0, string loginUser = "", string passwordUser = "") 
        {
            IdUser = idUser;
            LoginUser = loginUser;
            PwdUser = passwordUser;
        }

        public void AddUtilisateurToDb()
        {
            var dbMan = new DbManager();
            var parameters = new Dictionary<string, object> {
                { "@login_user", this.LoginUser },
                { "@pwd_user", this.PwdUser },
            };
            Console.WriteLine($"INSERT INTO Utilisateurs (login_user, pwd_user) VALUES ({this.LoginUser}, {this.PwdUser});");
            dbMan.ExecuteInsertQuery($"INSERT INTO Utilisateurs (login_user, pwd_user) VALUES (@login_user, @pwd_user);", parameters);   
        }


        public static void FindUser(string login = "", int id = 0)
        {
            var dbMan = new DbManager();
            var parameters = new Dictionary<string, object> 
            {
                ["@login_user"] = login,
                ["@id_user"] = id
            };
            if (login != "") {
                object res = dbMan.ExecuteSelectQuery($"SELECT * FROM Utilisateurs WHERE login_user = @login_user", parameters);
            } else if (id != 0) {
                dbMan.ExecuteSelectQuery($"SELECT * FROM Utilisateurs WHERE id_user = @id_user", parameters);
            }
        } 
    }    
}
