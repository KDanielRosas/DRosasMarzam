using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            int codeResult = BL.Usuario.Login(userName, password);

            if (codeResult == 1)
            {
                return RedirectToAction("Index", "Medicamento");
            }
            else
            {
                if (codeResult == 2)
                {
                    ViewBag.Message = "El UserName no existe, verifique la información.";
                }

                if (codeResult == 3)
                {
                    ViewBag.Message = "El Password es incorrecto.";

                }
                return PartialView("Modal");
            }          
        }//Login(post)
    }
}
