using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassDefect.Data;
using Newtonsoft.Json;
using System.IO;
using System.Data.Entity.Migrations;

namespace MassDefect.ExportJson
{
    public class Program
    {
        private const string exportPathPlantes = @"../../../planetsFromJson.json";
        private const string exportPathPeople = @"../../../peopleFromJson.json";
        private const string exportPathAnomaly = @"../../../anomalyFromJson.json";


        static void Main()
        {
           
            var context = new MassDefectContext();

            ExportPlanetsWhichAreNotAnomalyOrigins(context);

            ExportPeopleWhichHaveNotBeenVictims(context);

            ExportTopAnomaly(context);

        }

        private static void ExportTopAnomaly(MassDefectContext context)
        {
            var exportedAnomaliy = context.Anomalies.OrderByDescending(anom => anom.PersonVictims.Count).Take(1)
                .Select(anom => new
                {
                    id = anom.Id,
                    originPlanet = new
                    {
                       name = anom.OriginPlanet.Name
                    },
                    teleportPlanet = new
                    {
                        name = anom.TeleportPoint.Name
                    },
                    victimsCount = anom.PersonVictims.Count
                    
                });

            var anomalyExport = JsonConvert.SerializeObject(exportedAnomaliy, Formatting.Indented);
            File.WriteAllText(exportPathAnomaly, anomalyExport);

        }

        private static void ExportPlanetsWhichAreNotAnomalyOrigins(MassDefectContext context)
        {
            var exportedPlanets = context.Planets.Where(planet => !planet.OriginAnomalies.Any()).
                Select(planet => new
                {
                    name = planet.Name
                });

            var planetExport = JsonConvert.SerializeObject(exportedPlanets, Formatting.Indented);
            File.WriteAllText(exportPathPlantes, planetExport);
        }

        private static void ExportPeopleWhichHaveNotBeenVictims(MassDefectContext context)
        {
            var exportedPeoples = context.Persons.Where(person => !person.Anomalies.Any()).Select(
                people => new
                {
                    name = people.Name,
                    homePlanet = new
                    {
                        name = people.HomePlanet.Name
                    }
                });
            var peopleExport = JsonConvert.SerializeObject(exportedPeoples, Formatting.Indented);
            File.WriteAllText(exportPathPeople, peopleExport);
        }
    }
}
