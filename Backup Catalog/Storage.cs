using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Backup_Catalog
{
    class Storage
    {
        public static List<string> XmlPath = new List<string>();

        public static Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();
        public static string CurrentType = EntityTypes.GroupType;
        public static string SelectedEntity = "";

        public static List<string> AllPaths = new List<string>() { "" };
        public static List<string> AllSelectedPaths = new List<string>() { "" };
        public static int CurrentIndex = 0;

        //Entities
        public static Entity GetSelectedEntity()
        {
            return Entities[SelectedEntity];
        }

        //AllPaths
        public static void AddItemToPathList(string path)
        {
            if (CurrentIndex < AllPaths.Count)
            {
                if (AllPaths[CurrentIndex] != path)
                {
                    AllPaths.RemoveRange(CurrentIndex, AllPaths.Count - CurrentIndex);
                    AllSelectedPaths.RemoveRange(CurrentIndex, AllSelectedPaths.Count - CurrentIndex);

                    AllPaths.Add(path);
                    AllSelectedPaths.Add("");
                }
            }else{
                AllPaths.Add(path);
                AllSelectedPaths.Add("");
            }
        }
        public static void SetCurrentPathListItem(string text)
        {
            AllSelectedPaths[CurrentIndex] = text;
        }
    }
}
