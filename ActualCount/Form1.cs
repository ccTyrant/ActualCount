using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace ActualCount
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string content = fileGetContents(txtLink.Text);
                JObject o = JObject.Parse(content);
                int total = (int)o.SelectToken("data.attributes.signature_count");
                foreach (var item in o.SelectToken("data.attributes.signatures_by_country"))
                {
                    if ((string)item.SelectToken("name") == "United Kingdom")
                    {
                        int value = (int)item.SelectToken("signature_count");
                        lblCount.Text = "Actual Count of UK citizens: " + value;
                        double percent = 100 * (double)value / (double)total;
                        lblPercent.Text = "Percentage of signatures that are UK citizens: " + percent + "%";
                        break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Not a valid JSON Link", "Not a valid JSON Link",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected string fileGetContents(string fileName) //This function makes the https connection which is used for accessing the Google API
        {
            string sContents = string.Empty;
            string me = string.Empty;
            try
            {
                if (fileName.ToLower().IndexOf("https:") > -1)
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] response = wc.DownloadData(fileName);
                    sContents = System.Text.Encoding.ASCII.GetString(response);

                }
                else
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                    sContents = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch { sContents = "unable to connect to server "; }
            return sContents;
        }
    }
}
