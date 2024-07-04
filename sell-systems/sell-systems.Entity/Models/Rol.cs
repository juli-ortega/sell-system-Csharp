using System;
using System.Collections.Generic;

namespace sell_systems.Entity.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string? Descripcion { get; set; }

    public ulong? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Rolmenu> Rolmenus { get; set; } = new List<Rolmenu>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
