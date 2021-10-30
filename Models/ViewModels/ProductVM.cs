using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocky.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> MyProperty { get; set; }
    }
}