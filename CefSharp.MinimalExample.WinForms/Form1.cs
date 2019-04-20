using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;
using CefSharp.MinimalExample.WinForms.Controls;

namespace CefSharp.MinimalExample.WinForms
{
    public partial class Form1 : Form
    {
        public Form1(string url, int[] position)
        {
            ChromiumWebBrowser browser;
            InitializeComponent();
            Text = "大屏展示";
            SetDesktopLocation(position[0], position[1]);
            browser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill,
            };
        }
    }
}
