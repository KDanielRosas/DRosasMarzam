using System;
using System.Collections.Generic;

namespace DL;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public int? IdMedicamento { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Total { get; set; }

    public virtual Medicamento? IdMedicamentoNavigation { get; set; }
}
