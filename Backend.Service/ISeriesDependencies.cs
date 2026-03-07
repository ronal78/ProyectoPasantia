using PasantiaTI1.Models;
using ROP;

namespace Backend.Service;

public interface ISeriesDependencies
{
    public Result<List<Serie>> GetSeries();

    public Result<bool> AddSerie(Serie Nuevaserie);

    public Result<bool> UpdateSerie(int id, Serie serieActualizada);

    public Result<bool> DeleteSerie(int id);
}

public class SeriesService
{
    private readonly ISeriesDependencies _dependencies;

    public SeriesService(ISeriesDependencies dependencies)
    {
        _dependencies = dependencies;
    }

    public Result<List<Serie>> GetSeries() => _dependencies.GetSeries();

    public Result<bool> DeleteSerie(int id) => _dependencies.DeleteSerie(id);

    public Result<bool> AddSerie(Serie nuevaSerie) => _dependencies.AddSerie(nuevaSerie);

    public Result<bool> UpdateSerie(int id, Serie serieActualizada) => _dependencies.UpdateSerie(id
    , serieActualizada);

    
}