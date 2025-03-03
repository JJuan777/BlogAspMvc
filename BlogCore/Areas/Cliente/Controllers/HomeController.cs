using System.Diagnostics;
using System.Drawing.Printing;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {

        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        //Version sin paginación
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    HomeVM homeVM = new HomeVM()
        //    {
        //        Sliders = _contenedorTrabajo.Slider.GetAll(),
        //        ListArticulos = _contenedorTrabajo.Articulo.GetAll()
        //    };

        //    //saber si estamos en home
        //    ViewBag.IsHome = true;

        //    return View(homeVM);
        //}

        //Version con paginación

        [HttpGet]
        public IActionResult Index(int page = 1, int pageSise = 6)
        {
            var articulos = _contenedorTrabajo.Articulo.AsQueryable();

            //paginacion
            var paginatedEntries = articulos.ToList().Skip((page - 1) * pageSise).Take(pageSise);

            HomeVM homeVM = new HomeVM()
            {
                Sliders = _contenedorTrabajo.Slider.GetAll(),
                ListArticulos = paginatedEntries.ToList(),
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(articulos.Count() / (double)pageSise)
            };

            //saber si estamos en home
            ViewBag.IsHome = true;

            return View(homeVM);
        }


        //Buscador
        public IActionResult ResultadoBusqueda(string searchString, int page = 1, int pageSise = 6)
        {
            var articulos = _contenedorTrabajo.Articulo.AsQueryable();

            //metodo para buscador
            if (!string.IsNullOrEmpty(searchString))
            {
                articulos = articulos.Where(e => e.Nombre.Contains(searchString) || e.Descripcion.Contains(searchString));
            }

            //paginacion
            var paginatedEntries = articulos.ToList().Skip((page - 1) * pageSise).Take(pageSise);

            var model = new ListaPaginada<Articulo>(paginatedEntries.ToList(), articulos.Count(), page, pageSise, searchString);
 

            return View(model);
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var articuloDesdeBd = _contenedorTrabajo.Articulo.Get(id);
            if (articuloDesdeBd == null)
            {
                return NotFound();
            }
            return View(articuloDesdeBd);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
