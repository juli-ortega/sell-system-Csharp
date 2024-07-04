using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sell_systems.DAL.DBContext;
using sell_systems.DAL.Interfaces;
using sell_systems.Entity;
using sell_systems.Entity.Models;

namespace sell_systems.DAL.Implementacion
{
    public class VentaRepository : GenericRepository<Venta>, IVentasRepository
    {

        private readonly DbventaContext _dbContext;

        public VentaRepository(DbventaContext dBContext) : base(dBContext)
        {
            _dbContext = dBContext;

        }

        public async Task<Venta> Registrar(Venta entidad)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = _dbContext.Database.BeginTransaction()) {

                try
                {
                    foreach (Detalleventa dv in entidad.Detalleventa)
                    {
                        Producto productoEncontrado = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();

                        productoEncontrado.Stock = productoEncontrado.Stock - dv.Cantidad;

                        _dbContext.Productos.Update(productoEncontrado);
                    }
                    await _dbContext.SaveChangesAsync();

                    Numerocorrelativo correlativo = _dbContext.Numerocorrelativos.Where(n => n.Gestion == "venta").First();

                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaActualizacion = DateTime.Now;

                    _dbContext.Numerocorrelativos.Update(correlativo);
                    await _dbContext.SaveChangesAsync();

                    string ceros = string.Concat(Enumerable.Repeat("0", correlativo.CantidadDigitos.Value));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - correlativo.CantidadDigitos.Value, correlativo.CantidadDigitos.Value);
                    entidad.NumeroVenta = numeroVenta;

                    await _dbContext.Venta.AddAsync(entidad);
                    await _dbContext.SaveChangesAsync();

                    ventaGenerada = entidad;
                    transaction.Commit();

                }
                catch (Exception ex) { 
                    transaction.Rollback();
                    throw ex;
                }

                
            
            }  
            return ventaGenerada;

        }

        public async Task<List<Detalleventa>> Reporte(DateTime fechaInicio, DateTime fechaDefIN)
        {
            List<Detalleventa> listaResumen = await _dbContext.Detalleventa
                .Include(v => v.IdVentaNavigation)
                .ThenInclude(u => u.IdUsuarioNavigation)
                .Include(v => v.IdVentaNavigation)
                .ThenInclude(tdv => tdv.IdTipoDocumentoVentaNavigation)
                .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= fechaInicio.Date &&
                    dv.IdVentaNavigation.FechaRegistro.Value.Date <= fechaDefIN.Date).ToListAsync();

            return listaResumen;

                
        }
    }
        
}

