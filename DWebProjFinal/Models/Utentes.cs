using System.ComponentModel.DataAnnotations;

namespace DWebProjFinal.Models
{
    public class Utentes
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telemovel { get; set; }

        public DateOnly dataNasc { get; set; }

        public string Biografia {  get; set; }

        public ICollection<Paginas> ListaPaginas { get; set; }





    }
}
