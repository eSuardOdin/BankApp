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
        /// <param name="idUser"></param>
        /// <param name="loginUser"></param>
        /// <param name="passwordUser"></param>
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
    }    
}
