#region

using System.Windows.Forms;

#endregion

namespace Superfish_Removal_Tool.Forms
{
    public partial class BrowsersForm : Form
    {
        public BrowsersForm()
        {
            InitializeComponent();
        }

        private void CheckRunningBrowsers()
        {
            listBox1.Items.Clear();

            var browsers = BrowserUtilities.GetRunningBrowsers();

            if (browsers.Count == 0)
                Close();

            foreach (var browser in browsers)
            {
                listBox1.Items.Add(browser.Name);
            }
        }

        private void BrowserTimer_Tick(object sender, System.EventArgs e)
        {
            CheckRunningBrowsers();
        }
    }
}