using System.Web;
using System.Web.Optimization;

namespace IRecordweb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/js/select2.min.js",
                      "~/js/jquery-3.5.1.min.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/basescript").Include(
                "~/Scripts/popper.min.js",
                "~/Scripts/bootstrap.min.v4.js",
                "~/js/simplebar.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/basescript1").Include(
               "~/Scripts/metisMenu.min.js",
               "~/Scripts/app.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
         "~/AngScripts/angular.js",
         "~/AngScripts/ngStorage.min.js",
         "~/AngularDataFunction/AngularDataFunction.js"
           ));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/basiccss").Include(
                      "~/Content/bootstrap.min.v4.css",
                      "~/Content/line-awesome.min.css",
                      "~/Content/theme-color.css",
                      "~/Content/app.min.css",
                      "~/Content/custom-body.css",
                      "~/Content/themes/base/jquery-ui.min.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/select2.min.css"
                      ));

        }
    }
}
