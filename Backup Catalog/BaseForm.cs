using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Backup_Catalog
{
    public partial class BaseForm : Form
    {
        public static BackgroundWorker BackgroundWorker = new BackgroundWorker();
        private static Progress progress;

        private static List<Entity> entitiesToUpdate = new List<Entity>();

        public BaseForm()
        {
            InitializeComponent();

            contextMenuStrip1.Renderer = new MyRenderer();

            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;  //Tell the user how the process went

            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.WorkerSupportsCancellation = true; //Allow for the process to be cancelled
        }

        private void Base_Load(object sender, EventArgs e)
        {
            SetPath();

            CreateBase();

            SetStyle();

            Load_View();

            timer1.Start();
        }

        //Background Worker
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Get all values
            int progressBarMax = (int)((object[])e.UserState)[0];
            int currentEntityIndex = (int)((object[])e.UserState)[1];
            string fileName = (string)((object[])e.UserState)[2];

            //Set Progress bar
            if (progress.progressBar1.Maximum != progressBarMax) {
                progress.progressBar1.Maximum = progressBarMax;
            }
            progress.progressBar1.Value = e.ProgressPercentage;

            //Percentage label
            progress.percentageLabel.Text = e.ProgressPercentage + " / " + progressBarMax;

            //Progress string to use
            Progress.stringToUse = "Updating (" + (currentEntityIndex + 1) + "/" + entitiesToUpdate.Count + ")";

            //File label
            progress.fileLabel.Text = fileName;
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateEntity.Update(entitiesToUpdate);
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Progress.stringToUse = "Updating";
            progress.fileLabel.Text = "";
            progress.percentageLabel.Text = "";
            progress.progressBar1.Value = 0;

            entitiesToUpdate.Clear();

            if (!UpdateEntity.EncounteredError) {
                MessageBox.Show("Update successfully completed!\t\t\t\t", "Update", MessageBoxButtons.OK);
            }

            progress.Close();
        }

        //Set
        private void SetPath() {
            Properties.Settings.Default.PathFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Backup Catalog v1\Storage";
            Properties.Settings.Default.PathFile = Properties.Settings.Default.PathFolder + @"\Backups.xml";
        }
        private void CreateBase() {
            if (!Directory.Exists(Properties.Settings.Default.PathFolder)) {
                Directory.CreateDirectory(Properties.Settings.Default.PathFolder);
            }

            if (!File.Exists(Properties.Settings.Default.PathFile)) {
                using (StreamWriter sw = File.CreateText(Properties.Settings.Default.PathFile)){
                    sw.WriteLine(XmlMethods.GetEmptyDocumentXml());
                }
            }
        }
        private void SetStyle() {
            //DataGridView
            dataGridView1.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dataGridView1.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            dataGridView1.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            dataGridView1.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

            //Button
            leftArrowButton.FlatAppearance.BorderColor = BackColor;
            rightArrowButton.FlatAppearance.BorderColor = BackColor;

            refreshButton.FlatAppearance.BorderColor = BackColor;
        }

        //Load
        public void Load_View() {
            ignoreSelection = true;

            dataGridView1.Rows.Clear();

            Storage.Entities.Clear();
            List<Entity> allEntities = XmlMethods.GetChildEntities(PathToList(Storage.XmlPath));

            for (int i = 0; i < allEntities.Count; i++) {
                Storage.Entities.Add(i.ToString(), allEntities[i]);

                dataGridView1.Rows.Add(
                    i,
                    GetImage(allEntities[i]),
                    allEntities[i].Name,
                    allEntities[i].OriginalPath,
                    allEntities[i].DestinationPath,
                    GetTimeSpan(allEntities[i])
                );

                dataGridView1.Rows[i].Cells[2].ToolTipText = allEntities[i].Name;
                dataGridView1.Rows[i].Cells[3].ToolTipText = allEntities[i].OriginalPath;
                dataGridView1.Rows[i].Cells[4].ToolTipText = allEntities[i].DestinationPath;
            }

            allEntities.Clear();
            dataGridView1.ClearSelection();

            ignoreSelection = false;

            SelectRowBasedOnValue();

            Storage.CurrentType = XmlMethods.GetEntityType(PathToList(Storage.XmlPath));

            pathTextBox.Text = GetTextBoxPath();
        }
        private void SelectRowBasedOnValue() {
            for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == Storage.AllSelectedPaths[Storage.CurrentIndex]) {
                    dataGridView1.Rows[i].Selected = true;
                }
            }
        }

        private Image GetImage(Entity entity) {
            if (entity.Type == EntityTypes.GroupType) {
                return Properties.Resources.Group;
            } else if (entity.Type == EntityTypes.FileType) {
                return Properties.Resources.File;
            } else if (entity.Type == EntityTypes.FileBundleType) {
                return Properties.Resources.FileBundle;
            } else if (entity.Type == EntityTypes.FolderType) {
                return Properties.Resources.Folder;
            } else {
                return Properties.Resources.FolderBundle;
            }
        }
        private string GetTimeSpan(Entity entity) {
            TimeSpan timeSpan = DateTime.Now - entity.LastUpdated;

            if (entity.LastUpdated > DateTime.MinValue && entity.LastUpdated < DateTime.MaxValue)
            {
                if (timeSpan.TotalSeconds < 60)
                {
                    int totalSeconds = (int)Math.Round(timeSpan.TotalSeconds);

                    if (totalSeconds == 1){
                        return totalSeconds + " second ago";
                    }else{
                        return totalSeconds + " seconds ago";
                    }
                }
                if (timeSpan.TotalSeconds < 3600)
                {
                    int totalMinutes = (int)Math.Round(timeSpan.TotalMinutes);

                    if (totalMinutes == 1){
                        return totalMinutes + " minute ago";
                    }else{
                        return totalMinutes + " minutes ago";
                    }
                }
                if (timeSpan.TotalSeconds < 86400)
                {
                    int totalHours = (int)Math.Round(timeSpan.TotalHours);

                    if (totalHours == 1){
                        return totalHours + " hour ago";
                    }else{
                        return totalHours + " hours ago";
                    }
                }
                if (timeSpan.TotalSeconds >= 86400)
                {
                    int totalDays = (int)Math.Round(timeSpan.TotalDays);

                    if (totalDays == 1){
                        return totalDays + " day ago";
                    }else{
                        return totalDays + " days ago";
                    }
                }
            }

            if (entity.LastUpdated == DateTime.MaxValue) {
                return "Never";
            }

            return "";
        }

        //Scripts
        public static string PathToList(List<string> paths) {
            return String.Join("/", paths.ToArray());
        }
        private static string GetTextBoxPath()
        {
            return " " + String.Join(" > ", Storage.XmlPath.ToArray());
        }

        //Action
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1) {
                string type = Storage.GetSelectedEntity().Type;

                if (type == EntityTypes.GroupType || type == EntityTypes.FolderBundleType || type == EntityTypes.FileBundleType) {
                    //Add to path
                    Storage.XmlPath.Add(Storage.GetSelectedEntity().Name);

                    //All Paths & All Selected Paths
                    Storage.CurrentIndex++;
                    Storage.AddItemToPathList(Storage.GetSelectedEntity().Name);

                    //Load
                    Load_View();
                }
            }
        }
        private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dataGridView1.HitTest(e.X, e.Y);
            if (hit.Type != DataGridViewHitTestType.Cell)
            {
                dataGridView1.ClearSelection();
            }
        }

        private static bool ignoreSelection = false;
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!ignoreSelection) {
                //Update selected entity
                if (dataGridView1.SelectedRows.Count > 0) {
                    Storage.SelectedEntity = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                } else {
                    Storage.SelectedEntity = "";
                }

                //Update selected entity
                if (dataGridView1.SelectedRows.Count > 0) {
                    string path = Storage.GetSelectedEntity().Name;
                    Storage.SetCurrentPathListItem(path);
                } else {
                    Storage.SetCurrentPathListItem("");
                }
            }
        }

        //Left-Right / Refresh Buttons
        private void LeftArrowButton_Click(object sender, EventArgs e)
        {
            if (Storage.XmlPath.Count > 0) {
                Storage.XmlPath.RemoveAt(Storage.XmlPath.Count - 1);
                Storage.CurrentIndex--;

                Load_View();
            }
        }
        private void RightArrowButton_Click(object sender, EventArgs e)
        {
            if (Storage.CurrentIndex + 1 < Storage.AllPaths.Count) {
                Storage.CurrentIndex++;

                Storage.XmlPath.Add(Storage.AllPaths[Storage.CurrentIndex]);
                Load_View();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //Left-Button
            if (Storage.XmlPath.Count > 0)
            {
                leftArrowButton.Enabled = true;
            }
            else {
                leftArrowButton.Enabled = false;
            }

            //Right-Button
            if (Storage.CurrentIndex < Storage.AllPaths.Count - 1)
            {
                rightArrowButton.Enabled = true;
            }
            else {
                rightArrowButton.Enabled = false;
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Load_View();
        }

        //Context Menu Strip
        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ToolStripMenuItem[] menuItems = {updateToolStripMenuItem, renameToolStripMenuItem, deleteToolStripMenuItem,
                                             groupToolStripMenuItem, fileToolStripMenuItem, fileBundleToolStripMenuItem,
                                            folderToolStripMenuItem, folderBundleToolStripMenuItem};

            bool[] menuItemsEnabled = {};

            bool threeEnabled = false;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                threeEnabled = true;
            }
            else {
                threeEnabled = false;
            }

            if (Storage.CurrentType == EntityTypes.GroupType){
                menuItemsEnabled = new bool[] { threeEnabled, threeEnabled, threeEnabled, true, true, true, true, true };
            }
            else if (Storage.CurrentType == EntityTypes.FileBundleType){
                menuItemsEnabled = new bool[] { threeEnabled, threeEnabled, threeEnabled, false, true, false, false, false };
            }
            else if (Storage.CurrentType == EntityTypes.FolderBundleType){
                menuItemsEnabled = new bool[] { threeEnabled, threeEnabled, threeEnabled, false, false, false, true, false };
            }

            if (dataGridView1.SelectedRows.Count > 0){
                if (dataGridView1.SelectedRows[0].Cells[0].Value.ToString() == EntityTypes.GroupType){
                    menuItemsEnabled[0] = false;
                }else{
                    menuItemsEnabled[0] = true;
                }
            }else {
                menuItemsEnabled[0] = false;
            }

            for (int i = 0; i < menuItems.Length; i++) {
                menuItems[i].Enabled = menuItemsEnabled[i];
            }
        }

        private void GroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGroup newGroup = new NewGroup();
            NewGroup.baseForm = this;
            newGroup.ShowDialog();
        }
        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile newFile = new NewFile();
            NewFile.baseForm = this;
            newFile.ShowDialog();
        }
        private void FileBundleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFileBundle newFileBundle = new NewFileBundle();
            NewFileBundle.baseForm = this;
            newFileBundle.ShowDialog();
        }
        private void FolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFolder newFolder = new NewFolder();
            NewFolder.baseForm = this;
            newFolder.ShowDialog();
        }
        private void FolderBundleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFolderBundle newFolderBundle = new NewFolderBundle();
            NewFolderBundle.baseForm = this;
            newFolderBundle.ShowDialog();
        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedType = Storage.GetSelectedEntity().Type;

            if (selectedType == EntityTypes.FileType || selectedType == EntityTypes.FolderType) {
                UpdateSingleEntity(Storage.GetSelectedEntity());

                Load_View();
            } else if (selectedType == EntityTypes.FileBundleType || selectedType == EntityTypes.FolderBundleType) {
                UpdateEntityBundle(Storage.GetSelectedEntity());

                Load_View();
            }
        }
        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rename rename = new Rename(this, Storage.GetSelectedEntity());
            rename.ShowDialog();
        }
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this?\t\t\t\t", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                XmlMethods.Delete(Storage.GetSelectedEntity());

                Load_View();
            }
        }

        private class MyRenderer : ToolStripProfessionalRenderer
        {
            public MyRenderer() : base(new MyColors()) { }
        }
        private class MyColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.Gray; }
            }

            public override Color MenuItemBorder
            {
                get { return Color.Gray; }
            }
        }

        //Update
        private void UpdateSingleEntity(Entity entity) {
            StartUpdate(new List<Entity> { entity });
        }
        private void UpdateEntityBundle(Entity entity) {
            List<string> tempList = new List<string>(Storage.XmlPath){ entity.Name };
            string path = PathToList(tempList);

            StartUpdate(XmlMethods.GetChildEntities(path));
        }

        private void StartUpdate(List<Entity> entities) {
            if (entities.Count > 0)
            {
                entitiesToUpdate = entities;

                BackgroundWorker.RunWorkerAsync();

                progress = new Progress();
                progress.progressBar1.Maximum = 1;
                progress.ShowDialog();
            }
            else {
                MessageBox.Show("Backup is empty.\t\t\t\t\t\t", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
