using Microsoft.AspNetCore.Mvc;
using ML;

namespace PL.Controllers
{
    public class MedicamentoController : Controller
    {
        public ActionResult Index()
        {         
            return View(BL.Medicamento.GetAll());
        }

        [HttpGet]
        public ActionResult Form(int? idMedicamento)
        {
            ML.Medicamento medicamento = new();
            if (idMedicamento == null)
            {  
                //Form Vacio
                return View(medicamento);
            }
            else
            {
                //Form Update                
                return View(BL.Medicamento.GetById(idMedicamento.Value));
            }
        }

        [HttpPost]
        public ActionResult Form(Medicamento medicamento)
        {
            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] imagen = ImagenABase64(file);
                medicamento.Imagen = Convert.ToBase64String(imagen);
            }

            if (medicamento.IdMedicamento == 0)
            {
                //Add
                if (BL.Medicamento.Add(medicamento))
                {
                    ViewBag.Message = "Registro almacenado correctamente.";
                }
                else
                {
                    ViewBag.Message = "Error al guardar el registro.";
                }                
                return PartialView("Modal");
            }
            else
            {
                //Update
                if (BL.Medicamento.Update(medicamento))
                {
                    ViewBag.Message = "Registro actualizado correctamente.";
                }
                else
                {
                    ViewBag.Message = "Error al editar el registro.";
                }
                return PartialView("Modal");
            }
        }//Form POST
       
        public ActionResult Delete(int idMedicamento)
        {
            if (BL.Medicamento.Delete(idMedicamento))
            {
                ViewBag.Message = "Registro eliminado correctamente";
            }
            else
            {
                ViewBag.Message = "Error al eliminar el registro.";
            }
            return PartialView("Modal");
        }
        public byte[] ImagenABase64(IFormFile foto)
        {
            using var fileStream = foto.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }//imagenABase64

        [HttpPost]
        public JsonResult CartAdd(int idMedicamento)
        {
            bool existe = false;
            Carrito carrito = new Carrito();
            carrito.Medicamentos = new List<object>();

            ML.Medicamento medicamento = BL.Medicamento.GetById(idMedicamento);

            if (HttpContext.Session.GetString("Medicamento") == null)
            {
                medicamento.Cantidad = 1;
                medicamento.SubTotal = medicamento.Precio * medicamento.Cantidad;
                carrito.Medicamentos.Add(medicamento);
                HttpContext.Session.SetString("Medicamento", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Medicamentos));
                HttpContext.Session.GetString("Medicamento");                
            }
            else
            {
                GetCarrito(carrito);
                foreach (Medicamento item in carrito.Medicamentos.Cast<Medicamento>())
                {
                    if (medicamento.IdMedicamento == item.IdMedicamento)
                    {
                        item.Cantidad++;
                        item.SubTotal  = item.Precio * item.Cantidad;
                        existe = true;
                        break;
                    }                    
                }//foreach

                if (!existe)
                {
                    medicamento.Cantidad = 1;
                    medicamento.SubTotal = medicamento.Precio * medicamento.Precio;
                    carrito.Medicamentos.Add(medicamento);
                }
                HttpContext.Session.SetString("Medicamento", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Medicamentos));
            }

            string result = HttpContext.Session.GetString("Medicamento") != null ?
                "OK" : "Error";

            return Json(result);
        }//CartPost

        public ActionResult ResumenCompra()
        {
            Carrito carrito = new();
            carrito.Medicamentos = new();
            if (HttpContext.Session.GetString("Medicamento") != null)
            {
                GetCarrito(carrito);
            }
            return View(carrito);
        }//ResumenCompra

        public Carrito GetCarrito(Carrito carrito)
        {
            var session = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Medicamento"));

            foreach (var item in session)
            {
                Medicamento obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(item.ToString());
                carrito.Medicamentos.Add(obj);
            }
            return carrito;
        }//GetCarrito

        [HttpPost]
        public JsonResult SumarCantidad(int idMedicamento)
        {
            Carrito carrito = new();
            carrito.Medicamentos = new();
            string response = "ERROR";
            GetCarrito(carrito);

            foreach (Medicamento item in carrito.Medicamentos.Cast<Medicamento>())
            {
                if (item.IdMedicamento == idMedicamento)
                {
                    item.Cantidad++;
                    response = "OK";
                    break;
                }
            }
            HttpContext.Session.SetString("Medicamento",Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Medicamentos));
            return Json(response);
        }//SumarCantidad

        [HttpPost]
        public JsonResult RestarCantidad(int idMedicamento)
        {
            Carrito carrito = new();
            carrito.Medicamentos = new();
            string response = "ERROR";
            GetCarrito(carrito);

            foreach (Medicamento item in carrito.Medicamentos.Cast<Medicamento>())
            {
                if (item.IdMedicamento == idMedicamento)
                {
                    --item.Cantidad;
                    response = "OK";
                    break;
                }
            }
            HttpContext.Session.SetString("Medicamento", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Medicamentos));
            return Json(response);
        }//RestarCantidad

        [HttpPost]
        public JsonResult EliminarCarrito(int idMedicamento)
        {
            string response = "ERROR";
            Carrito carrito = new();
            carrito.Medicamentos = new();
            GetCarrito(carrito);

            foreach (Medicamento item in carrito.Medicamentos)
            {
                if (item.IdMedicamento == idMedicamento)
                {
                    carrito.Medicamentos.RemoveAt(carrito.Medicamentos.IndexOf(item));
                    response = "OK";
                    break;
                }
            }

            HttpContext.Session.SetString("Medicamento", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Medicamentos));
            return Json(response);
        }//EliminarDelCarrito

        [HttpGet]
        public JsonResult GetCantidad(int idMedicamento)
        {
            Carrito carrito = new();
            carrito.Medicamentos = new();
            Medicamento medicamento = new();
            GetCarrito(carrito);

            foreach (Medicamento item in carrito.Medicamentos)
            {
                if (item.IdMedicamento == idMedicamento)
                {
                    medicamento = item;
                }
            }
            return Json(medicamento);
        }//GetCantidad
    }
}
