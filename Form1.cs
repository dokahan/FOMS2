using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace FOMSSubmarine
{
    public partial class Form1 : Form
    {

        Excel.Application oXL = null; //
        Excel.Workbook oWB = null; //
        Excel.Worksheet oSheet = null; //
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WebBrowser.Navigate(new Uri("http://daum.net"));    // OTDR Access Master

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (oXL != null)
            {
                button3_Click(null, null);
            }
            oXL = new Excel.Application();
            oXL.Visible = true;
            oWB = oXL.Workbooks.Open("a.xlxs", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            oSheet = (Excel.Worksheet)oWB.ActiveSheet;
            oSheet.Range[1].Value = string.Format("{0:#0.00#}", 111);
            oWB.Save();
            oXL.Quit();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (oXL != null)
            {
                button3_Click(null, null);
            }
            oXL = new Excel.Application();
            oXL.Visible = true;
            oWB = oXL.Workbooks.Add();
            oWB.SaveAs("a1.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            oWB = oXL.Workbooks.Open("a1.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            oSheet = (Excel.Worksheet)oWB.ActiveSheet;
            oSheet.Range["A2"].Value = DateTime.Now.ToString("yyyy-MM-dd");
            oWB.SaveAs("a1.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //oXL.Quit();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            oWB.Close();
            oXL.Quit();

            Marshal.ReleaseComObject(oWB);
            Marshal.ReleaseComObject(oXL);
            oWB = null;
            oXL = null;
        }
    }
}
