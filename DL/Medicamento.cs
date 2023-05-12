using System;
using System.Collections.Generic;

namespace DL;

public partial class Medicamento
{
    public int IdMedicamento { get; set; }

    public decimal? Precio { get; set; }

    public string? Nombre { get; set; }

    public string? Imagen { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
