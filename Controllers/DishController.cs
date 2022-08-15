// Using statements
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;
namespace CRUDelicious.Controllers;

public class DishController : Controller
{
    private CRUDeliciousContext _context;
    // here we can "inject" our context service into the constructor
    public DishController(CRUDeliciousContext context)
    {
        _context = context;
    }


    [HttpGet("/")]
    [HttpGet("/dishes/all")]
    public IActionResult All()
    {
        List<Dish> AllDishes = _context.Dishes.ToList();

        return View("All", AllDishes);
    }


    [HttpGet("/dishes/new")]
    public IActionResult New()
    {
        return View("New");
    }


    [HttpPost("/dishes/create")]
    public IActionResult Create(Dish newDish) 
    {
        if(ModelState.IsValid == false)
        {
            return New();
        }
        _context.Dishes.Add(newDish);
        _context.SaveChanges();

        return RedirectToAction("All");
    }


    [HttpGet("/dishes/{DishId}")]
    public IActionResult ViewDish(int DishId)
    {
        Dish? dish = _context.Dishes.FirstOrDefault(dish => dish.DishId == DishId);
        if(dish == null)
        {
            return RedirectToAction("All");
        }
        return View("ViewDish", dish);
    }


    [HttpPost("/dishes/{deletedDishId}")]
    public IActionResult Delete(int deletedDishId)
    {
        Dish? dishToBeDeleted = _context.Dishes.FirstOrDefault(dish => dish.DishId == deletedDishId);
    if (dishToBeDeleted != null)
    {
        _context.Dishes.Remove(dishToBeDeleted);
        _context.SaveChanges();
    }
    return RedirectToAction("All");
    }


    [HttpGet("dishes/{dishToBeEdited}/edit")]
    public IActionResult EditDish(int dishToBeEdited)
    {
        Dish? dish = _context.Dishes.FirstOrDefault(dish => dish.DishId == dishToBeEdited);
    if (dish == null)
    {
        return RedirectToAction("All");
    }
    return View("Edit", dish);
    }


    [HttpPost("dishes/{updatedDishId}/update")]
    public IActionResult UpdateDish(int updatedDishId, Dish updatedDish)
    {
        if(ModelState.IsValid == false)
        {
            return EditDish(updatedDishId);
        }
        Dish? dbDish = _context.Dishes.FirstOrDefault(dish => dish.DishId == updatedDishId);
        if (dbDish == null)
        {
            return RedirectToAction("All");
        }

        dbDish.Name = updatedDish.Name;
        dbDish.Chef = updatedDish.Chef;
        dbDish.Calories = updatedDish.Calories;
        dbDish.Tastiness = updatedDish.Tastiness;
        dbDish.Description = updatedDish.Description;
        dbDish.UpdatedAt = DateTime.Now;

        _context.Dishes.Update(dbDish);
        _context.SaveChanges();

        return RedirectToAction("ViewDish", updatedDish);
    }
}