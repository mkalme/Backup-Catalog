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
    public partial class Progress : Form
    {
        public static DateTime timeAtStart = new DateTime();
        public static string stringToUse = "Updating";

        private static bool paused = false;

        public Progress()
        {
            InitializeComponent();

            loadingLabel.Text = stringToUse;
            timer1.Interval = 600;
            timer1.Start();

            timeAtStart = DateTime.Now;
            timer2.Interval = 500;
            timer2.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (loadingLabel.Text.Length == stringToUse.Length){
                loadingLabel.Text = stringToUse + ".";
                Text = stringToUse + ".";
            }else if (loadingLabel.Text.Length == stringToUse.Length + 1){
                loadingLabel.Text = stringToUse + "..";
                Text = stringToUse + "..";
            }else if (loadingLabel.Text.Length == stringToUse.Length + 2){
                loadingLabel.Text = stringToUse + "...";
                Text = stringToUse + "...";
            }else{
                loadingLabel.Text = stringToUse;
                Text = stringToUse;
            }
        }
        private void Timer2_Tick(object sender, EventArgs e)
        {
            TimeSpan timeSpan = DateTime.Now - timeAtStart;

            if (timeSpan.TotalSeconds >= 1)
            {
                double progress = (double)progressBar1.Maximum / (double)progressBar1.Value;

                TimeSpan estimatedTimeSpan = TimeSpan.FromTicks((long)(
                    ((double)timeSpan.Ticks * progress) *
                    (double)((double)(progressBar1.Maximum - progressBar1.Value) / (double)progressBar1.Maximum)
                ));

                string stringFormat = "";

                if (estimatedTimeSpan.TotalHours >= 1)
                {
                    stringFormat = "{0:%h} hours, {0:%m} minutes and {0:%s} seconds.";
                }
                else if (estimatedTimeSpan.TotalMinutes >= 1)
                {
                    stringFormat = "{0:%m} minutes and {0:%s} seconds.";
                }
                else if (estimatedTimeSpan.TotalSeconds > 0)
                {
                    stringFormat = "{0:%s} seconds.";
                }

                estimatedTimeLabel.Text = "Estimated time left: " + string.Format(stringFormat, estimatedTimeSpan);
            }
        }

        private void PauseResumeButtom_Click(object sender, EventArgs e)
        {
            if (!paused){//If running
                paused = true;

            }
            else {//If paused
                paused = false;


            }
        }
    }
}
