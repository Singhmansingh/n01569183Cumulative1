using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using n01569183Cumulative1.Models;
namespace n01569183Cumulative1.Controllers
{
    public class TeacherController : Controller
    {

        // GET: Teacher/List
        public ActionResult List()
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teacherList = controller.ListTeachers();
            return View(teacherList);
        }

        // GET: Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.SelectTeacher(id);

            return View(teacher);
        }
    }
}