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
    public partial class NewFile : Form
    {
        public static BaseForm baseForm;

        public NewFile()
        {
            InitializeComponent();
        }

        private void BrowseOriginalButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            originalPathTextBox.Text = openFileDialog1.FileName;
        }

        private void BrowseDestinationButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            destinationPathTextBox.Text = folderBrowserDialog1.SelectedPath;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (Methods.CheckIfPathExists(originalPathTextBox.Text, true) &&
                Methods.CheckIfPathExists(destinationPathTextBox.Text, false) &&
                Methods.CheckIfAppropriateName(nameTextBox.Text, BaseForm.PathToList(Storage.XmlPath)))
            {
                AddFile();
            }
        }

        private void AddFile() {
            Entity entity = new Entity(
                EntityTypes.FileType,
                nameTextBox.Text,
                originalPathTextBox.Text,
                destinationPathTextBox.Text,
                DateTime.MaxValue,
                BaseForm.PathToList(Storage.XmlPath) + "/" + nameTextBox.Text
            );

            XmlMethods.AddFile(BaseForm.PathToList(Storage.XmlPath), entity);
            baseForm.Load_View();

            Close();
        }
    }
}
