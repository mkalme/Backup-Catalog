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
    public partial class NewFileBundle : Form
    {
        public static BaseForm baseForm;
        private static List<string> FileNames = new List<string>();

        public NewFileBundle()
        {
            InitializeComponent();

            FileNames.Clear();
            SetStyle();
        }

        private void BrowseFileBundleButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            FileNames = openFileDialog1.FileNames.ToList();

            Load_DataGridView();
        }
        private void DestinationPathBrowseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            destinationPathTextBox.Text = folderBrowserDialog1.SelectedPath;
        }

        private void SetStyle() {
            //DataGridView
            dataGridView1.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dataGridView1.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            dataGridView1.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            dataGridView1.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
        }
        private void Load_DataGridView() {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < FileNames.Count; i++) {
                dataGridView1.Rows.Add(Properties.Resources.File, FileNames[i]);

                dataGridView1.Rows[i].Cells[1].ToolTipText = FileNames[i];
            }

            dataGridView1.ClearSelection();
            label3.Text = "Number of files: " + FileNames.Count;
        }

        private void RemoveFromListLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0){
                string path = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();

                FileNames.RemoveAt(FileNames.IndexOf(path));

                Load_DataGridView();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (CheckAllFiles() &&
                Methods.CheckIfPathExists(destinationPathTextBox.Text, false) &&
                Methods.CheckIfAppropriateName(nameTextBox.Text, BaseForm.PathToList(Storage.XmlPath)))
            {
                AddFileBundle();
            }
        }
        private bool CheckAllFiles() {
            bool allExist = true;

            for (int i = 0; i < FileNames.Count; i++) {
                bool exist = Methods.CheckIfPathExists(FileNames[i], true);

                if (!exist) {
                    allExist = false;
                    i = FileNames.Count;
                }
            }

            return allExist;
        }

        private void AddFileBundle() {
            Entity entity = new Entity(
                EntityTypes.FileBundleType,
                nameTextBox.Text,
                "",
                destinationPathTextBox.Text,
                new DateTime(),
                BaseForm.PathToList(Storage.XmlPath) + "/" + nameTextBox.Text
            );

            XmlMethods.AddFileBundle(BaseForm.PathToList(Storage.XmlPath), entity, FileNames.ToArray());
            baseForm.Load_View();

            Close();
        }
    }
}
