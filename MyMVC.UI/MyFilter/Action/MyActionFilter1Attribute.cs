using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMVC.UI.MyFilter.Action
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MyActionFilter1Attribute : MyActionFilterAttribute
    {
        public override void OnActionExecuting()
        {
            Log("MyAction1Attribute.OnActionExecuting()");
        }

        public override void OnActionExecuted()
        {
            Log("MyAction1Attribute.OnActionExecuted()");
        }
    }
}