namespace Entities {
    using Db;
    using System;
    using Microsoft.Data.Sqlite;
    public class Transaction
    {
        private int IdTransac { get; set; }
        private int IdAccountFkTransac { get; set; }
        public decimal AmountTransac { get; private set; }
        public DateTime DateTransac { get; private set; }
        public string LibelleTransac { get; private set; }
        public string? DescriptionTransac { get; private set; }
        private int IdUserFkType { get; set; }
        private Db.DbManager DbManager { get; set; } 

        public Transaction(int idAccountFkTransac, int idUserFkType, decimal amountTransac, DateTime dateTransac, string libelleTransac, int idTransac = 0, string descriptionTransac = null ) 
        {
            IdTransac = idTransac;
            IdAccountFkTransac = idAccountFkTransac;
            IdUserFkType = idUserFkType;
            AmountTransac = amountTransac;
            DateTransac = dateTransac;
            LibelleTransac = libelleTransac;
            DescriptionTransac = descriptionTransac;
            DbManager = new Db.DbManager();
        }

        public void AddTransacToDb()
        {
            var parameters = new Dictionary<string, object>() {
                { "@id_account_fktransac", this.IdAccountFkTransac},
                {"@amout_transac", this.AmountTransac},
                {"@date_transac", this.DateTransac.ToString("dd MMM yyyy")},
                {"@libelle_transac", this.LibelleTransac},
                {"@description_transac", this.DescriptionTransac}
            };
        }
        
    }
}