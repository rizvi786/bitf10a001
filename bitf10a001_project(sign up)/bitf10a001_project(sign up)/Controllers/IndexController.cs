using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using PagedList;
using bitf10a001_project_sign_up_.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace bitf10a001_project_sign_up_.Controllers
{
    public class IndexController : Controller
    {
        IAccounts AccCreater;

        private accountsEntities db = new accountsEntities();
        private adsEntities db2 = new adsEntities();

        public IndexController(IAccounts Account)
        {
            AccCreater = Account;
        }

        #region Index
        // GET: /Index/
        [AllowAnonymous]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 12;
            int pageNumber = page;


            var ads = (from p in db2.allads select p).OrderByDescending(x => x.Id).ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
            {
                return View("adView", ads);
            }

            return View(ads);

        }


        // ajax call
        //public 

        //link to post Ads tab of postAd Page
        [Authorize]
        public ActionResult postAd()
        {
            //.html?tab3 & tab1
            string tab = "tab1";
            ViewBag.a = tab;

            return View("postAd", Allads());
        }

        [Authorize]
        //link to myAccount tab of postAd Page
        public ActionResult myAcc()
        {
            //.html?tab3 & tab1
            string tab = "tab3";
            ViewBag.a = tab;

            return View("postAd", Allads());
        }

        [Authorize]
        //link to myads tab of postAd Page
        public ActionResult myAds()
        {
            //.cshtml?tab2 & tab1
            string tab = "tab2";
            ViewBag.a = tab;

            return View("postAd", Allads());
        }

        #endregion

        #region Create Accounts, login and Sign up
        // create new account
        [AllowAnonymous]
        [HttpPost]
        public ActionResult save(acc acc)
        {

            if (ModelState.IsValid)
            {
                AccCreater.save(acc);
            }
            int pageSize = 12;
            int pageNumber = 1;


            var ads = (from p in db2.allads select p).OrderByDescending(x => x.Id).ToPagedList(pageNumber, pageSize);

            return View("index",ads);
        }

        // start session for a user
        [AllowAnonymous]
        [HttpPost]
        public ActionResult login(int page = 1)
        {
            int pageSize = 12;
            int pageNumber = page;


            var ads = (from p in db2.allads select p).OrderByDescending(x => x.Id).ToPagedList(pageNumber, pageSize);

            string username = Request["email"];
            string password = Request["pass"];


            if (WebSecurity.Login(username, password, false))
            {
                var obj = db.accs.First(user => user.UserName == username);


                Session["id"] = obj.Id;
                Session["name"] = obj.name;
                Session["email"] = obj.UserName;
                Session["cell"] = obj.mobile;

                Session.Timeout = 6400;

                return RedirectToAction("index", ads);
            }
            else
            {
                Session["id"] = null;
                Session["name"] = null;
                Session["email"] = null;
                Session["cell"] = null;
                ViewBag.status = "no";
                return View("index", ads);
            }
            // Lets first check if the Model is valid or not

            //using (accountsEntities6 entities = new accountsEntities6())
            //{


            //    bool userValid = entities.accs.Any(user => user.UserName == username && user.pass == password);

            //    // User found in the database
            //    if (userValid)
            //    {
            //        var obj = entities.accs.First(user => user.UserName == username && user.pass == password);


            //        Session["id"] = obj.Id;
            //        Session["name"] = obj.name;
            //        Session["email"] = obj.UserName;
            //        Session["cell"] = obj.mobile;
            //        Session.Timeout = 6400;

            //        if (Request.IsAjaxRequest())
            //        {
            //            return View("adView", ads);
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.status = "no";
            //    }

            //}

            //// If we got this far, something failed, redisplay form
            //return View("index", ads);
        }

        // kill session
        [Authorize]
        public ActionResult logout()
        {
            WebSecurity.Logout();
            Session["id"] = null;
            return Redirect("~/Index");
        }

        // check for availability of account
        [AllowAnonymous]
        public JsonResult emailChecker(String email)
        {
            acc checker = null;
            try
            {
                checker = db.accs.First(x => x.UserName == email);
            }
            catch
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
            if (checker != null)
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Ad Creation
        // create ads for a person
        [Authorize]
        [HttpPost]
        public ViewResult createAd()
        {

            var context = new adsEntities();
            allad ad = new allad();
            moredetail adDetail = new moredetail();
            DateTime dateTime = DateTime.UtcNow.Date;

            ad.emailacc = (string)Session["email"];
            ad.adtitle = Request["title"];
            ad.description = Request["detail"];
            ad.price = Request["price"];
            ad.catagory = Request["catagory"];
            ad.postas = Request["radio7"];
            ad.condtion = Request["condition"];
            ad.warranty = Request["warranty"];
            ad.adis = Request["radio1"];
            ad.email = Request["email"];
            ad.city = Request["city"];
            ad.area = Request["area"];
            ad.mobile1 = Request["mobilePrim"];
            ad.mobile2 = Request["mobileSec"];
            ad.date = dateTime.ToString("dd-MM-yyyy");
            ad.img1 = Request["imgshow1"];
            ad.img2 = Request["imgshow2"];
            ad.img3 = Request["imgshow3"];
            ad.img4 = Request["imgshow4"];
            ad.time = DateTime.Now.ToString("HH:mm:ss tt");
            ad.Name = Session["name"].ToString();

            if (Request["brand"] != null)
            {
                adDetail.subcatagory = Request["brand"];
                adDetail.model = Request["model"];
                adDetail.accesories = Request["radio3"];
                adDetail.postid = ad.Id;
                context.moredetails.Add(adDetail);
            }

            //for (int i = 0; i < Request.Files.Count; i++)
            //{
            //    HttpPostedFileBase file = Request.Files[i];
            //    file.SaveAs(Server.MapPath(@"~\Files\" + file.FileName));
            //}

            context.allads.Add(ad);

            context.SaveChanges();
            string tab = "tab1";
            ViewBag.a = tab;

            return View("postAd", Allads());
        }

        // all ads of currently loged in person
        [Authorize]
        public IEnumerable<allad> Allads()
        {
            String email = (string)Session["email"];
            var result = db2.allads
                .Where(x => x.emailacc == email);
            return result;
        }

        // image uploader
        [Authorize]
        [HttpPost]
        public ActionResult SaveFile()
        {

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                file.SaveAs(Server.MapPath(@"~\Files\" + file.FileName));

            }
            return View("postAd");
        }

        //get full details of ads currently logged in person
        [Authorize]
        public modelAds myAllAds(int id)
        {
            modelAds modelAds = new modelAds();

            allad allad = db2.allads.Find(id);
            moredetail moredetail = null;
            try
            {
                moredetail = db2.moredetails.First(x => x.postid == id);
            }
            catch { }
            modelAds.Obj1 = allad;
            modelAds.Obj2 = moredetail;
            if (allad == null)
            {
                return null;
            }

            return modelAds;

        }


        //Ad Details
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            modelAds modelAds = myAllAds(id);

            if (modelAds == null)
            {
                return HttpNotFound();
            }

            return View("Details", modelAds);
        }
        #endregion

        #region Accounts Managment
        //Edit ad
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            allad allad = db2.allads.Find(id);
            if (allad == null)
            {
                return HttpNotFound();
            }
            return View("Edit", allad);
        }

        // POST: /login/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(allad allad)
        {
            if (ModelState.IsValid)
            {
                db2.Entry(allad).State = EntityState.Modified;
                db2.SaveChanges();

                string tab = "tab2";

                ViewBag.a = tab;
                return View("postAd", Allads());
            }
            return View(allad);
        }

        // change name or phone
        [Authorize]
        [HttpPost]
        public ActionResult editAcc()
        {
            string email = Session["email"].ToString();
            acc edit = db.accs.First(x => x.UserName == email);
            if (Request["name"] != null)
            {
                edit.name = Request["name"];
                Session["name"] = Request["name"];
            }
            if (Request["mobile"] != null)
            {
                edit.mobile = Request["mobile"];
                Session["cell"] = Request["mobile"];
            }

            db.SaveChanges();

            string tab = "tab3";
            ViewBag.a = tab;

            return View("postAd");
        }


        //change passward
        [HttpPost]
        public ActionResult changePass()
        {
            String Old = Request["OldPassword"];
            String New = Request["NewPassword"];

            bool changePasswordSucceeded;
            try
            {
                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, Old, New);
            }
            catch (Exception)
            {
                changePasswordSucceeded = false;
            }

            if (changePasswordSucceeded)
            {
                ViewBag.error = "yes";
            }
            else
            {
                ViewBag.error = "no";
            }

            string tab = "tab3";
            ViewBag.a = tab;

            return View("postAd", Allads());
        }

        // get ad by id for deletion
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            allad allad = db2.allads.Find(id);
            if (allad == null)
            {
                return HttpNotFound();
            }
            return View(allad);
        }

        
        //Delete ad
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            allad allad = db2.allads.Find(id);
            moredetail moredetail = db2.moredetails.First(x => x.postid == id);

            db2.moredetails.Remove(moredetail);
            db2.allads.Remove(allad);

            db2.SaveChanges();

            string tab = "tab2";

            ViewBag.a = tab;
            ViewBag.email = Session["email"].ToString();
            ViewBag.name = Session["name"].ToString();
            return View("postAd", Allads());
        }


        [HttpGet]
        public ActionResult AdminPanel()
        {
            return View("AdminPanel");
        }


        [HttpPost]
        public ActionResult Role(string RoleName)
        {

            Roles.CreateRole(RoleName);

            return View();
        }

        [HttpPost]
        public ActionResult UserRole(string RoleName, string UserName)
        {

            Roles.AddUserToRole(UserName, RoleName);

            return View("Role");
        }

        #endregion



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            db2.Dispose();
            base.Dispose(disposing);
        }

    }
}
