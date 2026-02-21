using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission6Assignment.Models;

namespace Mission6Assignment.Controllers;

public class HomeController : Controller
{
    private MovieDbContext _context;
    public HomeController(MovieDbContext temp) //constructor
    {
        _context = temp;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GetToknowJoel()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult AddMovie()
    {
        ViewBag.Categories = _context.Categories
            .OrderBy(x => x.CategoryName)
            .ToList();
        
        return View();
    }

    [HttpPost]
    public IActionResult AddMovie(AddMovie response)
    {
        if (ModelState.IsValid)
        {
            _context.Movies.Add(response); // add the new movie to the database context
            _context.SaveChanges(); // save the changes to the database
            return View("Confirmation", response);
        }
        else // invalid data, return to form
        {
            ViewBag.Categories = _context.Categories
                .OrderBy(x => x.CategoryName)
                .ToList();
            
            return View(response);
        }
    }
}