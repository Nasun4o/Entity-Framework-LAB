using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefect.AppXML
{
    using System.Xml.XPath;
    using MassDefect.Data;
    using MassDefect.Models;
    using System.Xml.Linq;
    class Program
    {
        private const string NewAnomaliesPath = @"..\..\..\datasets\new-anomalies.xml";


        static void Main(string[] args)
        {
            var xml = XDocument.Load(NewAnomaliesPath);
            var anomalies = xml.XPathSelectElements("anomalies/anomaly");

            var context = new MassDefectContext();

            foreach (var anomaly in anomalies)
            {
                ImportAnomalyAndVictims(anomaly, context);
            }
            context.SaveChanges();
        }

        private static void ImportAnomalyAndVictims(XElement anomalyNode, MassDefectContext context)
        {
            var originPlanetName = anomalyNode.Attribute("origin-planet");
            var teleportPlanetName = anomalyNode.Attribute("teleport-planet");

            if (originPlanetName == null || teleportPlanetName == null)
            {
                Console.WriteLine("Error: Invalid data.");

                return;

            }

            var anomalyEntity = new Anomaly()
            {
                OriginPlanet = GetPlanetByName(originPlanetName.Value, context),
                TeleportPoint = GetPlanetByName(teleportPlanetName.Value, context)
            };

            if (anomalyEntity.OriginPlanet == null || anomalyEntity.TeleportPoint == null)
            {
                Console.WriteLine("Error: Invalid data.");

                return;
            }
            context.Anomalies.Add(anomalyEntity);
            Console.WriteLine($"Successfully imported anomaly");



            var victims = anomalyNode.XPathSelectElements("victims/victim");
            foreach (var victim in victims)
            {
                ImportVictim(victim, context, anomalyEntity);
            }
            context.SaveChanges();
        }

        private static void ImportVictim(XElement victim, MassDefectContext context, Anomaly anomalyEntity)
        {
            var name = victim.Attribute("name");

            if (name == null)
            {
                Console.WriteLine("Error: Invalid data.");

                return;
                
            }
            var personEntity = GetPersonByName(name.Value, context);

            if (personEntity == null)
            {
                Console.WriteLine($"{personEntity} is NULL");
                return;
            }

            anomalyEntity.PersonVictims.Add(personEntity);
            Console.WriteLine($"Successfully imported anomaly.");

            context.SaveChanges();
        }

        private static Person GetPersonByName(string value, MassDefectContext context)
        {
            var names = context.Persons;

            foreach (var item in names)
            {
                if(item.Name == value)
                 {
                    return item;
                 }
            }
                return null;

        }

        private static Planet GetPlanetByName(string value, MassDefectContext context)
        {
            var planets = context.Planets;

            foreach (var planet in planets)
            {
                if (planet.Name == value)
                {
                    return planet;
                }
            }
            return null;
        }
    }
}
