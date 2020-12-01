using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using ClaimsZonderIdentity.Models;

namespace ClaimsZonderIdentity.Controllers
{

    //Ga eerst naar Home/Authenticate
    //Daarna naar Home/Secret Je krijgt dan de requested pagina met de claim te zien 
    //Zie commentaar in de Authenticate action en StartUp
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        [Authorize (Policy = "Regering")]
        public IActionResult Secret()
        {
            var c = User.Claims;
            
            return View(c);
        }
        public IActionResult Privacy()
        {
            
            return View();
        }

        public IActionResult Authenticate()
        {
            
            var rijbewijsClaims = new List<Claim>()
            {
                new Claim("rijbewijs","B-E")
            };

            var identityVoorRijbewijsClaims = new ClaimsIdentity(rijbewijsClaims, "RDW"); //RDW is de partij die we vertrouwen 
                                                                                          //en die zijn claims heeft over de gebruiker 
                                                                                          // waarmee ingelogd kan worden

            var userPrincipal = new ClaimsPrincipal(new[] { identityVoorRijbewijsClaims });

            //De userPrincipal (De claims die zeggen dat jij het bent) is bijv. hetgeen je terugkrijgt wanneer je met een social account inlogd.
            //Bijv. met een Google account. Je logt dan in met je Google credentials waarna Google een claim teruggeeft
            //Met de claim log je in en ben je geautoriseerd. 


            HttpContext.SignInAsync(userPrincipal); //<= Deze code is anders wanneer je met .NET Core Identity werkt

            // Je bent nu ingelogd. Nu kun je Home/Secret opvragen. Middels de [Authorize] boven de Secret action kan ASP.NET Core nu weten
            // dat je geautoriseerd bent en dus wordt je requested pagina getoond.

            //Wanneer je de naam van de claim veranderd zul je zien dat wanneer je /Home/Secret opvraagt na authenticate pagina, je niet 
            //wordt toegelaten tot de Secret pagina. Je vereiste claim wordt nl. niet meegegeven.

            //Kijk anders ook naar deze Blog: https://digitalmccullough.com/posts/aspnetcore-auth-system-demystified.html

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
