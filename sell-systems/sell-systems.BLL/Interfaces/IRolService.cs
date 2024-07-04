using sell_systems.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sell_systems.BLL.Interfaces
{
    public interface IRolService
    {
        Task<List<Rol>> Lista();

    }
}
