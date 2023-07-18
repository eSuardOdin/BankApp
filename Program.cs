﻿using System;
// using System.Data.SQLite;
using Microsoft.Data.Sqlite;

string dbPath = "bank_app.db.sqlite";
bool isDbNew = !System.IO.File.Exists(dbPath);

var connection = new SqliteConnection($"Data Source = {dbPath}");
connection.Open();

// if (isDbNew)
// {
    Console.WriteLine("Create db");
    var createUser = new SqliteCommand("CREATE TABLE IF NOT EXISTS Utilisateurs (id_user INTEGER PRIMARY KEY, login_user TEXT NOT NULL, pwd_user TEXT NOT NULL);", connection); 
    var createComptes = new SqliteCommand("CREATE TABLE IF NOT EXISTS Comptes (id_compte INTEGER PRIMARY KEY, libelle_compte TEXT NOT NULL,id_user_fkcompte INTEGER NOT NULL,FOREIGN KEY(id_user_fkcompte) REFERENCES Utilisateurs(id_user));", connection); 
    var createTypes = new SqliteCommand("CREATE TABLE IF NOT EXISTS Types (id_type INTEGER PRIMARY KEY, libelle_type TEXT NOT NULL, id_user_fktype INTEGER, FOREIGN KEY(id_user_fktype) REFERENCES Utilisateurs(id_user))", connection);
    
    createUser.ExecuteNonQuery();
    createComptes.ExecuteNonQuery();
    createTypes.ExecuteNonQuery();
// } else {
//     Console.WriteLine("Db already created");
// }

connection.Close();