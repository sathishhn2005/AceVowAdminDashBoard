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
        public JsonResult AddCategory(Category obj)

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
           // TempData["CatMess"] = msg;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateComments(ClientUser obj)

        {
            string msg = "";

            objBAL = new DataModel();
            long returnCode = objBAL.UpdatePostComment(obj.Id, obj.ReplyComment, out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                TempData["CatMess"] = msg;
            }
            else
            {
                msg = "Error Occured, Please check it.";
                TempData["CatMess"] = msg;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
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
                throw ex;
            }

            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public ActionResult BulkPostUpload(HttpPostedFileBase PostCSVFile, string hdnMsgStatus, string hdnId)
        {
            long returnCode = -1;
            string SchedulePostsJson = string.Empty;
            string ErrorMsg = string.Empty;
            //int id = Convert.ToInt32(hdnId);

            string _filePath = string.Empty;
            string _FileName = string.Empty;
            string FPath = ConfigurationManager.AppSettings["PostUploadPath"];

            objBAL = new DataModel();
            try
            {
                string UserId = hdnId;

                if (ModelState.IsValid)
                {

                    if (PostCSVFile.ContentLength > 0)
                    {
                        _FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(PostCSVFile.FileName);
                        var originalDirectory = new System.IO.DirectoryInfo(string.Format("{0}" + FPath, Server.MapPath(@"\")));
                        _filePath = System.IO.Path.Combine(originalDirectory.ToString(), _FileName);
                        PostCSVFile.SaveAs(_filePath);
                    }


                    List<SchedulePosts> lstValues = System.IO.File.ReadAllLines(_filePath)
                                              .Skip(1)
                                              .Select(v => FromPostCsv(v))
                                              .ToList();

                    List<List<SchedulePosts>> lstValueList = lstValues.Select((x, i) => new { Index = i, Value = x })
                                                                 .GroupBy(x => x.Index / 5000)
                                                                 .Select(x => x.Select(v => v.Value).ToList()).ToList();

                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            for (int i = 0; i < lstValueList.Count; i++)
                            {

                                SchedulePostsJson = JsonConvert.SerializeObject(lstValueList[i]);
                                returnCode = objBAL.BulkInsertSchedulePosts("", SchedulePostsJson, UserId, out ErrorMsg);
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
                        TempData["Alertmsg"] = "";
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["Alertmsg"] = "";
                throw ex;
            }

            return RedirectToAction("ClientPost", "Home");

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


            Product objCsvFileBulkUplaod = new Product
            {
                Name = values[0],
                CategoryName = values[1],
                Description = values[2],
                RegularPrice = Convert.ToDecimal(values[3]),
                TaxType = values[4],
                IsRecommended = values[5],
                IsActive = values[6],
                Email = values[7],
                image = values[8]
            };


            return objCsvFileBulkUplaod;
        }
        public static SchedulePosts FromPostCsv(string csvPostLineData)
        {
            string[] values = csvPostLineData.Split(',');
            SchedulePosts objCsvPostBulkUplaod = new SchedulePosts
            {
                FacebookPost = values[0],
                PageName = values[1],
                Message = values[2],
                ImageUrL = values[3],
                ScheduledTime = Convert.ToDateTime(values[4]) //2021 - 08 - 08 12:15:00.0000000
                                                              //ScheduledTime = DateTime.ParseExact(values[4], "yyyy-MM-dd HH:MM:SS", null)

            };
            return objCsvPostBulkUplaod;
        }
        public static Recipes FromRecipeCsv(string csvPostLineData)
        {
            string[] values = csvPostLineData.Split(',');
            //UserName ProductCategory RecipeName RecipeCategory  Ingredients Duration    ImageName Credits Serving

            Recipes objCsvRecipeBulkUplaod = new Recipes
            {
                UserName = values[0],
                CategoryName = values[1],//ProductCategory
                RecipeName = values[2],
                RecipeCategory = values[3],
                Ingredients = values[4],
                Duration = values[5],
                ImageName = values[6],
                Credits = values[7],
                Serving = values[8],


            };
            return objCsvRecipeBulkUplaod;
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
        [HttpGet]
        public JsonResult GetCountQR(ClientUser objUser)
        {
            DateTime PolicyFDate = objUser.FromDate == Convert.ToDateTime("01-01-0001 12.00.00 AM") ? Convert.ToDateTime("01-01-1753 12.00.00 AM") : objUser.FromDate;
            DateTime PolicyTDate = objUser.ToDate == Convert.ToDateTime("01-01-0001 12.00.00 AM") ? Convert.ToDateTime("01-01-1753 12.00.00 AM") : objUser.ToDate;
            objBAL = new DataModel();
            long returnCode = objBAL.GetQRCount(objUser.UserId, PolicyFDate, PolicyTDate);
            return Json(new
            {
                Result = returnCode
            }, JsonRequestBehavior.AllowGet);
            //return Json(returnCode);
        }
        [HttpGet]
        public JsonResult GetDeals()
        {
            objBAL = new DataModel();
            int returnCode = objBAL.GetDeals(out List<ClientUser> lstDeals);
            return Json(new
            {
                Result = lstDeals
            }, JsonRequestBehavior.AllowGet);
            //return Json(lstClientinfo, JsonRequestBehavior.AllowGet);
            //return Json(returnCode);
        }
        public ActionResult ClientPost()
        {
            return View();
        }
        public ActionResult Deals()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetPostComments()
        {
            List<ClientUser> response = null;
            try
            {
                objBAL = new DataModel();
                int i = 0, j = 0;
                i = objBAL.GetComments(j, out response);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(new
            {
                Result = response
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RecipeUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BulkRecipeUpload(HttpPostedFileBase RecipeCSVFile, string hdnMsgStatus, string hdnId)
        {
            if (RecipeCSVFile != null)
            {


                long returnCode = -1;
                string RecipeJson = string.Empty;
                string ErrorMsg = string.Empty;
                //int id = Convert.ToInt32(hdnId);

                string _filePath = string.Empty;
                string _FileName = string.Empty;
                string FPath = ConfigurationManager.AppSettings["RecipeUploadPath"];

                objBAL = new DataModel();
                try
                {
                    string UserId = hdnId;

                    if (ModelState.IsValid)
                    {


                        if (RecipeCSVFile.ContentLength > 0)
                        {
                            _FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(RecipeCSVFile.FileName);
                            var originalDirectory = new System.IO.DirectoryInfo(string.Format("{0}" + FPath, Server.MapPath(@"\")));
                            _filePath = System.IO.Path.Combine(originalDirectory.ToString(), _FileName);
                            RecipeCSVFile.SaveAs(_filePath);
                        }


                        List<Recipes> lstValues = System.IO.File.ReadAllLines(_filePath)
                                                  .Skip(1)
                                                  .Select(v => FromRecipeCsv(v))
                                                  .ToList();

                        List<List<Recipes>> lstValueList = lstValues.Select((x, i) => new { Index = i, Value = x })
                                                                     .GroupBy(x => x.Index / 5000)
                                                                     .Select(x => x.Select(v => v.Value).ToList()).ToList();

                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            try
                            {
                                for (int i = 0; i < lstValueList.Count; i++)
                                {

                                    RecipeJson = JsonConvert.SerializeObject(lstValueList[i]);
                                    returnCode = objBAL.BulkInsertRecipeMaster("", RecipeJson, out ErrorMsg);
                                    TempData["Alertmsg"] = "Recipe Uploaded Successfully.!";
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
                    throw ex;
                }

                return RedirectToAction("RecipeUpload", "Home");
            }
            else
            {

                TempData["Alertmsg"] = "Please Check the csv file.!";
                return RedirectToAction("RecipeUpload", "Home");
            }
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
        [HttpPost]
        public ActionResult UpdateRecipe(Recipes obj)

        {
            string msg = "";
            long RIMasterID = 0;
            TempData["CatMess"] = "";
            string JParamVal = JsonConvert.SerializeObject(obj);
            objBAL = new DataModel();
            RIMasterID = objBAL.DMLRecipeMaster(JParamVal);
            if (RIMasterID > 0)
            {
                msg = "Inserted Successfully";
            }
            else
            {
                msg = "Error Occured, Please check it.";
            }
            TempData["CatMess"] = msg;
           // return RedirectToAction("RecipeUpdate", "Home");
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult RecipeUpdate()
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
        public ActionResult GetRecipe(int UserId)
        {
            objBAL = new DataModel();
            List<Recipes> lstRecipe = new List<Recipes>();
            int i = objBAL.GetRecipeforUpdate(UserId, out lstRecipe);
            return Json(lstRecipe, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeactivateClient(ClientUser objUser)
        {

            int ReturnCode = 0;
            int Uid = Convert.ToInt32(objUser.UserId);

            objBAL = new DataModel();
            ReturnCode = objBAL.DeactivateClient(objUser);

            string Msg = ReturnCode > 0 ? "Submitted Successfully" : "Error occure,Contact Admin";
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeactivateRecipe(Recipes objRecipe)
        {

            int ReturnCode = 0;
           // int Uid = Convert.ToInt32(objUser.UserId);

            objBAL = new DataModel();
            ReturnCode = objBAL.DeactivateRecipe(objRecipe);

            string Msg = ReturnCode > 0 ? "Submitted Successfully" : "Error occure,Contact Admin";
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
    }
}