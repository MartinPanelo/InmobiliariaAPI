using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models{


    [Table("Pago")]
    public class Pago{
        [Key]
		[Display(Name = "Identificador")]
        public int IdPago {get; set;} 

        public int Numero { get; set; }

        [ForeignKey(nameof(ContratoId))]
		[Display(Name = "Contrato")]
		public int ContratoId { get; set; }
        public Contrato? Contrato{ get; set; }
		public decimal Importe{ get; set; }
        
        [DataType(DataType.Date)]
        public DateTime FechaDePago { get; set; }



    }
}