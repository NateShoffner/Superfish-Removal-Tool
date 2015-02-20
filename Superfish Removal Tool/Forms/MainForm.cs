#region

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Superfish_Removal_Tool.Removers;

#endregion

namespace Superfish_Removal_Tool.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (BrowserUtilities.GetRunningBrowsers().Count > 0)
            {
                using (var bd = new BrowsersForm())
                {
                    bd.ShowDialog();
                }
            }

            btnStart.Enabled = false;

            StartRemoval();

            btnStart.Enabled = true;
        }

        private void StartRemoval()
        {
            var certificateRemover = new CertificateRemover();
            var serviceRemover = new ServiceRemover();
            var mozillaRemover = new MozillaRemover();

            var removers = new List<ISuperfishRemover> { certificateRemover, serviceRemover, mozillaRemover };
            progressBar1.Maximum = removers.Count;
            var failedRemovals = 0;

            //populate task list
            foreach (var remover in removers)
            {
                var lvi = new ListViewItem {Text = remover.Name};
                lvi.SubItems.Add(remover.NeedsRemoved() ? "Needs Removed" : "Ready");
                listView1.Items.Add(lvi);
            }

            for (var i = 0; i < removers.Count; i++)
            {
                var remover = removers[i];

                if (remover.NeedsRemoved())
                {
                    var result = remover.Remove();

                    if (!result.Succeeded)
                        failedRemovals++;

                    listView1.Items[i].SubItems[1].Text = result.Succeeded ? "Removed!" : string.Format("Failed: {0}", result.Error.Message);
                }

                progressBar1.Value += 1;
            }

            if (failedRemovals > 0)
                MessageBox.Show("There was an error during the removal process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("The removal process has completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}