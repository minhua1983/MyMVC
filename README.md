# MyMVC
一个基于webform的mvc结构的粗略demo

## 项目结构
* MyMVC.UI\MyAttribute 自定义特性，用于自定义路由机制，可以用于定义Dispatcher(即Controller)和Action的路由。
* MyMVC.UI\MyContainer 自定义容器，用于注册类型，目前只注册了自定义Dispatcher类型，后期可能可能会从容器中获取实例，不过目前暂时没用到。
* MyMVC.UI\MyFilter 自定义过滤器，用于自定义ActionFilter来拦截请求，可以在Action执行前后来复写其OnActionExecuting和OnActionExecuted方法。
* MyMVC.UI\MyModule 自定义模块，核心程序，用于处理请求，分析出Dispatcher和Action名称，再把请求传递给对应的Dispatcher.ashx的一般处理程序来处理请求。
* MyMVC.UI\MyMVC\MyModel 数据模型
* MyMVC.UI\MyMVC\MyView 以aspx文件代替视图
* MyMVC.UI\MyMVC\MyController 自定义Controller，即xxxDispatcher.ashx.cs，可以理解成mvc中的xxxController.cs文件。其继承自AbstractDispatcher.ashx.cs，此AbstractDispatcher类为核心程序，用于在此类中找到和Action名称对应的方法，并执行该方法，如果该方法上有自定义ActionFilter特性时，并在该方法前后执行OnActionExecuting和OnActionExecuted方法。

## 项目灵感
之所以写这样一个项目，就是凭着对“asp.net mvc”和“java中servlet中getRequestDispatcher("xxx.jsp").forward方法”的理解，所以自己尝试着写了一个基于asp.net webform的自定义mvc风格的demo
