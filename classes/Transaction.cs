namespace Entities {
    using Db;
    using System;
    using Microsoft.Data.Sqlite;
    public class Transaction
    {
        private int IdTransac { get; set; }
        private int IdAccountFkTransac { get; set; }
        private decimal AmountTransac { get; set; }
        private DateTime DateTransac { get; set; }
        public string LibelleTransac { get; private set; }
        public string? DescriptionTransac { get; private set; }
        private int IdUserFkType { get; set; }
        private Db.DbManager DbManager { get; set; } 

        
        
    }
}