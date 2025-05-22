using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Backup_Catalog
{
    class UpdateEntity
    {
        public static bool EncounteredError = false;

        public static void Update(List<Entity> allEntities) {
            EncounteredError = false;

            for (int i = 0; i < allEntities.Count; i++) {
                if (CheckIfPathsExist(allEntities[i])){
                    Progress.timeAtStart = DateTime.Now;

                    UpdateOneEntity(allEntities[i], i);
                    XmlMethods.UpdateDate(allEntities[i], DateTime.Now);
                }
                else {
                    EncounteredError = true;
                    i = allEntities.Count;
                }
            }
        }
        private static bool CheckIfPathsExist(Entity entity) {
            bool ifFile = entity.Type == EntityTypes.FileType ? true : false;

            if (!Methods.CheckIfPathExists(entity.OriginalPath, ifFile)) {
                return false;
            }
            if (!Methods.CheckIfPathExists(entity.DestinationPath, false)) {
                return false;
            }

            return true;
        }

        private static void UpdateOneEntity(Entity entity, int entityIndex) {
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(entity.OriginalPath);

            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                DeleteDirectory(entity.DestinationPath + @"\" + Path.GetFileName(entity.OriginalPath));
                CopyDirectory(entity.OriginalPath, entity.DestinationPath + @"\" + Path.GetFileName(entity.OriginalPath), entityIndex);
            }
            else {
                DeleteFile(entity.DestinationPath + @"\" + Path.GetFileName(entity.OriginalPath));
                CopyFile(entity.OriginalPath, entity.DestinationPath + @"\" + Path.GetFileName(entity.OriginalPath), entityIndex);
            }
        }

        private static void DeleteDirectory(string path) {
            if (Directory.Exists(path)) {
                string[] dirPaths = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < dirPaths.Length; i++)
                {
                    Directory.Delete(dirPaths[i], true);
                }

                string[] newPaths = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < newPaths.Length; i++)
                {
                    File.Delete(newPaths[i]);
                }
            }
        }
        private static void DeleteFile(string path) {
            if (File.Exists(path)) {
                File.Delete(path);
            }
        }

        private static void CopyDirectory(string path1, string path2, int entityIndex) {
            if (!Directory.Exists(path2)) {
                Directory.CreateDirectory(path2);
            }

            //Now Create all of the directories
            string[] dirPaths = Directory.GetDirectories(path1, "*", SearchOption.AllDirectories);
            for (int i = 0; i < dirPaths.Length; i++)
            {
                Directory.CreateDirectory(dirPaths[i].Replace(path1, path2));
            }

            //Copy all the files & Replaces any files with the same name
            string[] newPaths = Directory.GetFiles(path1, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < newPaths.Length; i++)
            {
                File.Copy(newPaths[i], newPaths[i].Replace(path1, path2), true);
                BaseForm.BackgroundWorker.ReportProgress(i, new object[] { newPaths.Length, entityIndex, Path.GetFileName(newPaths[i]) });
            }
        }
        private static void CopyFile(string path1, string path2, int entityIndex) {
            File.Copy(path1, path2);
            BaseForm.BackgroundWorker.ReportProgress(1, new object[] { 1, entityIndex, Path.GetFileName(path1) });
        }
    }
}
