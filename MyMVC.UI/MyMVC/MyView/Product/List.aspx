<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="MyMVC.UI.MyMVC.MyView.Product.List" %>

<%@ Import Namespace="MyMVC.UI.MyMVC.MyModel" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%
                var productList = (List<Product>)HttpContext.Current.Items["productList"];
            %>
        </div>
        <div>
            <%
                foreach (var product in productList)
                {
            %>
            <div><%=product.Title %></div>
            <%
                }
            %>
        </div>
    </form>
</body>
</html>
