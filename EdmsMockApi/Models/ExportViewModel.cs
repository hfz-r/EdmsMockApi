using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EdmsMockApi.Models
{
    public class ExportViewModel
    {
        [Display(Name = "File Content")]
        public IFormFile FileContent { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }
    }
}