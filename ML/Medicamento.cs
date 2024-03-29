﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Medicamento
    {
        public int IdMedicamento { get; set; }
        public decimal Precio { get; set; }
        public string? Nombre { get; set; }
        public string? Imagen { get; set; }
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int Stock { get; set; }
        public decimal SubTotal { get; set; }
        public List<object>? Medicamentos { get; set; }

        public Medicamento()
        {
            Stock = 20;
        }
    }
}
