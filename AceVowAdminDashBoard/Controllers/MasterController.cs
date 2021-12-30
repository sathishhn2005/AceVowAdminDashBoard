using AceVowEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceVowBusinessLayer;
using System.Configuration;
using System.IO;
using System.Transactions;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;


namespace AceVowAdminDashBoard.Controllers
{
    public class MasterController : Controller
    {
        DataModel objBAL;
        // GET: Master
        public ActionResult CategoryMaster()
        {
            objBAL = new DataModel();
            objBAL.GetUserList(out List<ClientUser> lstusers);
            List<SelectListItem> userList = new List<SelectListItem>();
            foreach (var item in lstusers)
            {
                userList.Add(new SelectListItem { Text = item.UserName, Value = Convert.ToString(item.Id) });
            }

            ViewBag.lstUsers = userList;
            return View();
           
        }
        [HttpGet]
        public JsonResult GetCategory(int UserId)
        {
            objBAL = new DataModel();
            objBAL.GetCategoryList(out List<Category> lstCategory, UserId);
            return Json(new
            {
                lstCategory = lstCategory

            }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult AddUpdateProductMaster(Product objBM)
        {
            objBAL = new DataModel();
            //  objBAL.DMLProductMaster(out List<Category> lstCategory, UserId);
            string msg = string.Empty;
           
            string JParamVal = JsonConvert.SerializeObject(objBM);

            long res = objBAL.DMLProductMaster(JParamVal);
            if (res > 0)
                msg = "Product Saved successfully";
            else
                msg = "Product not Saved successfully";


            return Json(new
            {
                Status = msg

            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string FName = TempData["PMFolderName"].ToString();
            TempData["PMFolderName"] = FName;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        string pathString = GetFolderPath(FName);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);

                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
        private string GetFolderPath(string FName)
        {
            string FPath = ConfigurationManager.AppSettings["ProductImage"];
        //    var originalDirectory = new System.IO.DirectoryInfo(string.Format("{0}" + FPath, Server.MapPath(@"\")));
            string pathString = Path.Combine(FPath, FName);

            bool isExists = Directory.Exists(pathString);
            if (!isExists)
                Directory.CreateDirectory(pathString);

            return pathString;
        }
    }
}