using Db;
using Microsoft.Data.Sqlite;
namespace Entities 
{
    public class Account
    {
        private int IdAccount { get; set; }
        private string LibelleAccount { get; set; }
        private int IdUserFkAccount { get; set; }

        public Account (int idAccount, string libelleAccount, int idUserFkAccount)
        {
            IdAccount = idAccount;
            LibelleAccount = libelleAccount;
            IdUserFkAccount = idUserFkAccount;
        }
    }
}