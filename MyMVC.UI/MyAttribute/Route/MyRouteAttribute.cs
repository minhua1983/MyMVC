using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMVC.UI.MyAttribute.Route
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class MyRouteAttribute : Attribute
    {
        public string ActionUrl { get; }

        public MyRouteAttribute(string actionUrl)
        {
            ActionUrl = actionUrl.ToLower();
        }
    }
}