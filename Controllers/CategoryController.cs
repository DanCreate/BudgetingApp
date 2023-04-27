using AutoMapper;
using BudgetingApp.Models;
using BudgetingApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetingApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IServiceUsers serviceUsers;

        public CategoryController(ICategoryRepository categoryRepository, IServiceUsers serviceUsers)
        {
            this.categoryRepository = categoryRepository;
            this.serviceUsers = serviceUsers;
        }

        public async Task<IActionResult> Index()
        {
            var userid = serviceUsers.GetUserID();
            var category = await categoryRepository.GetCategory(userid);

      

            return View(category);
        }
        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            

            if (!ModelState.IsValid)
            {

                return View(category);

            }
            var userid = serviceUsers.GetUserID();
            category.UserID = userid;

            await categoryRepository.Create(category);

         
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userid = serviceUsers.GetUserID();

            var category = await categoryRepository.GetViaId(id, userid);

            if (category is null)
            {

                return RedirectToAction("Not_found", "Home");
            }


            return View(category);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {

                return View(category);

            }

            var userid = serviceUsers.GetUserID();

            var categoryedit = await categoryRepository.GetViaId(category.Id, userid);

            if (categoryedit is null)
            {

                return RedirectToAction("Not_found", "Home");

            }
            category.UserID = userid;

            await categoryRepository.Update(category);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var userId = serviceUsers.GetUserID();
            var category = await categoryRepository.GetViaId(Id, userId);
            if (category is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var userId = serviceUsers.GetUserID();
            var category = await categoryRepository.GetViaId(Id, userId);
            if (category is null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            await categoryRepository.Delete(Id);

            return RedirectToAction("Index");
        }
    }

    }
