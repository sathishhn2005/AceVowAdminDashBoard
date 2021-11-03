using System.Web;
using System.Web.Optimization;

namespace AceVowAdminDashBoard.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                        "~/Content/assets/js/purpose.core.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                      "~/Content/assets/js/purpose.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/assets/libs/fontawesome/all.min.css",
                      "~/Content//assets/css/purpose.css"));
        }
    }
}