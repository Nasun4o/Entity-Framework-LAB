using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefect.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SolarSystem
    {
        public SolarSystem()
        {
            this.Planets = new HashSet<Planet>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Star> Stars { get; set; }
        public virtual ICollection<Planet> Planets  { get; set; }
           

    }
}
