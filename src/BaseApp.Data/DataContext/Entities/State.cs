using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Data.DataContext.Entities
{
    public class State
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int CountryId { get; set; }
        [Required, StringLength(64)]
        public string Name { get; set; }
        [Required, StringLength(16)]
        public string Code { get; set; }

        public virtual Country Country { get; set; }
    }
}
