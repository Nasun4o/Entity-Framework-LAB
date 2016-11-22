using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefect.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Persons")]
    public class Person
    {

        public Person()
        {
            this.Anomalies = new HashSet<Anomaly>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? HomePlanetId { get; set; }

        [ForeignKey("HomePlanetId")]
        public virtual Planet HomePlanet { get; set; }

        public virtual ICollection<Anomaly> Anomalies  { get; set; }
    }
}

