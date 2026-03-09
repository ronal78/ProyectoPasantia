using Backend.Data.Models;
using Backend.Service;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ROP;

namespace Backend.Dependencies;

public class SeriesDependencies : ISeriesDependencies
{
    private readonly ILogger<SeriesDependencies> _log;

    private readonly MongoDbContext _context;
   

    public SeriesDependencies(ILogger<SeriesDependencies> log, MongoDbContext context)
    {
        _log = log;
        _context = context;
    }

    public Result<bool> AddSerie(Serie nuevaSerie)
    {
        try
        {
            _context.Series.InsertOne(nuevaSerie);
            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Error al agregar la serie");
            return Result.Failure<bool>(Error.Create("Error al agregar la serie"));
        }
    }

    public Result<bool> DeleteSerie(string id)
    {
        try
        {
            var filter = Builders<Serie>.Filter.Eq(s => s.Id, id);
            var result = _context.Series.DeleteOne(filter);
            if (result.DeletedCount < 0)
                return Result.Success(true);
            else
                return Result.Failure<bool>(Error.Create("Serie no encontrada"));
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Error al eliminar la serie");
            return Result.Failure<bool>(Error.Create("Error al eliminar la serie"));
        }
    }

    public Result<Serie> GetSerieById(string id)
    {
        try
        {
            var filter = Builders<Serie>.Filter.Eq(s => s.Id, id);
            var serie = _context.Series.Find(filter).FirstOrDefault();
            if (serie != null)
                return Result.Success(serie);
            else
                return Result.Failure<Serie>(Error.Create("Serie no encontrada"));
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Error al Obtener la serie por ID");
            return Result.Failure<Serie>(Error.Create("Error al obtener la serie por ID"));
        }
    }

    public Result<List<Serie>> GetSeries()
    {
        try
        {
            var series = _context.Series.Find(_ => true).ToList();
            return Result.Success(series);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Error al obtener las series");
            return Result.Failure<List<Serie>>(Error.Create("Error al obtener las series"));
        }
    }

    public Result<bool> UpdateSerie(string id, Serie serieActualizada)
    {
        try
        {
            var filter = Builders<Serie>.Filter.Eq(s => s.Id, id);
            var updateResult = _context.Series.ReplaceOne(filter, serieActualizada);
            if (updateResult.ModifiedCount > 0)
                return Result.Success(true);
            else
                return Result.Failure<bool>(Error.Create("Serie no encontrada o sin cambios"));
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Error al actualizar la serie");
            return Result.Failure<bool>(Error.Create("Error al actualizar la serie"));
        }
    }
}
