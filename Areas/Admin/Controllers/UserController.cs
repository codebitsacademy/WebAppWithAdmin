﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppWithAdmin.Areas.Admin.Models;
using WebAppWithAdmin.Areas.Admin.Repository;
using WebAppWithAdmin.Areas.Admin.ViewModels;

namespace WebAppWithAdmin.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class UserController : Controller
	{
		private IUserRepository userRepository;
		public UserController(IUserRepository userRepository)
		{
			this.userRepository = userRepository;
		}
		[HttpGet]
		public IActionResult Index()
		{
			var users = userRepository.GetAllUsers();

			var viewModel = new UserIndexViewModel { ApplicationUsers = users }; 

			return View(viewModel);
		}

		[HttpGet]
		public IActionResult Add()
		{
			var viewModel = new UserAddViewModel();
			return View(viewModel);
		}

		[HttpPost]
		public IActionResult Add(UserAddViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					FirstName = viewModel.ApplicationUser.FirstName,
					LastName = viewModel.ApplicationUser.LastName,
					Email = viewModel.ApplicationUser.Email,
					Role = viewModel.ApplicationUser.Role
				};

				var result = userRepository.AddUser(user);

				if (result == true)
					viewModel.StatusMessage = "New user added successfully!";
				else
					viewModel.StatusMessage = "This email has already been used!";

				return View(viewModel);
			}
			return View(viewModel);
		}

		[HttpGet]
		public IActionResult delete(int Id)
		{
			if (Id == 0)
				return NotFound();

			var user = userRepository.GetUser(Id);

			if (user == null)
				return NotFound();

			var viewModel = new UserDeleteViewModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Id = user.Id
			};

			return View(viewModel);
		}

		[HttpPost]
		[ActionName("delete")]
		public IActionResult ConfirmDelete(int Id)
		{
			userRepository.RemoveUser(Id);
			return RedirectToAction("Index");
		}
	}
}
