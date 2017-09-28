using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMVC.UI.MyMVC.MyModel
{
    public class Product
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = "";
        public DateTime Created_At { get; set; } = DateTime.Parse("1900-01-01");
    }
}