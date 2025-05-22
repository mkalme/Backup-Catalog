using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Backup_Catalog
{
    public partial class Rename : Form
    {
        public static BaseForm BaseForm;
        public static Entity Entity;

        public Rename(BaseForm baseForm, Entity entity)
        {
            InitializeComponent();

            BaseForm = baseForm;
            Entity = entity;
        }

        private void Rename_Load(object sender, EventArgs e)
        {
            SetStyle();
        }

        private void SetStyle() {
            Point location = new Point(6, 86);
            string renameText = "Name";

            if (Entity.Type == EntityTypes.GroupType)
            {
                pictureBox1.BackgroundImage = Properties.Resources.Group;
                location = new Point(6, 86);
                renameText = "Rename group";
            }
            else if (Entity.Type == EntityTypes.FileType)
            {
                pictureBox1.BackgroundImage = Properties.Resources.File;
                location = new Point(6, 90);
                renameText = "Rename file";
            }
            else if (Entity.Type == EntityTypes.FileBundleType)
            {
                pictureBox1.BackgroundImage = Properties.Resources.FileBundle;
                location = new Point(6, 90);
                renameText = "Rename file bundle";
            }
            else if (Entity.Type == EntityTypes.FolderType)
            {
                pictureBox1.BackgroundImage = Properties.Resources.Folder;
                location = new Point(7, 92);
                renameText = "Rename folder";
            }
            else if (Entity.Type == EntityTypes.FolderBundleType)
            {
                pictureBox1.BackgroundImage = Properties.Resources.FolderBundle;
                location = new Point(13, 87);
                renameText = "Rename folder bundle";
            }

            pictureBox1.Location = location;

            Text = renameText;
            renameLabel.Text = renameText;

            nameTextBox.Text = Entity.Name;
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            if (Methods.CheckIfAppropriateName(nameTextBox.Text, BaseForm.PathToList(Storage.XmlPath))) {
                XmlMethods.Rename(Entity, nameTextBox.Text);

                BaseForm.Load_View();

                Close();
            }
        }
    }
}
