using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefect.Models
{
    public class Anomaly
    {
        private ICollection<Person> perosnVictims;
        public Anomaly()
        {
            this.perosnVictims = new HashSet<Person>();
        }

        public int Id { get; set; }
        public int? OriginPlanetId { get; set; }
        public int? TeleportPlanetId { get; set; }


        [ForeignKey("OriginPlanetId")]
        [InverseProperty("OriginAnomalies")]
        public virtual Planet OriginPlanet { get; set; }
        [ForeignKey("TeleportPlanetId")]
        public virtual Planet TeleportPoint { get; set; }

        public virtual ICollection<Person> PersonVictims {
            get
            {
                return this.perosnVictims;
            }
            set
            {
                this.perosnVictims = value;
            }
        }
    }
    }