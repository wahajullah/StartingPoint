﻿using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.CompanyInfoViewModel;
using StartingPoint.Pages;
using StartingPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace InvenToryPlus.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CompanyInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public CompanyInfoController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = MainMenu.CompanyInfo.RoleName)]
        public async Task<IActionResult> Index()
        {
            CompanyInfoCRUDViewModel vm = await _context.CompanyInfo.FirstOrDefaultAsync(m => m.Id == 1);
            return View(vm);
        }


        public async Task<IActionResult> Edit()
        {
            CompanyInfoCRUDViewModel vm = await _context.CompanyInfo.FirstOrDefaultAsync(m => m.Id == 1);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyInfoCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        CompanyInfo _CompanyInfo = new CompanyInfo();
                        _CompanyInfo = await _context.CompanyInfo.FindAsync(vm.Id);
                        if (vm.CompanyLogo != null)
                            vm.Logo = "/upload/" + _iCommon.UploadedFile(vm.CompanyLogo);
                        vm.ModifiedDate = DateTime.Now;
                        vm.ModifiedBy = HttpContext.User.Identity.Name;
                        _context.Entry(_CompanyInfo).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();
                        TempData["successAlert"] = "Company Info Updated Successfully. Company Name: " + _CompanyInfo.Name;
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["errorAlert"] = "Operation failed.";
                    return View("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!IsExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            return View(vm);
        }

        private bool IsExists(long id)
        {
            return _context.CompanyInfo.Any(e => e.Id == id);
        }
    }
}

