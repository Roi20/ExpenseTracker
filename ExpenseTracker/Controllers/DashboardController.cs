﻿using ExpenseTracker.Common;
using ExpenseTracker.Contracts;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : BaseController
    {

        private readonly IDashboardRepository _repo;
        private readonly UserManager<AppIdentityUser> _userManager;

        public DashboardController(IDashboardRepository repo, UserManager<AppIdentityUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
           
        }


        public async Task<IActionResult> Index(int dayRange = 7)
        {

            var currentUserId = GetUserId();

            try
            {

                var StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-dayRange));
                var EndDate = DateOnly.FromDateTime(DateTime.Today);

                var model = new DashboardViewModel
                {
                    TotalIncome = await _repo.TotalIncome(StartDate, EndDate, currentUserId),
                    TotalExpense = await _repo.TotalExpense(StartDate, EndDate, currentUserId),
                    Balance = await _repo.Balance(StartDate, EndDate, currentUserId),
                    Transactions = await _repo.GetAllTransaction(currentUserId), 
                    User = await _repo.GetUserInfo(currentUserId)
                };
            
                var LineChartData = await _repo.GetLineChartData(StartDate, EndDate,  dayRange + 1, currentUserId);

                var data = await _repo.DoughnutChartData(StartDate, EndDate, currentUserId);

                ViewBag.LineChart = Newtonsoft.Json.JsonConvert.SerializeObject(LineChartData);
                ViewBag.DoughnutChart = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                ViewBag.DayRange = dayRange;
                ViewBag.User = await _repo.GetUserInfo(currentUserId);
                return View(model);

            }
            catch(Exception ex)
            {

                return View("Error", new ErrorViewModel { Message = ex.Message });

            }

       
        }






    }
}
