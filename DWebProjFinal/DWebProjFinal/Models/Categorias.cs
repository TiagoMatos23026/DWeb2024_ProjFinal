using System.ComponentModel.DataAnnotations;

namespace DWebProjFinal.Models
{
    public class Categorias
    {
        public Categorias()
        {
            ListaPaginas = new HashSet<Paginas>();
        }
        [Key]
        public int Id { get; set; }

        public string Nome { get; set;}

        public ICollection<Paginas> ListaPaginas { get; set; }
    }
}
