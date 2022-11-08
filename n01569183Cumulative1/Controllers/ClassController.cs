using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using n01569183Cumulative1.Models;
namespace n01569183Cumulative1.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult List()
        {
            ClassDataController classController = new ClassDataController();
            IEnumerable<Class> Classes = classController.ListClasses();
            return View(Classes);
        }

        public ActionResult Show(int id)
        {
            ClassDataController classController = new ClassDataController();
            Class SelectedClass = classController.SelectClass(id);
            return View(SelectedClass);
        }
    }
}