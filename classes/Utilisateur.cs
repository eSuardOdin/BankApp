namespace Entities {
    using Db;
    public class Utilisateur 
    {
        private int IdUser {get; init;}
        private string LoginUser {get; init;}
        private string PwdUser {get; init;}

        public Utilisateur(int idUser, string loginUser, string passwordUser) 
        {
            IdUser = idUser;
            LoginUser = loginUser;
            PwdUser = passwordUser;
        }

        public void AddUtilisateurToDb()
        {
            var dbMan = new DbManager();
            Console.WriteLine($"INSERT INTO Utilisateurs (login_user, pwd_user) VALUES ({this.LoginUser}, {this.PwdUser});");
            dbMan.ExecuteNonQuery($"INSERT INTO Utilisateurs (login_user, pwd_user) VALUES (\"{this.LoginUser}\", \"{this.PwdUser}\");");

            
        }
    }    
}
