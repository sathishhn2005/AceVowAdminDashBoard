using AceVowEntities;

using System.Web.Hosting;
using System.Web.Mvc;
using AceVowBusinessLayer;
using System.Collections.Generic;
using System;
using System.Drawing.Imaging;
using SelectPdf;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using QRCoder;

namespace AceVowAdminDashBoard.Controllers
{

    public class DealsController : Controller
    {
        string source = string.Empty;
        DealsModel objBAL;
        // GET: Deals
        public ActionResult PreviewFlyer(int id)
        {
            List<PreviewDeals> model = new List<PreviewDeals>();

            objBAL = new DealsModel();
            objBAL.GetFlyerPreview(id, out model);
            CalcPrice(model);
            if (Session["Flyer"] != null)
            {
                model = (List<PreviewDeals>)Session["Flyer"];
            }
            return View(model);
        }
        public ActionResult SingleProductFlyer(List<PreviewDeals> lstPreview,int? ThemeId,int? DealId)
        {

            string clientLogo = string.Empty;
            if (lstPreview != null)
            {
                objBAL = new DealsModel();
                string PreviewJson = JsonConvert.SerializeObject(lstPreview);
                List<PreviewDeals> lstResponse = null;
                lstResponse = objBAL.GetSingleProductFlyer(PreviewJson, DealId);
                CalcPrice(lstResponse);
                // string Path = "ProductImages/" + lstResponse[0].image;
                // Image image = Image.FromFile(Server.MapPath("~/Content/img/toendra.JPG"));

                foreach (var item in lstResponse)
                {
                    using (Image image = Image.FromFile(Server.MapPath("~/ProductImages/" + item.image)))
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
                    using (Image image = Image.FromFile(Server.MapPath("~/ProductImages/" + lstResponse[0].ClientLogo)))
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
                ViewBag.OfferStartDate = lstResponse[0].StartDate;
                ViewBag.OfferEndDate = lstResponse[0].StartDate;
                if (!string.IsNullOrEmpty(lstResponse[0].ClientLogo))
                {
                    ViewBag.ClientLogo = clientLogo;
                }
                lstPreview = lstResponse;
                Session["FlyerType"] = null;
                Session["FlyerType"] = ThemeId;
                Session["PreviewFlyer"] = lstPreview;
            }
            if (Session["PreviewFlyer"] != null)
            {
                lstPreview = (List<PreviewDeals>)Session["PreviewFlyer"];
                ViewBag.OfferStartDate = lstPreview[0].StartDate;
                ViewBag.OfferEndDate = lstPreview[0].StartDate;
                using (Image image = Image.FromFile(Server.MapPath("~/ProductImages/" + lstPreview[0].ClientLogo)))
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
            string qrCodeName = HostingEnvironment.MapPath("~/ProductImages/" + Guid.NewGuid().ToString() + ".png");
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.L);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, false);
            Bitmap resized = new Bitmap(qrCodeImage, new Size(50, 50));
            resized.Save(qrCodeName);
            return qrCodeName;
        }
    }
}