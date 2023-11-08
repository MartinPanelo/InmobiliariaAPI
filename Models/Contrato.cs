using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models{


    [Table("Contrato")]
    public class Contrato{
        [Key]
		[Display(Name = "Identificador")]
        public int IdContrato {get; set;}

        [Display(Name = "Fecha desde")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime FechaInicio { get; set; }
            
        [Display(Name = "Fecha hasta")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime FechaFin { get; set; }

        [Required]
        public decimal MontoAlquiler { get; set; }

		/* [Display(Name = "Inquilino")]
		public int InquilinoId { get; set; } */
		[ForeignKey("InquilinoId")]
		public Inquilino? Inquilino{ get; set; }


       /*  [Display(Name = "Inmueble")]
		public int InmuebleId { get; set; }
		[ForeignKey(nameof(InmuebleId))] */

        [ForeignKey("InmuebleId")]
		public Inmueble? Inmueble{ get; set; }




    }
}