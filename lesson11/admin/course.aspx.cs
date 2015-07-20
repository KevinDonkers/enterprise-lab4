using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//reference models
using lesson11.Models;
using System.Web.ModelBinding;

namespace lesson11
{
    public partial class course : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //check for the id in url
            if (!IsPostBack)
            {
                GetDepartments();

                if (!String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                {
                    //we have a parameter populate the form
                    GetCourse();
                    GetStudents();
                }
            }
        }

        protected void GetDepartments()
        {
            try
            {
                //connect
                using (DefaultConnectionEF conn = new DefaultConnectionEF())
                {
                    //get the student id
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                    //get student info
                    var d = (from dep in conn.Departments
                             orderby dep.Name
                             select dep);

                    //populate the form
                    ddlDepartment.DataSource = d.ToList();
                    ddlDepartment.DataBind();

                }
            }
            catch (Exception e)
            {
                Response.Redirect("~/error.aspx");
            }
        }

        protected void GetStudents()
        {
            try
            {
                using (DefaultConnectionEF conn = new DefaultConnectionEF())
                {
                    //Get courseID 
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                    //fill students grid
                    var stu = (from s in conn.Students
                               join en in conn.Enrollments on s.StudentID equals en.StudentID
                               where en.CourseID == CourseID
                               orderby s.LastName
                               select new { en.EnrollmentID, s.FirstMidName, s.LastName, s.StudentID, s.EnrollmentDate }).Distinct();

                    //populate the form
                    grdStudents.DataSource = stu.ToList();
                    grdStudents.DataBind();

                    //fill students dropdown
                    var allstu = (from s in conn.Students
                                  join en in conn.Enrollments on s.StudentID equals en.StudentID
                                  orderby s.LastName
                                  select new { en.EnrollmentID, s.FirstMidName, s.LastName, s.StudentID, s.EnrollmentDate, DisplayField = (s.LastName + ", " + s.FirstMidName) }).Distinct();

                    ddlStudent.DataTextField = "DisplayField";

                    //populate the form
                    ddlStudent.DataSource = allstu.ToList();
                    ddlStudent.DataBind();

                    //add default options to the dropdown
                    ListItem newItem = new ListItem("-Select-", "0");
                    ddlStudent.Items.Insert(0, newItem);


                    pnlStudents.Visible = true;
                }
            }
            catch (Exception e)
            {
                Response.Redirect("~/error.aspx");
            }
        }

        protected void GetCourse()
        {
            try
            {
                //connect
                using (DefaultConnectionEF conn = new DefaultConnectionEF())
                {
                    //get the course id
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                    //get student info
                    var c = (from crs in conn.Courses
                             where crs.CourseID == CourseID
                             select crs).FirstOrDefault();

                    //populate the form
                    txtTitle.Text = c.Title;
                    txtCredits.Text = c.Credits.ToString();
                    ddlDepartment.SelectedValue = c.DepartmentID.ToString();
                }
            }
            catch (Exception e)
            {
                Response.Redirect("~/error.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //connect
                using (DefaultConnectionEF conn = new DefaultConnectionEF())
                {
                    //instantiate a new student object in memory
                    Course c = new Course();

                    //decide if updating or adding, then save
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                    {
                        Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                        c = (from crs in conn.Courses
                             where crs.CourseID == CourseID
                             select crs).FirstOrDefault();
                    }

                    //fill the properties of our object from the form inputs
                    c.Title = txtTitle.Text;
                    c.Credits = Convert.ToInt32(txtCredits.Text);
                    c.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);

                    if (Request.QueryString.Count == 0)
                    {
                        conn.Courses.Add(c);
                    }
                    conn.SaveChanges();

                    //redirect to updated departments page
                    Response.Redirect("Courses.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/error.aspx");
            }
        }

        protected void grdStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //get the selected enrollment id
            Int32 EnrollmentID = Convert.ToInt32(grdStudents.DataKeys[e.RowIndex].Values["EnrollmentID"]);

            try
            {
                using (DefaultConnectionEF conn = new DefaultConnectionEF())
                {

                    Enrollment objE = (from en in conn.Enrollments
                                       where en.EnrollmentID == EnrollmentID
                                       select en).FirstOrDefault();

                    conn.Enrollments.Remove(objE);
                    conn.SaveChanges();

                    GetStudents();

                }
            }
            catch (Exception exc)
            {
                Response.Redirect("~/error.aspx");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (DefaultConnectionEF conn = new DefaultConnectionEF())
                {

                    Int32 StudentID = Convert.ToInt32(ddlStudent.SelectedValue);
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                    Enrollment objE = new Enrollment();

                    objE.StudentID = StudentID;
                    objE.CourseID = CourseID;

                    conn.Enrollments.Add(objE);
                    conn.SaveChanges();

                    //refresh
                    GetStudents();

                }
            }
            catch (Exception exception)
            {
                Response.Redirect("~/error.aspx");
            }
        }
    }
}