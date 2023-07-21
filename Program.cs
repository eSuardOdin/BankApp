using Entities;
var dbManager = new Db.DbManager();

Console.Write("username : ");
string log = Console.ReadLine();
Console.Write("password : ");
string pwd = Console.ReadLine();

Utilisateur ut = new Utilisateur(0, log, pwd);

ut.AddUtilisateurToDb();


Console.WriteLine("Pas de crash !");
