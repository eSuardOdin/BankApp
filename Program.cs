using Entities;
Db.DbManager dbMan = new Db.DbManager();
dbMan.ConstructDb();
// Console.Write("username : ");
// string log = Console.ReadLine();
// Console.Write("password : ");
// string pwd = Console.ReadLine();

// Console.Write("id : ");
// int id = Console.ReadLine();

AppUser ut1 = new AppUser(0, "wannot", "p@ssw0rd");
AppUser ut2 = new AppUser(0, "floflo", "m0t2p4ss3");
ut1.AddUserToDb();
ut2.AddUserToDb();

AppUser ut3 = new AppUser();
ut3.SelectUser(id:1);
// AppUser ut4 = AppUser.SelectUser(login:"wannot");


Console.WriteLine("Pas de crash !");
