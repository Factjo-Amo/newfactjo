using System.Collections.Generic;
using Newfactjo.Models;
using Newfactjo.ViewModels;


namespace Newfactjo.ViewModels
{
    public class CategoryWithNewsViewModel
    {
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; }
        public List<News> NewsItems { get; set; }
    }
}
