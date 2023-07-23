using Entities;
Db.DbManager dbMan = new Db.DbManager();
dbMan.ConstructDb();

AppUser ut1 = new AppUser(0, "wannot", "p@ssw0rd");
AppUser ut2 = new AppUser(0, "floflo", "m0t2p4ss3");
ut1.AddUserToDb();
ut2.AddUserToDb();

AppUser ut3 = new AppUser();
ut3.SelectUser(id:1);

// Console.Write("Account to create : ");
// string libelle = Console.ReadLine();
// ut3.CreateAccount(libelle);


var type1 = new AppType(libelleType: "Loisir");
var type3 = new AppType(libelleType: "Enfants");
var type2 = new AppType(libelleType: "Travail");

type1.AddAppTypeToDb();
type2.AddAppTypeToDb();
type3.AddAppTypeToDb();


string libelle = Console.ReadLine();
ut3.CreateType(libelle);
Console.WriteLine("Pas de crash !");
