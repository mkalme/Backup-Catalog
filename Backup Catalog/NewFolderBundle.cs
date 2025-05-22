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
    public partial class NewFolderBundle : Form
    {
        public static BaseForm baseForm;

        private static readonly List<string> FolderNames = new List<string>();

        public NewFolderBundle()
        {
            InitializeComponent();

            FolderNames.Clear();
            SetStyle();
        }

        private void BrowseFolderBundleButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            FolderNames.Add(folderBrowserDialog1.SelectedPath);

            Load_DataGridView();
        }

        private void SetStyle()
        {
            //DataGridView
            dataGridView1.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dataGridView1.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            dataGridView1.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            dataGridView1.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
        }
        private void Load_DataGridView() {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < FolderNames.Count; i++) {
                dataGridView1.Rows.Add(Properties.Resources.Folder, FolderNames[i]);
                dataGridView1.Rows[i].Cells[1].ToolTipText = FolderNames[i];
            }

            dataGridView1.ClearSelection();
            label3.Text = "Number of folders: " + FolderNames.Count;
        }

        private void DestinationPathBrowseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            destinationPathTextBox.Text = folderBrowserDialog1.SelectedPath;
        }

        private void RemoveFromListLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string path = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();

                FolderNames.RemoveAt(FolderNames.IndexOf(path));

                Load_DataGridView();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (CheckAllFolders() &&
                Methods.CheckIfPathExists(destinationPathTextBox.Text, false) &&
                Methods.CheckIfAppropriateName(nameTextBox.Text, BaseForm.PathToList(Storage.XmlPath)))
            {
                AddFolderBundle();
            }
        }
        private bool CheckAllFolders()
        {
            bool allExist = true;

            for (int i = 0; i < FolderNames.Count; i++)
            {
                bool exist = Methods.CheckIfPathExists(FolderNames[i], false);

                if (!exist)
                {
                    allExist = false;
                    i = FolderNames.Count;
                }
            }

            return allExist;
        }

        private void AddFolderBundle() {
            Entity entity = new Entity(
                EntityTypes.FolderBundleType,
                nameTextBox.Text,
                "",
                destinationPathTextBox.Text,
                new DateTime(),
                BaseForm.PathToList(Storage.XmlPath) + "/" + nameTextBox.Text
            );

            XmlMethods.AddFolderBundle(BaseForm.PathToList(Storage.XmlPath), entity, FolderNames.ToArray());
            baseForm.Load_View();

            Close();
        }
    }
}
