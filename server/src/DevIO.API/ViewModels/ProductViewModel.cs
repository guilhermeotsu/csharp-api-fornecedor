using DevIO.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.API.ViewModels
{
    [ModelBinder(typeof(JsonWithFilesFormDataModelBinder), Name = "product")]
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required!")]
        public Guid ProviderId { get; set; }

        [Required(ErrorMessage = "The field {0} is required!")]
        [StringLength(200, ErrorMessage = "The field {0} must be between {2} and {1}", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required!")]
        [StringLength(1000, ErrorMessage = "The field {0} must be between {2} and {1}", MinimumLength = 2)]
        public string Description { get; set; }
        public IFormFile ImageUpload { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage = "The field {0} is required!")]
        public decimal Value { get; set; }

        [ScaffoldColumn(false)]
        public DateTime RegisterDate { get; set; }
        public bool Active { get; set; }

        [ScaffoldColumn(false)]
        public string NameProvider { get; set; }
    }
}
