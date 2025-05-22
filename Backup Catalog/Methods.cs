using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Backup_Catalog
{
    class Methods
    {
        //Check if appropriate name
        public static bool CheckIfAppropriateName(string name, string xmlParent) {
            //Check if null or empty
            bool nullOrEmpty = IfNullOrEmpty(name);

            //Check if name exists
            bool nameExists = NameAlreadyExists(name, xmlParent);

            //Check for illegal characters
            bool illegalCharacters = ContainsIllegalChars(name);

            //Return
            if (nullOrEmpty || nameExists || illegalCharacters)
            {
                //Construct error message
                string text = "";
                if (nullOrEmpty){
                    text += "Name must not be empty.\t\t\t\t";
                }else if (nameExists) {
                    text += "Name already exists.\t\t\t\t\n";
                } else if (illegalCharacters) {
                    text += "Name contains illegal characters. There must not be: & < > ' \"";
                }
                MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            else {
                return true;
            }
        }

        private static bool IfNullOrEmpty(string name)
        {
            return string.IsNullOrEmpty(name.Trim());
        }
        private static bool NameAlreadyExists(string name, string xmlParent) {
            bool nameExists = false;

            List<Entity> entities = XmlMethods.GetChildEntities(xmlParent);
            nameExists = entities.Any(Entity => Entity.Name == name);

            return nameExists;
        }
        private static bool ContainsIllegalChars(string name) {
            bool illegalCharacters = false;
            char[] illegalChars = { '&', '<', '>', '\"', '\'' };
            for (int i = 0; i < illegalChars.Length; i++)
            {
                if (name.Contains(illegalChars[i]))
                {
                    illegalCharacters = true;
                    i = illegalChars.Length;
                }
            }

            return illegalCharacters;
        }

        //Check if path exists
        public static bool CheckIfPathExists(string path, bool file) {
            if (file)
            {
                bool exists = File.Exists(path);

                if (!exists) {
                    MessageBox.Show("File doesn't exist!\t\t\t\t\t\n" + path, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return exists;
            }
            else {
                bool exists = Directory.Exists(path);

                if (!exists)
                {
                    MessageBox.Show("Folder doesn't exist!\t\t\t\t\t\n" + path, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return exists;
            }
        }
    }
}
