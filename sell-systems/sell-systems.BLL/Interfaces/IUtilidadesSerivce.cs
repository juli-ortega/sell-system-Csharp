using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sell_systems.BLL.Interfaces
{
    public interface IUtilidadesSerivce
    {
        string GenerarClave();
        string ConvertirSha256(string texto);


    }
}
