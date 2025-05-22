using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backup_Catalog
{
    public partial class NewFolder : Form
    {
        public static BaseForm baseForm;

        public NewFolder()
        {
            InitializeComponent();
        }

        private void OriginalBrowseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            originalPathTextBox.Text = folderBrowserDialog1.SelectedPath;
        }
        private void DestinationBrowseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            destinationPathTextBox.Text = folderBrowserDialog1.SelectedPath;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (Methods.CheckIfPathExists(originalPathTextBox.Text, false) &&
                Methods.CheckIfPathExists(destinationPathTextBox.Text, false) &&
                Methods.CheckIfAppropriateName(nameTextBox.Text, BaseForm.PathToList(Storage.XmlPath)))
            {
                AddFolder();
            }
        }

        private void AddFolder() {
            Entity entity = new Entity(
                EntityTypes.FolderType,
                nameTextBox.Text,
                originalPathTextBox.Text,
                destinationPathTextBox.Text,
                DateTime.MaxValue,
                BaseForm.PathToList(Storage.XmlPath) + "/" + nameTextBox.Text
            );

            XmlMethods.AddFolder(BaseForm.PathToList(Storage.XmlPath), entity);
            baseForm.Load_View();

            Close();
        }
    }
}
