using AceVowEntities;


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
        public ActionResult SingleProductFlyer(List<PreviewDeals> lstPreview)
        {

            if (lstPreview != null)
            {
                objBAL = new DealsModel();
                List<PreviewDeals> lstResponse = objBAL.GetSingleProductFlyer(lstPreview);
                CalcPrice(lstResponse);
                string Path = "ProductImages/" + lstResponse[0].image;
               // Image image = Image.FromFile(Server.MapPath("~/Content/img/toendra.JPG"));

                using (Image image = Image.FromFile(Server.MapPath("~/ProductImages/"+lstResponse[0].image)))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        lstResponse[0].image = base64String;
                       // return base64String;
                    }
                }

                lstPreview = lstResponse;
                Session["PreviewFlyer"] = lstPreview;
            }
            if (Session["PreviewFlyer"] != null)
            {
                lstPreview = (List<PreviewDeals>)Session["PreviewFlyer"];
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
    }
}