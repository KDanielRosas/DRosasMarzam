using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Medicamento
    {
        public static bool Add(ML.Medicamento medicamento)
        {
            using (DL.DrosasMarzamContext context = new())
            {
                int query = context.Database.ExecuteSqlRaw($"MedicamentoAdd {medicamento.Precio}," +
                    $"'{medicamento.Nombre}', '{medicamento.Imagen}', '{medicamento.Descripcion}'");

                return query > 0;
            }
        }//Add

        public static bool Update(ML.Medicamento medicamento)
        {
            using (DL.DrosasMarzamContext context = new())
            {
                int query = context.Database.ExecuteSqlRaw($"MedicamentoUpdate {medicamento.IdMedicamento}," +
                    $"{medicamento.Precio}, '{medicamento.Nombre}', '{medicamento.Imagen}', '{medicamento.Descripcion}'");

                return query > 0;
            }
        }//Update

        public static bool Delete(int idMedicamento)
        {
            using (DL.DrosasMarzamContext context = new())
            {
                int query = context.Database.ExecuteSqlRaw($"MedicamentoDelete {idMedicamento}");

                return query > 0;
            }
        }//Delete

        public static ML.Medicamento GetAll()
        {
            ML.Medicamento medicamento = new();
            using (DL.DrosasMarzamContext context = new())
            {
                var query = context.Medicamentos.FromSqlRaw("MedicamentoGetAll").ToList();

                if (query != null)
                {
                    medicamento.Medicamentos = new List<object>();
                    foreach (var item in query)
                    {
                        ML.Medicamento obj = new();
                        obj.IdMedicamento = item.IdMedicamento;
                        obj.Precio = item.Precio.Value;
                        obj.Nombre = item.Nombre;
                        obj.Imagen = item.Imagen;
                        obj.Descripcion = item.Descripcion;

                        medicamento.Medicamentos.Add(obj);
                    }
                }
            }
            return medicamento;
        }//GetAll

        public static ML.Medicamento GetById(int idMedicamento)
        {
            ML.Medicamento obj = new();
            using (DL.DrosasMarzamContext context = new())
            {
                var query = context.Medicamentos.FromSqlRaw($"MedicamentoGetById {idMedicamento}").AsEnumerable().FirstOrDefault();

                if (query != null)
                {                                     
                    obj.IdMedicamento = query.IdMedicamento;
                    obj.Precio = query.Precio.Value;
                    obj.Nombre = query.Nombre;
                    obj.Imagen = query.Imagen;
                    obj.Descripcion = query.Descripcion;
                }                
            }
            return obj;
        }//GetById      
    }//Class Medicamento
}//NS
