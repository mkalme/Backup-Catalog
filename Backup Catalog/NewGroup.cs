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
    public partial class NewGroup : Form
    {
        public static BaseForm baseForm;

        public NewGroup()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (Methods.CheckIfAppropriateName(nameTextBox.Text, BaseForm.PathToList(Storage.XmlPath)))
            {
                Entity entity = new Entity
                {
                    Name = nameTextBox.Text,
                    XmlPath = BaseForm.PathToList(Storage.XmlPath) + "/" + nameTextBox.Text
                };

                XmlMethods.AddGroup(BaseForm.PathToList(Storage.XmlPath), entity);

                baseForm.Load_View();

                Close();
            }
        }
    }
}
