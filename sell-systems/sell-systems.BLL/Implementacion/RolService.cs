using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sell_systems.BLL.Interfaces;
using sell_systems.Entity.Models;
using sell_systems.DAL.Interfaces;
using sell_systems.Entity;
namespace sell_systems.BLL.Implementacion
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> _repository;

        public RolService(IGenericRepository<Rol> repository)
        {
            _repository = repository;
        }
        public async Task<List<Rol>> Lista()
        {
            IQueryable<Rol> query = await _repository.Consultar();

            return query.ToList();
        }
    }
}
