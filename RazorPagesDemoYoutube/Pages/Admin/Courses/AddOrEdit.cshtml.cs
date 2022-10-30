using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using RazorPagesDemoYoutube.Data;
using RazorPagesDemoYoutube.Models;
using RazorPagesDemoYoutube.ViewModels;

namespace RazorPagesDemoYoutube.Pages.Admin.Courses
{
    public class AddOrEditModel : PageModel
    {

        [BindProperty]
        public CourseViewModel CourseViewModel { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> AuthorList { get; set; }
        private readonly CourseDbContext _db;
        private readonly IMapper _mapper;
        private readonly IToastNotification _notify;

        public AddOrEditModel(CourseDbContext db, IMapper mapper, IToastNotification notify)
        {
            this._db = db;
            this._mapper = mapper;
            this._notify = notify;
        }
        public void OnGet()
        {
            CategoryList = _db.Category.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.CategoryId.ToString()
            });

            AuthorList = _db.AppUser.Select(u => new SelectListItem
            {
                Text = u.FullName,
                Value = u.Id
            });
        }


        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                Course course = _mapper.Map<Course>(CourseViewModel);
                await _db.Course.AddAsync(course);
                bool res = await _db.SaveChangesAsync() > 0;
                if (res)
                {
                    _notify.AddSuccessToastMessage("Course created successfully");
                    return RedirectToPage("Index");
                }
                else
                {
                    _notify.AddSuccessToastMessage("Course not created !!!");
                }
            }
            return Page();
        }
    }
}
