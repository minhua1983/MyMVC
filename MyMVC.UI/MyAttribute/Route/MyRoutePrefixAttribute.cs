using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMVC.UI.MyAttribute.Route
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MyRoutePrefixAttribute : Attribute
    {
        public string ControllrtUrl { get; }

        public MyRoutePrefixAttribute(string controllerUrl)
        {
            ControllrtUrl = controllerUrl.ToLower();
        }
    }
}