using System.Runtime.CompilerServices;
using Entities;
Db.DbManager dbMan = new Db.DbManager();
if (dbMan.ConstructDb())
{
    var type1 = new AppType(libelleType: "Loisir");
    var type3 = new AppType(libelleType: "Enfants");
    var type2 = new AppType(libelleType: "Travail");

    type1.AddAppTypeToDb();
    type2.AddAppTypeToDb();
    type3.AddAppTypeToDb();

    AppUser ut1 = new AppUser(0, "wannot", "p@ssw0rd");
    AppUser ut2 = new AppUser(0, "floflo", "m0t2p4ss3");
    ut1.AddUserToDb();
    ut2.AddUserToDb();
}



AppUser userSelected = new AppUser();

Console.Write("Choisissez un utilisateur (1 ou 2 pour le test) : ");
int idInput = Convert.ToInt32(Console.ReadLine()); // To delete obviously
userSelected.SelectUser(id: idInput);


// Bank account(s) creation :
Console.Write("Do you want to create a new bank account for the user ? (Y/N)");
string res = Console.ReadLine().ToUpper();
if(res == "Y")
{
    while (res == "Y")
    {
        Console.Write("Name the account : ");
        string accountName = Console.ReadLine();
        userSelected.CreateAccount(accountName);
        Console.Write("Another bank account ? (Y/N)");
        res = Console.ReadLine().ToUpper();
    }
}


// Transaction type(s) creation :
Console.Write("Do you want to create a new type for the user ? (Y/N)");
res = Console.ReadLine().ToUpper();
if(res == "Y")
{
    while (res == "Y")
    {
        Console.Write("Name the type : ");
        string typeName = Console.ReadLine();
        userSelected.CreateType(typeName);
        Console.Write("Another type ? ");
        res = Console.ReadLine().ToUpper();
    }
}



// Transaction(s) creation :
Console.Write("Do you want to create a new transaction for the user ? (Y/N)");
res = Console.ReadLine().ToUpper();
if(res == "Y")
{
    while (res == "Y")
    {
        Console.Write("Name the transaction : ");
        string transName = Console.ReadLine();

        Console.Write("You can enter a description : ");
        string description = Console.ReadLine();

        Console.Write("Choose an account id : ");
        int idAccount = Convert.ToInt32(Console.ReadLine());
        
        Console.Write("Choose an amount : ");
        decimal amount = Convert.ToDecimal(Console.ReadLine());

        DateTime date = DateTime.Now;
        
        string res2 = "Y";
        var typesId = new List<int>();
        Console.Write("Enter at least a type of transaction's id : ");
        while (res2 == "Y")
        {
            Console.Write("\nType name : ");
            int idType = Convert.ToInt32(Console.ReadLine());
            typesId.Add(idType);
            Console.Write("Another type ? (Y/N)");
            res2 = Console.ReadLine().ToUpper();
        }
            
        
        userSelected.CreateTransaction(idAccount, amount, date, transName, typesId, description);
        res = Console.ReadLine().ToUpper();
    }
}


Console.WriteLine("Pas de crash !");
