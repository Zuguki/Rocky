using System.Collections.Generic;

namespace Rocky.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ApplicationType> ApplicationTypes { get; set; }
    }
}