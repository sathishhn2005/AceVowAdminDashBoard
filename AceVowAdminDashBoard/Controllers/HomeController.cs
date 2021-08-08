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
    public class HomeController : Controller
    {
        DataModel objBAL;
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
                return View();
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Login(Login objModels)
        {
            string UName = string.Empty;
            Session["UserName"] = null;
            int i = 0;
            string Pswd = string.Empty;
            UName = objModels.UserName;
            Pswd = objModels.Password;
            if (!string.IsNullOrEmpty(UName) && !string.IsNullOrEmpty(Pswd))
            {
                objBAL = new DataModel();
                i = objBAL.GetLoginInfo(UName, Pswd);
                if (i > 0)
                {
                    Session["UserName"] = UName;
                    return RedirectToAction("About", "Home");
                }
                else
                {
                    ViewBag.Message = "Username or Password not found.!";
                    return View();
                }
            }

            else
                return View();
        }
        public ActionResult About()
        {
            if (Session["UserName"] != null)
                return View();
            else
                return RedirectToAction("Login", "Home");
        }

        public ActionResult Contact()
        {
            if (Session["UserName"] != null)
                return View();
            else
                return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public JsonResult GetClientInfo(string ClientName, string StoreUrl, string City)
        {
            List<ClientUser> lstClientinfo = null;
            int ReturnCode = 0;

            objBAL = new DataModel();
            ReturnCode = objBAL.GetClientInfo(ClientName, StoreUrl, City, out lstClientinfo);

            return Json(lstClientinfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddCategory(Category obj)

        {
            string msg = "";
            long RIMasterID = 0;
            TempData["CatMess"] = "";
            string JParamVal = JsonConvert.SerializeObject(obj);
            objBAL = new DataModel();
            RIMasterID = objBAL.DMLCatMaster(JParamVal);
            if (RIMasterID > 0)
            {
                msg = "Inserted Successfully";
            }
            else
            {
                msg = "Error Occured, Please check it.";
            }
            TempData["CatMess"] = msg;
            return View();
        }

        [HttpPost]
        public JsonResult GetParentCateogryName(string prefixText, string Action)
        {
            List<Category> lstCat = null;
            int ReturnCode = 0;

            objBAL = new DataModel();
            ReturnCode = objBAL.GetAutocompleteCat(prefixText, Action, out lstCat);

            return Json(lstCat, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BulkUpdate(HttpPostedFileBase CSVFile, string hdnMsgStatus, string hdnId)
        {
            long returnCode = -1;
            string RIMasterJson = string.Empty;
            string ErrorMsg = string.Empty;
            int id = Convert.ToInt32(hdnId);

            string _filePath = string.Empty;
            string _FileName = string.Empty;
            string FPath = ConfigurationManager.AppSettings["ProductUploadPath"];

            objBAL = new DataModel();
            try
            {
                long loginID = Convert.ToInt64(hdnId);

                if (ModelState.IsValid)
                {

                    if (CSVFile.ContentLength > 0)
                    {
                        _FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(CSVFile.FileName);
                        var originalDirectory = new System.IO.DirectoryInfo(string.Format("{0}" + FPath, Server.MapPath(@"\")));
                        _filePath = System.IO.Path.Combine(originalDirectory.ToString(), _FileName);
                        CSVFile.SaveAs(_filePath);
                    }


                    List<Product> lstValues = System.IO.File.ReadAllLines(_filePath)
                                              .Skip(1)
                                              .Select(v => FromCsv(v))
                                              .ToList();

                    List<List<Product>> lstValueList = lstValues.Select((x, i) => new { Index = i, Value = x })
                                                                 .GroupBy(x => x.Index / 5000)
                                                                 .Select(x => x.Select(v => v.Value).ToList()).ToList();

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            for (int i = 0; i < lstValueList.Count; i++)
                            {

                                RIMasterJson = JsonConvert.SerializeObject(lstValueList[i]);
                                returnCode = objBAL.BulkInsertProdMaster("", RIMasterJson, loginID, out ErrorMsg);
                            }
                            transactionScope.Complete();
                            transactionScope.Dispose();
                        }
                        catch (Exception ex)
                        {
                            transactionScope.Dispose();
                            TempData["Alertmsg"] = "please contact Administrator";
                            throw ex;
                        }
                    }
                    if (returnCode != -1)
                    {
                        TempData["Alertmsg"] = ErrorMsg;
                        System.IO.File.Delete(_filePath);
                    }
                    else
                    {
                        TempData["Alertmsg"] = "please contact Administrator";
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["Alertmsg"] = "please contact Administrator";
                throw;
            }

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public ActionResult UploadProductImages(HttpPostedFileBase[] files)
        {

            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/ProductImages/") + InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.  
                        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                    }

                }
            }
            return RedirectToAction("Index", "Home");
        }

        public static Product FromCsv(string csvLineData)
        {
            string[] values = csvLineData.Split(',');


            Product objCsvFileBulkUplaod = new Product();


            objCsvFileBulkUplaod.Name = values[0];
            objCsvFileBulkUplaod.CategoryName = values[1];
            objCsvFileBulkUplaod.Description = values[2];
            objCsvFileBulkUplaod.RegularPrice = Convert.ToDecimal(values[3]);
            objCsvFileBulkUplaod.TaxType = values[4];
            objCsvFileBulkUplaod.IsRecommended = values[5];
            objCsvFileBulkUplaod.IsActive = values[6];
            objCsvFileBulkUplaod.Email = values[7];
            objCsvFileBulkUplaod.image = values[8];


            return objCsvFileBulkUplaod;
        }

        public ActionResult EditClient(string hdnId)
        {
            if (Session["UserName"] != null)
            {
                int i = Convert.ToInt32(hdnId);
                ClientUser obj = new ClientUser();
                objBAL = new DataModel();
                objBAL.GetClientUser(i, out obj);
                return View(obj);
            }
            else
                return RedirectToAction("Login", "Home");

        }

        [HttpPost]
        public ActionResult Index(ClientUser user)
        {
            string msg = "";
            long RIMasterID = 0;
            TempData["UserMess"] = "";
            // user.Password = _encrypt(user.Password);
            try
            {
                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg" };
                if (user.ImageFileLogo != null)
                {
                    if (user.ImageFileLogo.FileName.Length > 0)
                    {
                        //Use Namespace called :  System.IO  
                        string FileName = Path.GetFileNameWithoutExtension(user.ImageFileLogo.FileName);

                        //To Get File Extension  
                        string FileExtension = Path.GetExtension(user.ImageFileLogo.FileName);
                        if (allowedExtensions.Contains(FileExtension)) //check what type of extension  
                        {

                            //Add Current Date To Attached File Name  
                            //  FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                            FileName = Guid.NewGuid().ToString() + FileExtension;
                            user.ClientLogo = FileName;
                            //Get Upload path from Web.Config file AppSettings.  
                            string ServerPath = ConfigurationManager.AppSettings["ClientLogo"].ToString();

                            //string UploadPath = Path.Combine("C:\\Users\\ThisPc\\Documents\\Visual Studio 2017\\Projects\\AllDealz\\AllDealz\\ClientLogo", FileName);
                            string UploadPath = Path.Combine(ServerPath, FileName);


                            //Its Create complete path to store in server.  
                            var fullPath = UploadPath;
                            //To copy and save file into server.  
                            user.ImageFileLogo.SaveAs(fullPath);
                        }
                    }
                }
                if (user.ImageFileBanner != null)
                {
                    if (user.ImageFileBanner.FileName.Length > 0)
                    {
                        //Use Namespace called :  System.IO  
                        string FileName = Path.GetFileNameWithoutExtension(user.ImageFileBanner.FileName);

                        //To Get File Extension  
                        string FileExtension = Path.GetExtension(user.ImageFileBanner.FileName);
                        if (allowedExtensions.Contains(FileExtension)) //check what type of extension  
                        {
                            //Add Current Date To Attached File Name  
                            //  FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                            FileName = Guid.NewGuid().ToString() + FileExtension;
                            user.ClientBanner = FileName;

                            //Get Upload path from Web.Config file AppSettings.  
                            string ServerPath = ConfigurationManager.AppSettings["ClientBanner"].ToString();
                            string UploadPath = Path.Combine(ServerPath, FileName);

                            //string UploadPath = Path.Combine("C:\\Users\\ThisPc\\Documents\\Visual Studio 2017\\Projects\\AllDealz\\AllDealz\\ClientBanner", FileName);

                            //Its Create complete path to store in server.  
                            var fullPath = UploadPath;
                            //To copy and save file into server.  
                            user.ImageFileBanner.SaveAs(fullPath);
                        }
                    }
                }
                if (string.IsNullOrEmpty(user.ClientBanner))
                    user.ClientBanner = string.Empty;
                if (string.IsNullOrEmpty(user.ClientLogo))
                    user.ClientLogo = string.Empty;
                string JParamVal = JsonConvert.SerializeObject(user);
                objBAL = new DataModel();
                RIMasterID = objBAL.DMLUserMaster(JParamVal);
                if (RIMasterID > 0)
                {
                    msg = "Inserted Successfully";
                }
                else
                {
                    msg = "Error Occured, Please check it.";
                }
                TempData["UserMess"] = msg;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View();
        }
        private string _encrypt(string toEncrypt)
        {
            string key = "nextGENp@55w0rd";
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                // MessageBox.Show(ex.Message);
            }
            return toEncrypt;
        }
    }
}