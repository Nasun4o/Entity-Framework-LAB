using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassDefect.ExportXML
{
    using System.Xml.Linq;
    using MassDefect.Data;
    using MassDefect.Models;

    public class Program
    {
        private const string xmlPath = @"../../../anomalies.xml";

        static void Main(string[] args)
        {
            var context = new MassDefectContext();
            var exportedAnomalies =
                context.Anomalies.Select(
                    anomaly =>
                    new
                    {
                        id = anomaly.Id,
                        originPlanet = anomaly.OriginPlanet.Name,
                        teleportPlantes = anomaly.TeleportPoint.Name,
                        victims = anomaly.PersonVictims.Select(name => name.Name)
                    })
                    .OrderBy(exportedAnomaly => exportedAnomaly.id);

            var xmlDocument = new XElement("anomalies");

            foreach (var anomaly in exportedAnomalies)
            {
                var anomalyNode = new XElement("anomaly");
                anomalyNode.Add(new XAttribute("id", anomaly.id));
                anomalyNode.Add(new XAttribute("origin-planet",anomaly.originPlanet));
                anomalyNode.Add(new XAttribute("teleport-planet", anomaly.teleportPlantes));
                var victimsNode = new XElement("victims");

                foreach (var victim in anomaly.victims)
                {
                    var victimNode = new XElement("victim");
                    victimNode.Add(new XAttribute("name", victim));
                    victimsNode.Add(victimNode);

                }
                anomalyNode.Add(victimsNode);
                xmlDocument.Add(anomalyNode);
            }

            xmlDocument.Save(xmlPath);
        }


    }
}
