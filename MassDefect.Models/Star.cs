using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefect.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Star
    {
        public Star()
        {
            this.Planets = new HashSet<Planet>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? SolarSystemId { get; set; }

        [ForeignKey("SolarSystemId")]
        public virtual SolarSystem SolarSystem  { get; set; }

        public virtual ICollection<Planet> Planets  { get; set; }

    }
}
