using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Data.Models;

public class MongoDbContext
{
    
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Pelicula> Peliculas => _database.GetCollection<Pelicula>("Peliculas");

    public IMongoCollection<Serie> Series => _database.GetCollection<Serie>("Series");
}
