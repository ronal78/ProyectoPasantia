using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Dependencias
{
    internal interface ISeriesDependencias
    {
        public Result<List<serie>> GetSeries();
    }
}
