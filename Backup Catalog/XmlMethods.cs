using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace Backup_Catalog
{
    class XmlMethods
    {
        private static readonly XmlDocument document = new XmlDocument();

        public static List<Entity> GetChildEntities(string xmlPath) {
            document.LoadXml(File.ReadAllText(Properties.Settings.Default.PathFile));

            List<Entity> entityList = new List<Entity>();

            string path = GetXmlPath(xmlPath);

            XmlNodeList nodes = document.SelectSingleNode(path).ChildNodes;

            for (int i = 0; i < nodes.Count; i++) {
                string name = nodes[i].Name;
                XmlNode node = nodes[i];

                if (name == "entity") {
                    if (nodes[i].Attributes["type"].Value == "group") {
                        entityList.Add(GetGroupEntity(node, xmlPath));
                    } else if (nodes[i].Attributes["type"].Value == "folder-bundle") {
                        entityList.Add(GetDirectoryBundleEntity(node, xmlPath));
                    } else if (nodes[i].Attributes["type"].Value == "file-bundle") {
                        entityList.Add(GetFileBundleEntity(node, xmlPath));
                    }else if (nodes[i].Attributes["type"].Value == "file"){
                        entityList.Add(GetFileEntity(node, xmlPath));
                    }else if (nodes[i].Attributes["type"].Value == "folder"){
                        entityList.Add(GetDirectoryEntity(node, xmlPath));
                    }
                }
            }

            return entityList;
        }

        private static Entity GetGroupEntity(XmlNode node, string xmlPath) {
            Entity entity = new Entity
            {
                Type = EntityTypes.GroupType,
                Name = node.Attributes["name"].Value,
                OriginalPath = "",
                DestinationPath = "",
                LastUpdated = new DateTime(),
                XmlPath = xmlPath + "/" + node.Attributes["name"].Value
            };

            return entity;
        }
        private static Entity GetFileEntity(XmlNode node, string xmlPath) {
            Entity entity = new Entity
            {
                Type = EntityTypes.FileType,
                Name = node.Attributes["name"].Value,
                OriginalPath = node.SelectSingleNode("original-path").InnerText,
                DestinationPath = node.SelectSingleNode("destination-path").InnerText,
                LastUpdated = DateTime.FromFileTime(Int64.Parse(node.SelectSingleNode("last-updated").InnerText)),
                XmlPath = xmlPath + "/" + node.Attributes["name"].Value
            };

            return entity;
        }
        private static Entity GetFileBundleEntity(XmlNode node, string xmlPath) {
            Entity entity = new Entity
            {
                Type = EntityTypes.FileBundleType,
                Name = node.Attributes["name"].Value,
                OriginalPath = "",
                DestinationPath = "",
                LastUpdated = GetBundleLastUpdate(xmlPath + "/" + node.Attributes["name"].Value),
                XmlPath = xmlPath + "/" + node.Attributes["name"].Value
            };

            return entity;
        }
        private static Entity GetDirectoryEntity(XmlNode node, string xmlPath) {
            Entity entity = new Entity
            {
                Type = EntityTypes.FolderType,
                Name = node.Attributes["name"].Value,
                OriginalPath = node.SelectSingleNode("original-path").InnerText,
                DestinationPath = node.SelectSingleNode("destination-path").InnerText,
                LastUpdated = DateTime.FromFileTime(Int64.Parse(node.SelectSingleNode("last-updated").InnerText)),
                XmlPath = xmlPath + "/" + node.Attributes["name"].Value
            };

            return entity;
        }
        private static Entity GetDirectoryBundleEntity(XmlNode node, string xmlPath) {
            Entity entity = new Entity
            {
                Type = EntityTypes.FolderBundleType,
                Name = node.Attributes["name"].Value,
                OriginalPath = "",
                DestinationPath = "",
                LastUpdated = GetBundleLastUpdate(xmlPath + "/" + node.Attributes["name"].Value),
                XmlPath = xmlPath + "/" + node.Attributes["name"].Value
            };

            return entity;
        }

        public static void AddGroup(string xmlPath, Entity entity) {
            XmlElement group = document.CreateElement("entity");
            group.SetAttribute("type", EntityTypes.GroupType);
            group.SetAttribute("name", entity.Name);

            document.SelectSingleNode(GetXmlPath(xmlPath)).AppendChild(group);

            SaveDocument();
        }
        public static void AddFile(string xmlPath, Entity entity) {
            XmlElement file = GetEntityElement(entity);

            document.SelectSingleNode(GetXmlPath(xmlPath)).AppendChild(file);

            SaveDocument();
        }
        public static void AddFileBundle(string xmlPath, Entity entity, string[] fileNames) {
            string destinationPathTemp = entity.DestinationPath;
            entity.DestinationPath = "";

            XmlElement fileBundle = GetEntityElement(entity);

            entity.DestinationPath = destinationPathTemp;

            for (int i = 0; i < fileNames.Length; i++) {
                XmlElement fileElement;

                Entity entityModel = new Entity(
                                EntityTypes.FileType,
                                Path.GetFileName(fileNames[i]),
                                fileNames[i],
                                entity.DestinationPath,
                                DateTime.MaxValue,
                                entity.XmlPath + "/" + Path.GetFileName(fileNames[i])
                );

                fileElement = GetEntityElement(entityModel);

                fileBundle.AppendChild(fileElement);
            }

            document.SelectSingleNode(GetXmlPath(xmlPath)).AppendChild(fileBundle);

            SaveDocument();
        }
        public static void AddFolder(string xmlPath, Entity entity)
        {
            XmlElement folder = GetEntityElement(entity);

            document.SelectSingleNode(GetXmlPath(xmlPath)).AppendChild(folder);

            SaveDocument();
        }
        public static void AddFolderBundle(string xmlPath, Entity entity, string[] folderNames)
        {
            string destinationPathTemp = entity.DestinationPath;
            entity.DestinationPath = "";

            XmlElement folderBundle = GetEntityElement(entity);

            entity.DestinationPath = destinationPathTemp;

            for (int i = 0; i < folderNames.Length; i++)
            {
                XmlElement fileElement;

                Entity entityModel = new Entity(
                                EntityTypes.FolderType,
                                Path.GetFileName(folderNames[i]),
                                folderNames[i],
                                entity.DestinationPath,
                                DateTime.MaxValue,
                                entity.XmlPath + "/" + Path.GetFileName(folderNames[i])
                );

                fileElement = GetEntityElement(entityModel);

                folderBundle.AppendChild(fileElement);
            }

            document.SelectSingleNode(GetXmlPath(xmlPath)).AppendChild(folderBundle);

            SaveDocument();
        }

        public static void Rename(Entity entity, string name) {
            string path = GetXmlPath(entity.XmlPath);

            document.SelectSingleNode(path).Attributes["name"].Value = name;

            SaveDocument();
        }
        public static void Delete(Entity entity) {
            string path = GetXmlPath(entity.XmlPath);

            XmlNode nodeToDelete = document.SelectSingleNode(path);

            nodeToDelete.ParentNode.RemoveChild(nodeToDelete);

            SaveDocument();
        }
        public static void UpdateDate(Entity entity, DateTime time) {
            string path = GetXmlPath(entity.XmlPath);

            document.SelectSingleNode(path).SelectSingleNode("last-updated").InnerText = time.ToFileTime().ToString();

            SaveDocument();
        }

        private static DateTime GetBundleLastUpdate(string xmlPath) {
            List<Entity> allEntities = GetChildEntities(xmlPath);

            if (allEntities.Count > 0)
            {
                return allEntities.Min(a => a.LastUpdated);
            }
            else {
                return DateTime.MinValue;
            }
        }
        private static XmlElement GetEntityElement(Entity entity) {
            XmlElement entityElement = document.CreateElement("entity");
            entityElement.SetAttribute("type", entity.Type);
            entityElement.SetAttribute("name", entity.Name);

            if (entity.Type != EntityTypes.FileBundleType && entity.Type != EntityTypes.FolderBundleType) {
                XmlElement originalPath = document.CreateElement("original-path");
                originalPath.InnerText = entity.OriginalPath;

                XmlElement destinationPath = document.CreateElement("destination-path");
                destinationPath.InnerText = entity.DestinationPath;

                XmlElement lastUpdated = document.CreateElement("last-updated");
                lastUpdated.InnerText = entity.LastUpdated.ToFileTime().ToString();

                entityElement.AppendChild(originalPath);
                entityElement.AppendChild(destinationPath);
                entityElement.AppendChild(lastUpdated);
            }

            return entityElement;
        }
        private static void SaveDocument() {
            File.WriteAllText(Properties.Settings.Default.PathFile, document.InnerXml);
        }
        private static string GetXmlPath(string path) {
            string xmlPath = "/root";

            string[] allNodes = path.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < allNodes.Length; i++) {
                xmlPath += "/entity[@name=\"" + allNodes[i] + "\"]";
            }

            return xmlPath;
        }

        public static String GetEmptyDocumentXml() {
            XmlDocument emptyDocument = new XmlDocument();
            XmlElement rootElement = emptyDocument.CreateElement("root");

            emptyDocument.AppendChild(rootElement);

            return emptyDocument.InnerXml;
        }
        public static string GetEntityType(string xmlPath) {
            XmlNode node = document.SelectSingleNode(GetXmlPath(xmlPath));

            if (node.Name == "entity")
            {
                if (node.Attributes["type"].Value == "group")
                {
                    return EntityTypes.GroupType;
                }
                else if (node.Attributes["type"].Value == "folder-bundle")
                {
                    return EntityTypes.FolderBundleType;
                }
                else if (node.Attributes["type"].Value == "file-bundle")
                {
                    return EntityTypes.FileBundleType;
                }
                else if (node.Attributes["type"].Value == "folder")
                {
                    return EntityTypes.FolderType;
                }
                else if (node.Attributes["type"].Value == "file")
                {
                    return EntityTypes.FileType;
                }
            }

            if (xmlPath == "") {
                return EntityTypes.GroupType;
            }

            return "";
        }
    }
}
