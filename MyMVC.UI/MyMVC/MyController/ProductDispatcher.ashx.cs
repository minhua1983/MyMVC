using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using MyMVC.UI.MyMVC.MyModel;
using MyMVC.UI.MyAttribute.Route;
using MyMVC.UI.MyFilter.Action;

namespace MyMVC.UI.MyMVC.MyController
{
    [MyRoutePrefix("p")]
    [MyRoute("html")]
    /// <summary>
    /// Summary description for ProductDispatcher
    /// </summary>
    public class ProductDispatcher : AbstractDispatcher
    {
        /*
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
        }
        //*/

        [MyActionFilter2]
        [MyActionFilter3]
        [MyActionFilter1]
        [MyRoute("all")]
        public void Index()
        {
            List<Product> productList = new List<Product>() {
                new Product() { Id=1,Title="game" },
                new Product() { Id=2,Title="book" }
            };
            HttpContext.Current.Items.Add("productList", productList);
            View("List");
            //View("detail");
            //View("profile","user");
        }

        [MyRoute("data")]
        public void ShowData()
        {
            //*
            List<Product> productList = new List<Product>() {
                new Product() { Id=1,Title="game" },
                new Product() { Id=2,Title="book" }
            };
            //*/

            Json(productList);
        }

        [MyRoute("content")]
        public void ShowContent()
        {
            Content("<h1>content</h1>");
        }

        [MyRoute("html")]
        public void ShowHtml()
        {
            //Html("<html><body><h1>html</h1></body></html>");
            Html("<h1>html</h1>");
        }

        [MyRoute("info")]
        public void Detail()
        {
            View();
        }

        public void Test(float v=1.2f, string name = "kaz", Product product = null)
        {
            Content("v=" + v + ",name=" + name + ",product.Id=" + product.Id + ",product.Title=" + product.Title);
        }
    }
}