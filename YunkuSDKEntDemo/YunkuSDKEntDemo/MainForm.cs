using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YunkuEntSDK;

namespace YunkuSDKEntDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        const string UESRNAME = "Brandon";
        const string PASSWORD = "123456";
        const string CLIENT_ID = "b2b54fa4261f9cf5e4772e6359f96161";
        const string CLIENT_SECRET = "134dba8e0adc4e59b511c09aa1ebf71e";

        private void StartTest_Click(object sender, EventArgs e)
        {
            YunkuEngine yunku = new YunkuEngine();
        }
    }
}
