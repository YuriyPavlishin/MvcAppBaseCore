using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseApp.Data.DataContext.Entities
{
    public class Country
    {
        public Country()
        {
            States = new HashSet<State>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required, StringLength(256)]
        public string Name { get; set; }
        public int? NumericCode { get; set; }
        [StringLength(2)]
        [Required, Column(TypeName = "char(2)")]
        public string Alpha2 { get; set; }
        [StringLength(3)]
        [Column(TypeName = "char(3)")]
        public string Alpha3 { get; set; }
        public int Ordinal { get; set; }

        public virtual ICollection<State> States { get; set; }
    }
}
