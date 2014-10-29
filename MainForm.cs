using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xml_valid
{
    public partial class MainForm : Form
    {
        private XmlValid xml;
        public MainForm()
        {
            InitializeComponent();
        }

        private void formLoad(object sender, EventArgs e)
        {
            return;
        }

        private void btnClick(object sender, EventArgs e)
        {
            XmlValid.errorsClear();
            result.Text = "";

            xml = new XmlValid(ref field);
            string[] response = xml.Errors;

            log.Text = "Ошибок" + ((response.Length != 0) ? ": " + response.Length : " нет");
            
            for (int i = 0; i < response.Length; i++)
            {
                result.Text += ">> " + response[i] + "\n";
            }
            XmlValid.fill(ref result, 0, result.Text.Length , Color.Red);
        }
    }
}
