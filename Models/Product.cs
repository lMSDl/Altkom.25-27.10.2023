using Models.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product : Entity
    {
        [Required]
        [NotNull]
        [MyValidationAnnotation(Value = ".")]
        [MyValidationAnnotation(Value = "jabłka")]
        public string Name { get; set; }
        [Range(0.01, double.MaxValue)]
        public float Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Nie ma takiej listy")]
        public int ShoppingListId { get; set; }
    }
}
