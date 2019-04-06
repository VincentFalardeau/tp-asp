using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFA.Models
{
    public class CategorieView
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public CategorieView()
        {
            Name = "";
        }

         public CategorieView(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }

        public void Update(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }
    }
}