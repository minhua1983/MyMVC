using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace MyMVC.UI.MyFilter.Action
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MyActionFilterAttribute : MyFilterAttribute, IMyActionFilter
    {
        public virtual void OnActionExecuting()
        {
            Log("MyActionAttribute.OnActionExecuting()");
        }

        public virtual void OnActionExecuted()
        {
            Log("MyActionAttribute.OnActionExecuted()");
        }

        public void Log(string content)
        {
            using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("~/log.txt"), true, Encoding.UTF8))
            {
                writer.WriteLine(content);
            }
        }
    }
}