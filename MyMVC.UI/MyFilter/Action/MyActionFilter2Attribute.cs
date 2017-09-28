using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMVC.UI.MyFilter.Action
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MyActionFilter2Attribute : MyActionFilterAttribute
    {
        public override void OnActionExecuting()
        {
            Log("MyAction2Attribute.OnActionExecuting()");
        }

        public override void OnActionExecuted()
        {
            Log("MyAction2Attribute.OnActionExecuted()");
        }
    }
}