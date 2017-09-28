using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMVC.UI.MyFilter.Action
{
    public interface IMyActionFilter
    {
        void OnActionExecuting();
        void OnActionExecuted();
    }
}
