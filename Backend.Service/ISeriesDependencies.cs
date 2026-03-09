using Backend.Data.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Validation;
using ROP;
using System.Collections.Immutable;

namespace Backend.Service;


public interface ISeriesDependencies
{
    public Result<List<Serie>> GetSeries();

    public Result<Serie> GetSerieById(string id);
    public Result<bool> AddSerie(Serie nuevaSerie);

    public Result<bool> UpdateSerie(string id, Serie serieActualizada);

    public Result <bool>DeleteSerie(string id);
}

//Servicio de Series que maneja la logica de negocio y validaciones,
// Delegando las operaciones de datos a traves de ISeriesDependencies.
public class SeriesService
{
    private readonly ISeriesDependencies _dependencies;

    private readonly ILogger<SeriesService> _log;

    public SeriesService(ISeriesDependencies dependencies, ILogger<SeriesService> logds)
    {
        _dependencies = dependencies;
        _log = logds;
    }

    public Result<List<Serie>> GetSeries() => _dependencies.GetSeries();

    public Result<bool> DeleteSerie(string id) => _dependencies.DeleteSerie(id);

    public Result<bool> AddSerie(Serie nuevaserie)
    {
        return ValidateSerie(nuevaserie)
        .Bind(_dependencies.AddSerie);
    }
    

    public Result<bool> UpdateSerie(string id, Serie serieActualizada)
    {
        return GetSerieById(id) //Va a buscar si existe la serie
        .Bind(_ => ValidateSerie(serieActualizada)) // Si existe, valida la serie actualizada
        .Bind(validSerie => _dependencies.UpdateSerie(id, validSerie));

    }

    // Validaciones
    private Result<Serie> ValidateSerie(Serie nuevaSerie)
    {
        _log.LogInformation("Agregando una nueva serie");

        List<Error> errores = new List<Error>();

        if (string.IsNullOrEmpty(nuevaSerie.Titulo))
            errores.Add(Error.Create("El titulo no puede estar vacio"));

        if (string.IsNullOrEmpty(nuevaSerie.Plataforma))
            errores.Add(Error.Create("La plataforma no puede estar vacio"));

        if (nuevaSerie.AnioEstreno == 0)
            errores.Add(Error.Create("El año de estreno no puede estar vacio"));

        if (string.IsNullOrEmpty(nuevaSerie.Genero))
            errores.Add(Error.Create("El Genero no puede estar vacio"));

        if (nuevaSerie.TemporadasEpisodios == null ||
            !nuevaSerie.TemporadasEpisodios.Any())
            errores.Add(Error.Create("Debe tener al menos una temporada"));

        if (nuevaSerie.TemporadasEpisodios != null)
        {
            foreach (var temp in nuevaSerie.TemporadasEpisodios)
            {
                if (temp.temporada <= 0)
                    errores.Add(Error.Create("La temporada no puede ser 0"));

                if (temp.Episodios <= 0)
                    errores.Add(Error.Create("El episodio no puede ser 0"));

            }
        }

        if (errores.Any())
        {
            _log.LogWarning($"Intento de agregar serie con datos invalidos: {nuevaSerie.Titulo}, se ha encontrado los siguientes errores:" +
                $"{string.Join(",", errores)}");

            return Result.Failure<Serie>(errores.ToImmutableArray());
        }

        return Result.Success(nuevaSerie);

    }

    public Result<Serie> GetSerieById(string id) => _dependencies.GetSerieById(id);
}