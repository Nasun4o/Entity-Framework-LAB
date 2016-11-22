using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefect.Application
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using MassDefect.Data;
    using MassDefect.Data.Migrations;
    using MassDefect.Models;
    using MassDefect.Models.DTO;
    using Newtonsoft.Json;


    class Program
    {
        private const string SolarSystemPath = @"..\..\..\datasets\solar-systems.json";
        private const string StarsPath = @"..\..\..\datasets\stars.json";
        private const string PlanetsPath = @"..\..\..\datasets\planets.json";
        private const string PersonsPath = @"..\..\..\datasets\persons.json";
        private const string AnomaliesPath = @"..\..\..\datasets\anomalies.json";
        private const string AnomalyVictimsPath = @"..\..\..\datasets\anomaly-victims.json";


        static void Main()
        {

            //var mig = new MigrateDatabaseToLatestVersion<MassDefectContext, Configuration>();
            var context = new MassDefectContext();
        
            
            context.SaveChanges();



            ImportySolarSystems();
            ImportyStars();
            ImportyPlanets();
            ImportPersons();
            ImportAnomalies();
            ImportyAnomalyVictims();


        }

        private static void ImportPersons()
        {
            var context = new MassDefectContext();
            var json = File.ReadAllText(PersonsPath);
            var persons = JsonConvert.DeserializeObject<IEnumerable<PersonDTO>>(json);

            foreach (var person in persons)
            {
                if (person.Name == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                    continue;
                }
                var personEntity = new Person()
                                            {
                                                Name = person.Name,
                                                HomePlanet = GetHomePlanet(person.HomePlanet, context)
                                            };
                context.Persons.AddOrUpdate(personEntity);

                Console.WriteLine($"Successfully imported Person {person.Name}.");
            }
            context.SaveChanges();
        }

       
        private static void ImportyAnomalyVictims()
        {
            var context = new MassDefectContext();
            var json = File.ReadAllText(AnomalyVictimsPath);
            var anomalyVictims = JsonConvert.DeserializeObject<IEnumerable<AnomalyVictimDTO>>(json);
            foreach (var anomalyVictim in anomalyVictims)
            {
                if (anomalyVictim.Person == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                    continue;
                }
                
                var anomalyEntity = GetAnomalyById(anomalyVictim.Id, context);
                var personEntity = GetPersonByName(anomalyVictim.Person, context);

                if (anomalyEntity == null || personEntity == null)
                {
                    Console.WriteLine("Invalid DATA");
                    continue;
                }
                anomalyEntity.PersonVictims.Add(personEntity);


            }
            context.SaveChanges();
        }

        private static Person GetPersonByName(string person, MassDefectContext context)
        {
            var anomPerson = context.Persons;
            foreach (var person1 in anomPerson)
            {
                if (person1.Name.Equals(person))
                {
                    return person1;
                }
            }
            return null;
        }

        private static Anomaly GetAnomalyById(int id, MassDefectContext context)
        {
            var anomaly = context.Anomalies;

            foreach (var anom in anomaly)
            {
                if (anom.Id == id)
                {
                    return anom;
                }
            }
            return null;
        }
        private static void ImportAnomalies()
        {
            var context = new MassDefectContext();
            var json = File.ReadAllText(AnomaliesPath);
            var anomalies = JsonConvert.DeserializeObject<IEnumerable<AnomalyDTO>>(json);

            foreach (var anomaly in anomalies)
            {
                if (anomaly.OriginPlanet == null || anomaly.TeleportPlanet == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                    continue;
                }
                var anomaliesEntity = new Anomaly()
                {
                    OriginPlanet = GetHomePlanet(anomaly.OriginPlanet,context),
                    TeleportPoint = GetHomePlanet(anomaly.TeleportPlanet, context)
                };
                context.Anomalies.AddOrUpdate(anomaliesEntity);

                Console.WriteLine($"Successfully imported Anomaly {anomaly.TeleportPlanet}.");
            }
            context.SaveChanges();
        }

        private static void ImportyPlanets()
        {
            var context = new MassDefectContext();
            var json = File.ReadAllText(PlanetsPath);
            var planets = JsonConvert.DeserializeObject<IEnumerable<PlanetDTO>>(json);

            foreach (var planet in planets)
            {
                if (planet.Name == null || planet.Sun == null || planet.SolarSystem == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                    continue;
                }
                var planetEntity = new Planet()
                                       {
                                           Name = planet.Name,
                                           Sun = GetSunByName(planet.Sun, context),
                                           SolarSystem = GetSolarSystemByName(planet.SolarSystem, context)
                                       };
                context.Planets.AddOrUpdate(planetEntity);
                Console.WriteLine($"Successfully imported PLANET {planet.Name}.");
            }
            context.SaveChanges();
        }

        private static Star GetSunByName(string sun, MassDefectContext context)
        {
            var planets = context.Stars;

            foreach (var planet in planets)
            {
                if (planet.Name.Equals(sun))
                {
                    return planet;
                }
            }
            return null;
        }
        private static Planet GetHomePlanet(string homePlanet, MassDefectContext context)
        {
            var planets = context.Planets;

            foreach (var planet in planets)
            {
                if (planet.Name.Equals(homePlanet))
                {
                    return planet;
                }
            }
            return null;
        }

        private static void ImportyStars()
        {
            var context = new MassDefectContext();
            var json = File.ReadAllText(StarsPath);
            var stars = JsonConvert.DeserializeObject<IEnumerable<StarDTO>>(json);
            foreach (var star in stars)
            {
                if (star.Name == null || star.SolarSystem == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                    continue;
                }
                var starEntity = new Star()
                                     {
                                         Name = star.Name,
                      SolarSystem = GetSolarSystemByName(star.SolarSystem, context)
                };

                context.Stars.AddOrUpdate(starEntity);
                Console.WriteLine($"Successfully imported Star {star.Name}.");

            }
            context.SaveChanges();
        }

        private static SolarSystem GetSolarSystemByName(string solarSystem, MassDefectContext context)
        {
            var system = context.SolarSystems;

            foreach (var solarSystem1 in system)
            {
                if (solarSystem1.Name.Equals(solarSystem))
                {
                    return solarSystem1;
                }
            }
            return null;

        }

        private static void ImportySolarSystems()
        {
            var context = new MassDefectContext();
            var json = File.ReadAllText(SolarSystemPath);
            var solarSystems = JsonConvert.DeserializeObject<IEnumerable<SolarSystemDTO>>(json);

            foreach (var solarSystem in solarSystems)
            {
                if (solarSystem.Name == null)
                {
                    Console.WriteLine("Error: Invalid data.");
                    continue;
                }
                var solarSystemEntity = new SolarSystem() { Name = solarSystem.Name };
                context.SolarSystems.AddOrUpdate(solarSystemEntity);

                Console.WriteLine($"Successfully imported Solar System {solarSystem.Name}.");
            }
            context.SaveChanges();
        }
    }
}
