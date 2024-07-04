using System;
using System.Collections.Generic;

namespace sell_systems.Entity.Models;

public partial class Numerocorrelativo
{
    public int IdNumeroCorrelativo { get; set; }

    public int? UltimoNumero { get; set; }

    public int? CantidadDigitos { get; set; }

    public string? Gestion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
