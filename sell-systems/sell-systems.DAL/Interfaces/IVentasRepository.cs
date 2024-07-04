using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sell_systems.Entity;
using sell_systems.Entity.Models;

namespace sell_systems.DAL.Interfaces
{
    public interface IVentasRepository : IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta entidad);
        Task<List<Detalleventa>> Reporte(DateTime fechaInicio, DateTime fechaDefIN);


    }
}
