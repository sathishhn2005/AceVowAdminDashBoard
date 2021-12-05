using AceVowEntities;
using System.Web.Hosting;
using System.Web.Mvc;
using AceVowBusinessLayer;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using QRCoder;
using System.Configuration;

namespace AceVowAdminDashBoard.Controllers
{

    public class DealsController : Controller
    {
        string source = string.Empty;
        DealsModel objBAL;
        //  DataModel objDBAL;
        // GET: Deals
        public ActionResult PreviewFlyer(int id, int flag = 0)
        {
            //   List<PreviewDeals> model = new List<PreviewDeals>();
          //  Session["Flyer"] = null;

            objBAL = new DealsModel();
            List<PreviewDeals> model;
            List<Category> lstCategory;
            if (flag > 0)
            {
                objBAL.GetCategoryFlyerPreview(id, out model, out lstCategory);
                Session["Flyer"] = model;
            }
            else
            {
                objBAL.GetFlyerPreview(id, out model, out lstCategory);
              //  Session["Flyer"] = model;
            }
            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (var item in lstCategory)
            {
                categoryList.Add(new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Id) });
            }

            ViewBag.lstCategoryList = categoryList;
            CalcPrice(model);
            if (Session["Flyer"] != null)
            {
                model = (List<PreviewDeals>)Session["Flyer"];
            }
            ModelState.Clear();

            return View(model);
        }
       
        public ActionResult SingleProductFlyer(List<PreviewDeals> lstPreview, int? ThemeId, int? DealId,string ThemeColor)
        {

            string clientLogo = string.Empty;
            if (lstPreview != null)
            {
                objBAL = new DealsModel();
                string PreviewJson = JsonConvert.SerializeObject(lstPreview);
                List<PreviewDeals> lstResponse = null;
                lstResponse = objBAL.GetSingleProductFlyer(PreviewJson, DealId);
                CalcPrice(lstResponse);
                string path = ConfigurationManager.AppSettings["ProductImage"];
                // string Path = "ProductImages/" + lstResponse[0].image;
                //C:\inetpub\wwwroot\AllDealz_Prod\ProductImages
                // Image image = Image.FromFile(Server.MapPath("~/Content/img/toendra.JPG"));

                foreach (var item in lstResponse)
                {
                    using (Image image = Image.FromFile(path + item.image))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            string base64String = Convert.ToBase64String(imageBytes);
                            item.image = base64String;
                            // return base64String;
                        }
                    }


                }
                if (!string.IsNullOrEmpty(lstResponse[0].ClientLogo))
                {
                    string ClientLogoPath = ConfigurationManager.AppSettings["ClientLogo"];
                    using (Image image = Image.FromFile(ClientLogoPath + lstResponse[0].ClientLogo))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            string base64String = Convert.ToBase64String(imageBytes);
                            clientLogo = base64String;
                            // return base64String;
                        }
                    }
                }
                int syear = lstResponse[0].StartDate.Year;
                string smonth = lstResponse[0].StartDate.ToString("MMM");// lstPreview[0].StartDate.Month;
                int sDay = lstResponse[0].StartDate.Day;

                int eyear = lstResponse[0].EndDate.Year;
                string emonth = lstResponse[0].EndDate.ToString("MMM");
                int eDay = lstResponse[0].EndDate.Day;

                var suffixSday = Add_st_nd_rd_thSuffixDay(sDay);
                var suffixEday = Add_st_nd_rd_thSuffixDay(eDay);

                ViewBag.SYear = syear;
                ViewBag.SMonth = smonth;
                ViewBag.SDay = sDay;
                ViewBag.SSup = suffixSday;

                ViewBag.EYear = eyear;
                ViewBag.EMonth = emonth;
                ViewBag.EDay = eDay;
                ViewBag.ESup = suffixEday;


                ViewBag.OfferStartDate = lstResponse[0].StartDate;
                ViewBag.OfferEndDate = lstResponse[0].EndDate;
                ViewBag.Address = lstResponse[0].Address;
                ViewBag.PirmaryContact = lstResponse[0].PirmaryContact;
                ViewBag.DealName = lstResponse[0].Domain;

                //   ViewBag.QRCode = QRCode;
                if (!string.IsNullOrEmpty(lstResponse[0].ClientLogo))
                {
                    ViewBag.ClientLogo = clientLogo;
                }
                lstPreview = lstResponse;
                Session["FlyerType"] = null;
                Session["FlyerType"] = ThemeId;
                Session["PreviewFlyer"] = lstPreview;
                Session["ThemeColor"] = ThemeColor;
            }
            if (Session["PreviewFlyer"] != null)
            {
                lstPreview = (List<PreviewDeals>)Session["PreviewFlyer"];
                string QRCode = GenerateQR(lstPreview[0].DealsUrl);

                int syear = lstPreview[0].StartDate.Year;
                string smonth = lstPreview[0].StartDate.ToString("MMM");// lstPreview[0].StartDate.Month;
                int sDay = lstPreview[0].StartDate.Day;

                int eyear = lstPreview[0].EndDate.Year;
                string emonth = lstPreview[0].EndDate.ToString("MMM");
                int eDay = lstPreview[0].EndDate.Day;

                var suffixSday = Add_st_nd_rd_thSuffixDay(sDay);
                var suffixEday = Add_st_nd_rd_thSuffixDay(eDay);

                ViewBag.SYear = syear;
                ViewBag.SMonth = smonth;
                ViewBag.SDay = sDay;
                ViewBag.SSup = suffixSday;

                ViewBag.EYear = eyear;
                ViewBag.EMonth = emonth;
                ViewBag.EDay = eDay;
                ViewBag.ESup = suffixEday;


                ViewBag.OfferStartDate = lstPreview[0].StartDate;
                ViewBag.OfferEndDate = lstPreview[0].EndDate;
                ViewBag.Address = lstPreview[0].Address;
                ViewBag.PirmaryContact = lstPreview[0].PirmaryContact;
                ViewBag.QRCode = QRCode;
                ViewBag.DealName = lstPreview[0].Domain;
                string ClientLogoPath = ConfigurationManager.AppSettings["ClientLogo"];
                using (Image image = Image.FromFile(ClientLogoPath + lstPreview[0].ClientLogo))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        clientLogo = base64String;
                        ViewBag.ClientLogo = clientLogo;
                        // return base64String;
                    }
                }

            }
            if (ThemeColor == null) {
                ThemeColor = Session["ThemeColor"].ToString();
            }
            ViewBag.ThemeColor = ThemeColor;
            return View(lstPreview);

        }
        private void CalcPrice(List<PreviewDeals> lstModel)
        {
            if (lstModel != null)
            {
                if (lstModel.Count > 0)
                {
                    foreach (var item in lstModel)
                    {
                        if (!item.OfferPrice.Equals(0))
                        {
                            item.SavingPrice = Convert.ToDecimal(item.RegularPrice - item.OfferPrice);
                            item.SavingPercentage = Convert.ToInt32(item.SavingPrice / item.RegularPrice * 100);
                            item.Price = item.OfferPrice;
                        }
                        else
                        {
                            item.Price = Convert.ToDecimal(item.RegularPrice);
                        }
                    }
                }
            }
        }
        private string GenerateQR(string url)
        {
            string qrCodeName = HostingEnvironment.MapPath("~/ProductImages/" + Guid.NewGuid().ToString() + ".jpeg");
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, false);
            Bitmap resized = new Bitmap(qrCodeImage, new Size(50, 50));
            resized.Save(qrCodeName);
            using (Image image = Image.FromFile(qrCodeName))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    qrCodeName = base64String;
                    // return base64String;
                }
            }
            return qrCodeName;
        }

        private string Add_st_nd_rd_thSuffixDay(int day)
        {
            var j = day % 10;
            var k = day % 100;
            if (j == 1 && k != 11)
            {
                return "st";
            }
            if (j == 2 && k != 12)
            {
                return "nd";
            }
            if (j == 3 && k != 13)
            {
                return "rd";
            }
            return "th";
        }
    }
}