using System.Globalization;
using AutoMapper;
using sell_systems.Entity.Models;
using Sell_System.AppWeb.Models.ViewModels;
using System;

namespace Sell_System.AppWeb.Utilidades.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, VMRol>().ReverseMap();
            #endregion Rol

            #region Usuario
            CreateMap<Usuario, VMUsuario>()
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0)
                )
                .ForMember(destino => destino.NombreRol,
                     opt => opt.MapFrom(origen => origen.IdRolNavigation.Descripcion)
                );

            CreateMap<VMUsuario, Usuario>()
                .ForMember(destino =>
                    destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo ==  1 ? true: false)
                )
                .ForMember(destino => destino.IdRolNavigation,
                     opt => opt.Ignore()
                );

            #endregion Usuario

            #region Negocio
            CreateMap<Negocio, VMNegocio>()
                .ForMember(destino => destino.PorcentajeImpuesto,
                    opt => opt.MapFrom(origen => origen.PorcentajeImpuesto.HasValue
                    ? origen.PorcentajeImpuesto.Value.ToString(new CultureInfo("es-ARG"))
                    : string.Empty)
                    );

            CreateMap<VMNegocio, Negocio>()
            .ForMember(destino => destino.PorcentajeImpuesto,
                opt => opt.MapFrom(origen =>
                    !string.IsNullOrEmpty(origen.PorcentajeImpuesto)
                        ? Convert.ToDecimal(origen.PorcentajeImpuesto, new CultureInfo("es-ARG"))
                        : (decimal?)null));
            #endregion Negocio

            #region Categoria
            CreateMap<Categoria, VMCategoria>()
                .ForMember(destino => destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo == true ? 1:0)
                    );

            CreateMap<VMCategoria, Categoria>()
                 .ForMember(destino => destino.EsActivo,
                     opt => opt.MapFrom(origen => origen.EsActivo == 1? true:false)
                     );
            #endregion Categoria

            #region Producto
            CreateMap<Producto, VMProducto>()
                .ForMember(destino => destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0)
                    )
                .ForMember(destino => destino.nombreCategoria,
                    opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Descripcion)
                    )
                .ForMember(destino => destino.Precio,
                    opt => opt.MapFrom(origen => origen.Precio.Value.ToString(new CultureInfo("es-ARG")))
                    );
            
            CreateMap<VMProducto, Producto>()
                .ForMember(destino => destino.EsActivo,
                    opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true:false)
                    )
                .ForMember(destino => destino.IdCategoriaNavigation,
                    opt => opt.Ignore()
                    )
                .ForMember(destino => destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-ARG")))
                    );


            #endregion Producto

            #region TipoDocumentoVenta
            CreateMap<Tipodocumentoventa, VMTipoDocumentoVenta>().ReverseMap();
            #endregion TipoDocumentoVenta

            #region Venta
            CreateMap<Venta, VMVenta>()
                .ForMember(destino => destino.tipoDocumentoVenta,
                    opt => opt.MapFrom(origen => origen.IdTipoDocumentoVentaNavigation.Descripcion)
                    )
                .ForMember(destino => destino.Usuario,
                    opt => opt.MapFrom(origen => origen.IdUsuarioNavigation.Nombre)
                    )
                .ForMember(destino => destino.SubTotal,
                    opt => opt.MapFrom(origen => origen.SubTotal.Value.ToString(new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.ImpuestoTotal,
                    opt => opt.MapFrom(origen => origen.ImpuestoTotal.Value.ToString(new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.Total,
                    opt => opt.MapFrom(origen => origen.Total.Value.ToString(new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.FechaRegistro,
                    opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                    );

            CreateMap<VMVenta, Venta>()
                .ForMember(destino => destino.SubTotal,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.SubTotal, new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.ImpuestoTotal,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.ImpuestoTotal, new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Total, new CultureInfo("es-ARG")))
                    );

            #endregion Venta

            #region DetalleVenta
            CreateMap<Detalleventa, VMDetalleVenta>()
                .ForMember(destino => destino.Precio,
                    opt => opt.MapFrom(origen => origen.Precio.Value.ToString(new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.Total,
                    opt => opt.MapFrom(origen => origen.Total.Value.ToString(new CultureInfo("es-ARG")))
                    );

            CreateMap<VMDetalleVenta, Detalleventa>()
                .ForMember(destino => destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.Total,
                    opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Total, new CultureInfo("es-ARG")))
                    );

            CreateMap<Detalleventa, VMReporteVenta>()
                .ForMember(destino => destino.fechaRegistro,
                    opt => opt.MapFrom(origen => origen.IdVenta.Value.ToString("dd/MM/yyyy"))
                    )
                .ForMember(destino => destino.numenroVenta,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.NumeroVenta)
                    )
                .ForMember(destino => destino.tipoDocumento,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.IdTipoDocumentoVentaNavigation.Descripcion)
                    )
                .ForMember(destino => destino.documentoCliente,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.DocumentoCliente)
                    )
                .ForMember(destino => destino.nombreCliente,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.NombreCliente)
                    )
                .ForMember(destino => destino.subTotalVenta,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.SubTotal.Value.ToString(new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.impuestoTotalVenta,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.ImpuestoTotal.Value.ToString(new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.totalVenta,
                    opt => opt.MapFrom(origen => origen.IdVentaNavigation.Total.Value.ToString(new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.producto,
                    opt => opt.MapFrom(origen => origen.DescripcionProducto)
                    )
                .ForMember(destino => destino.precio,
                    opt => opt.MapFrom(origen => origen.Precio.Value.ToString(new CultureInfo("es-ARG")))
                    )
                .ForMember(destino => destino.totalVenta,
                    opt => opt.MapFrom(origen => origen.Total.Value.ToString(new CultureInfo("es-ARG")))
                    );

            #endregion Venta

            #region Menu
            CreateMap<Menu, VMMenu>()
                .ForMember(destino => destino.subMenus,
                    opt => opt.MapFrom(origen => origen.InverseIdMenuPadreNavigation)
                    );
            #endregion Menu

        }
    }
}
