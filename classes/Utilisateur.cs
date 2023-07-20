namespace Entities {
    public class Utilisateur 
    {
        private int IdUser {get; init;}
        private string LoginUser {get; init;}

        private string PwdUser {get; init;}

        public Utilisateur(int idUser, Db.DbManager dbManager) 
        {
            
        }
    }    
}
