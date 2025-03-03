using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingrese un nombre para el slider")]
        [Display(Name = "Nombre del Slider")]
        public string Nombre { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Display(Name = "Imagen del Slider")]
        [DataType(DataType.ImageUrl)]
        public string Imagen { get; set; }
    }
}
