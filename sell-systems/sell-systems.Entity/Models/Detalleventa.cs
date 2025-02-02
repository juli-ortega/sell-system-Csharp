﻿using System;
using System.Collections.Generic;

namespace sell_systems.Entity.Models;

public partial class Detalleventa
{
    public int IdDetalleVenta { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public string? MarcaProducto { get; set; }

    public string? DescripcionProducto { get; set; }

    public string? CategoriaProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public decimal? Total { get; set; } 

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
