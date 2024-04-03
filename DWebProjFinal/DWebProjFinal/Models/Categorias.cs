using System.ComponentModel.DataAnnotations;

namespace DWebProjFinal.Models
{
    public class Categorias
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set;}

        public ICollection<Categorias> ListaCategorias { get; set; }
    }
}
