using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefect.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Planet
    {
        public Planet()
        {
            this.Persons = new HashSet<Person>();
            this.OriginAnomalies = new HashSet<Anomaly>();
            this.TeleportAnomalies = new HashSet<Anomaly>();

        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? SunId { get; set; }

        public int? SolarSystemId { get; set; }
        [ForeignKey("SunId")]
        public virtual Star Sun { get; set; }
        [ForeignKey("SolarSystemId")]

        public virtual SolarSystem SolarSystem { get; set; }

        public virtual ICollection<Person> Persons { get; set; }

        public virtual ICollection<Anomaly> OriginAnomalies { get; set; }
        public virtual ICollection<Anomaly> TeleportAnomalies { get; set; }

    }
}

