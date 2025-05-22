using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup_Catalog
{
    public class Entity
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string OriginalPath { get; set; }
        public string DestinationPath { get; set; }
        public DateTime LastUpdated { get; set; }
        public string XmlPath { get; set; }

        public Entity() {
            this.Type = "";
            this.Name = "";
            this.OriginalPath = "";
            this.DestinationPath = "";
            this.LastUpdated = new DateTime();
            this.XmlPath = "";
        }
        public Entity(string type, string name, string originalPath, string destinationPath, DateTime lastUpdated, string xmlPath) {
            this.Type = type;
            this.Name = name;
            this.OriginalPath = originalPath;
            this.DestinationPath = destinationPath;
            this.LastUpdated = lastUpdated;
            this.XmlPath = xmlPath;
        }
    }
}
