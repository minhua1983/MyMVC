using System;
using System.Web;
using System.Web.Compilation;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MyMVC.UI.MyMVC.MyController;
using MyMVC.UI.MyAttribute.Route;

namespace MyMVC.UI.MyModule
{
    public class MyUrlRoutingModule : IHttpModule
    {
        static Dictionary<string, string> controllerUrlDictionary = new Dictionary<string, string>();

        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it

            //*处理Dispatcher的MyRoutePrefix特性
            Assembly assembly = Assembly.GetAssembly(typeof(AbstractDispatcher));
            Type[] types = assembly.GetTypes();
            foreach (Type t in types)
            {
                if (t.BaseType == typeof(AbstractDispatcher))
                {
                    //context.Response.Write(t.FullName + "<br />");
                    MyRoutePrefixAttribute myRoutePrefixAttribute = (MyRoutePrefixAttribute)t.GetCustomAttribute(typeof(MyRoutePrefixAttribute));
                    if (myRoutePrefixAttribute != null)
                    {
                        if (!controllerUrlDictionary.ContainsKey(myRoutePrefixAttribute.ControllrtUrl))
                        {
                            controllerUrlDictionary.Add(myRoutePrefixAttribute.ControllrtUrl, t.Name.Replace("Dispatcher", "").ToLower());
                        }
                    }

                }
            }
            //*/

            context.PreRequestHandlerExecute += (sender, e) =>
            {
                /*
                ICollection assemblies = BuildManager.GetReferencedAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    context.Response.Write(assembly.FullName + "<br />");
                }
                //*/



                var request = HttpContext.Current.Request;
                var response = HttpContext.Current.Response;
                var server = HttpContext.Current.Server;
                var rawUrl = request.RawUrl;
                var rawUrlWithNoneParameter = rawUrl.Split('#')[0].Split('?')[0];
                if (!File.Exists(server.MapPath("~" + rawUrlWithNoneParameter)))
                {
                    var urlItems = rawUrlWithNoneParameter.Split('/');
                    var urlItemsCount = urlItems.Length;
                    var dispatcherName = urlItemsCount >= 2 ? (urlItems[1].ToLower() == "" ? "Product" : urlItems[1].ToLower()) : "Product";
                    var actionName = urlItemsCount >= 3 ? (urlItems[2].ToLower() == "" ? "Index" : urlItems[2].ToLower()) : "Index";

                    if (controllerUrlDictionary.Count > 0)
                    {
                        if (controllerUrlDictionary.ContainsKey(dispatcherName))
                        {
                            dispatcherName = controllerUrlDictionary[dispatcherName];
                        }
                    }

                    HttpContext.Current.Items.Add("dispatcherName", dispatcherName);
                    HttpContext.Current.Items.Add("actionName", actionName);
                    HttpContext.Current.RewritePath("~/MyMVC/MyController/" + dispatcherName + "Dispatcher.ashx");
                }
            };
            context.LogRequest += new EventHandler(OnLogRequest);
        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }
    }
}
