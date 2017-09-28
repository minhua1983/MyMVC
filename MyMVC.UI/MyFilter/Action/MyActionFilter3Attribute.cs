using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMVC.UI.MyFilter.Action
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MyActionFilter3Attribute : MyActionFilterAttribute
    {
        public override void OnActionExecuting()
        {
            Log("MyAction3Attribute.OnActionExecuting()");
        }

        public override void OnActionExecuted()
        {
            Log("MyAction3Attribute.OnActionExecuted()");
        }
    }
}