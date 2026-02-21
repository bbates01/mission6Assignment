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
        
        return View(new AddMovie());
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
            // fetch the model state errors and log them to the console for debugging purposes
            var failures = ModelState
                .Where(kvp => kvp.Value.Errors.Count > 0)
                .Select(kvp => new
                {
                    Field = kvp.Key,
                    AttemptedValue = kvp.Value.AttemptedValue,
                    Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                })
                .ToList();
            foreach (var f in failures)
            {
                System.Console.WriteLine($"ModelState error on '{f.Field}': attempted value='{f.AttemptedValue}' errors=[{string.Join("; ", f.Errors)}]");
            }
            
            ViewBag.Categories = _context.Categories
                .OrderBy(x => x.CategoryName)
                .ToList();
            
            return View(response);
        }
    }
    
    public IActionResult MovieList()
    {
        
        var movies = _context.Movies
            .Include(x => x.Category)
            .OrderBy(x => x.Title)
            .ToList();
        
        return View(movies);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var recordToEdit = _context.Movies
            .Single(x => x.MovieId == id);

        ViewBag.Categories = _context.Categories
            .OrderBy(c => c.CategoryName)
            .ToList();
        
        return View("AddMovie", recordToEdit);
    }

    [HttpPost]
    public IActionResult Edit(AddMovie updatedRecord)
    {
        if (ModelState.IsValid)
        {
            _context.Movies.Update(updatedRecord);
            _context.SaveChanges();
            return RedirectToAction("MovieList");
        }
        else
        {
            ViewBag.Categories = _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToList();
            
            return View("AddMovie", updatedRecord);
        }
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var recordToDelete = _context.Movies
            .Single(x => x.MovieId == id);
        
        return View(recordToDelete);
    }

    [HttpPost]
    public IActionResult Delete(AddMovie recordToDelete)
    {
        _context.Movies.Remove(recordToDelete);
        _context.SaveChanges();
        
        return RedirectToAction("MovieList");
    }
}