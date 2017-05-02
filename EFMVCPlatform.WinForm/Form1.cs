using DevExpress.XtraBars.Alerter;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFMVCPlatform.WinForm
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        DevExpress.XtraBars.Docking.DockPanel dp;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadGridData();

        }
        
        private void LoadGridData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("clm1");
            dt.Columns.Add("clm2");
            dt.Columns.Add("clm3");
            dt.Columns.Add("clm4");
            dt.Columns.Add("clm5");

            dt.Rows.Add("11", "12", "ww", "14", "15");
            dt.Rows.Add("21", "22", "ww", "24", "25");
            dt.Rows.Add("31", "32", "we", "34", "35");
            dt.Rows.Add("41", "42", "we", "44", "45");
            dt.Rows.Add("51", "52", "we", "54", "55");

            gridControl1.DataSource = dt;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int num = Convert.ToInt32(this.textBox1.Text);
            Business.SystemManagerBLL bll = new Business.SystemManagerBLL();
            bll.TestResultData(num);
            //for (int i = 0; i < 1000; i++)
            //{
            //    bll.TestResultData();
            //}

            ConfigurationManager.AppSettings.Set("", "");
            //ConfigurationManager.OpenExeConfiguration()

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.AppSettings.Settings[""] = "";
            config.Save();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Business.SystemManagerBLL bll = new Business.SystemManagerBLL();
            bll.AbordThread();
        }
    }
    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
}
