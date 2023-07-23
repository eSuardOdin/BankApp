using Entities;
Db.DbManager dbMan = new Db.DbManager();
dbMan.ConstructDb();

AppUser ut1 = new AppUser(0, "wannot", "p@ssw0rd");
AppUser ut2 = new AppUser(0, "floflo", "m0t2p4ss3");
ut1.AddUserToDb();
ut2.AddUserToDb();

AppUser ut3 = new AppUser();
ut3.SelectUser(id:1);

Console.Write("Account to create : ");
string libelle = Console.ReadLine();
ut3.CreateAccount(libelle);

Console.WriteLine("Pas de crash !");
