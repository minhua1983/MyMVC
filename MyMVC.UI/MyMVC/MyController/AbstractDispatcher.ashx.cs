using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using MyMVC.UI.MyAttribute.Route;
using MyMVC.UI.MyFilter.Action;

namespace MyMVC.UI.MyMVC.MyController
{
    /// <summary>
    /// Summary description for AbstractDispatcher
    /// </summary>
    public class AbstractDispatcher : IHttpHandler
    {
        protected string dispatcherName = "";
        protected string actionName = "";
        static List<string> noneActionMethodList = new List<string> {
            "processrequest",
            "get_isreusable",
            "tostring",
            "equals",
            "gethashcode",
            "gettype"
        };
        Dictionary<string, string> actionUrlDictionary = new Dictionary<string, string>();

        public virtual void ProcessRequest(HttpContext context)
        {
            dispatcherName = context.Items["dispatcherName"] == null ? "" : context.Items["dispatcherName"].ToString();
            actionName = context.Items["actionName"] == null ? "" : context.Items["actionName"].ToString();

            //context.Response.Write(dispatcherName + "<br />");
            //context.Response.Write(actionName + "<br />");

            List<MethodInfo> methodInfoList = this.GetType().GetMethods().ToList();

            //*处理Action的MyRoute特性
            MyRouteAttribute myRouteAttribute;
            if (actionUrlDictionary.Count == 0)
            {
                methodInfoList.ForEach(mi =>
                {
                    myRouteAttribute = (MyRouteAttribute)mi.GetCustomAttribute(typeof(MyRouteAttribute));
                    if (myRouteAttribute != null)
                    {
                        if (!actionUrlDictionary.ContainsKey(myRouteAttribute.ActionUrl))
                        {
                            actionUrlDictionary.Add(myRouteAttribute.ActionUrl, mi.Name.ToLower());
                        }
                    }
                });
            }
            //*/

            //如果url在字典中不能以url为key匹配到某个action
            if (!InvokeActionByActionUrlDictionary(methodInfoList))
            {
                //*遍历MethodInfo，找到匹配的MethodInfo就执行invoke
                //ForEach小心lambda调用其它方法时传递的参数引起闭包，导致InvokrAction被意外多执行一次
                foreach (MethodInfo mi in methodInfoList)
                {
                    //context.Response.Write(mi.Name + "<br />");
                    if (!noneActionMethodList.Contains(mi.Name.ToLower()))
                    {
                        if (mi.Name.ToLower().Equals(actionName.ToLower()))
                        {

                            if (mi.Name.ToLower() == "index")
                            {
                                MyRouteAttribute myRouteAttributeDefault = (MyRouteAttribute)this.GetType().GetCustomAttribute(typeof(MyRouteAttribute));
                                if (myRouteAttributeDefault != null)
                                {
                                    InvokeActionByActionUrlDictionary(methodInfoList, myRouteAttributeDefault.ActionUrl);
                                    return;
                                }
                            }
                            InvokeAction(mi);
                        }
                    }
                }
                //*/
            }

        }

        bool InvokeActionByActionUrlDictionary(List<MethodInfo> methodInfoList, string actionUrl = "")
        {
            actionUrl = actionUrl == "" ? actionName : actionUrl;
            if (actionUrlDictionary.Count > 0)
            {
                if (actionUrlDictionary.ContainsKey(actionUrl))
                {
                    actionName = actionUrlDictionary[actionUrl];
                    var methodInfo = methodInfoList.Where(mi => mi.Name.ToLower().Equals(actionName)).FirstOrDefault();
                    InvokeAction(methodInfo);
                    return true;
                }
            }
            return false;
        }

        protected void InvokeAction(MethodInfo mi)
        {
            //处理invoke之前的OnActionExecuting()
            var myActionAttributeList = mi.GetCustomAttributes<MyActionFilterAttribute>().ToList();
            myActionAttributeList.ForEach(a => a.OnActionExecuting());
            //执行action
            var request = HttpContext.Current.Request;
            List<object> objectList = new List<object>();

            foreach (ParameterInfo pi in mi.GetParameters())
            {
                string paramsName = string.Empty;

                foreach (string name in request.Params.AllKeys)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (pi.Name.ToLower().Equals(name.ToLower()))
                        {
                            paramsName = name;

                        }
                    }
                }

                if (pi.ParameterType.IsPrimitive)
                {
                    //int.Parse(request.Params[paramsName] ?? (pi.IsOptional ? pi.DefaultValue.ToString() : "0"))
                    //处理基元类型
                    objectList.Add(Convert.ChangeType(request.Params[paramsName] ?? (pi.IsOptional ? pi.DefaultValue.ToString() : Activator.CreateInstance(pi.ParameterType).ToString()), pi.ParameterType));
                }
                else if (pi.ParameterType == typeof(string))
                {
                    //处理string
                    objectList.Add(request.Params[paramsName] ?? (pi.IsOptional ? pi.DefaultValue.ToString() : ""));
                }
                else
                {
                    //处理自定义类型
                    object instance = Activator.CreateInstance(pi.ParameterType);
                    PropertyInfo[] propertyInfoArray = instance.GetType().GetProperties();
                    foreach (PropertyInfo propertyInfo in propertyInfoArray)
                    {
                        foreach (string name in request.Params.AllKeys)
                        {
                            if (propertyInfo.Name.ToLower().Equals(name.ToLower()))
                            {
                                Type t = propertyInfo.PropertyType;
                                propertyInfo.SetValue(instance, Convert.ChangeType(request.Params[name], propertyInfo.PropertyType));
                            }
                        }
                    }

                    objectList.Add(instance);
                }
            }

            mi.Invoke(this, objectList.ToArray());

            //处理invoke之后的OnActionExecuted()
            var myActionAttributeListTemp = myActionAttributeList;
            myActionAttributeListTemp.Reverse();
            myActionAttributeListTemp.ForEach(a => a.OnActionExecuted());
        }

        protected void View()
        {
            View(actionName);
        }

        protected void View(string actionName)
        {
            View(actionName, dispatcherName);
        }

        protected void View(string actionName, string dispatcherName)
        {
            HttpContext.Current.Server.Execute("~/MyMVC/MyView/" + dispatcherName + "/" + actionName + ".aspx");
        }

        protected void Json(object data)
        {
            //HttpContext.Current.Response.ContentType = "application/json;charset=utf-8";
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Charset = "utf-8";
            string json = JsonConvert.SerializeObject(data);
            using (StreamWriter writer = new StreamWriter(HttpContext.Current.Response.OutputStream))
            {
                writer.Write(json);
            }
        }

        protected void Content(string content)
        {
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Charset = "utf-8";
            using (StreamWriter writer = new StreamWriter(HttpContext.Current.Response.OutputStream))
            {
                writer.Write(content);
            }
        }

        protected void Html(string html)
        {
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.Charset = "utf-8";
            using (StreamWriter writer = new StreamWriter(HttpContext.Current.Response.OutputStream))
            {
                writer.Write(html);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
