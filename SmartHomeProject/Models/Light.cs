using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHomeProject.Models
{
    public partial class Light
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Açıklama boş olamaz")]
        [MinLength(4, ErrorMessage = "Lütfen en az 4 karakter giriniz")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Code boş olamaz")]
        [StringLength(4, ErrorMessage = "Code 4 haneli olmalıdır")]
        public string Code { get; set; }

        public bool Status { get; set; }
    }
}
