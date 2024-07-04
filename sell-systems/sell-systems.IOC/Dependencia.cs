using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sell_systems.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using sell_systems.DAL.Interfaces;
using sell_systems.DAL.Implementacion;
using sell_systems.BLL.Interfaces;
using sell_systems.BLL.Implementacion;

namespace sell_systems.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbventaContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("CadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IVentasRepository, VentaRepository>();

            services.AddScoped<ICorreoService, CorreoService>();

            services.AddScoped<IFireBaseService, FireBaseService>();
            services.AddScoped<IUtilidadesSerivce, UtilidadesService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();


        }

    }
}
