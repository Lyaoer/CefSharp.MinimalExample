// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.MinimalExample.WinForms.Controls;
using CefSharp.WinForms;
using System.Timers;
using System.Net;
using System.Text;
using YamlDotNet.RepresentationModel;
using System.IO;

namespace CefSharp.MinimalExample.WinForms
{
    public partial class BrowserForm : Form
    {
        private readonly ChromiumWebBrowser browser;
        public static string url_bak = "";
        public static string url = "kb.fzyun.io";
        public static int flag = 0;

        public const string GIT_URL = "http://git.fzyun.io/api/v4/projects/491/repository/files/current%2Fconfig%2Eyml/raw?ref=monitor-test";
        public const string PRIVATE_TOKEN = "9UqYy7kdvgAnEi2AhPJ_";

        public BrowserForm()
        {
            InitializeComponent();

            Text = "大屏展示";
            WindowState = FormWindowState.Maximized;
            //var url = "172.19.210.25/index/amcharts?env=stage";
            //var url = "cicd.dev.fzyun.io/index/three?env=try";
            //var url = "www.baidu.com";
            browser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill,
            };
            toolStripContainer.ContentPanel.Controls.Add(browser);

            browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            this.ShowIcon = false;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;
            timer.Tick += delegate (object o, EventArgs args)
            {
                changeURL();
            };
            timer.Start();

        }

        private void changeURL()
        {
            url_bak = url;
            url = GetUrlFromGit();
            if (url != url_bak)
            {
                LoadUrl(url);
            }
            //Console.WriteLine("OK, test event is fired at: " + DateTime.Now.ToString());
        }
        private string GetUrlFromGit()
        {
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(GIT_URL);
            request.Method = "GET";
            request.Headers.Add("PRIVATE-TOKEN", PRIVATE_TOKEN);
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            var responseText = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
            string[] sArray = responseText.Split(new char[2] { ' ', '\n' });
            return sArray[11];

        }
        private void OnIsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            if(e.IsBrowserInitialized)
            {
                var b = ((ChromiumWebBrowser)sender);

                this.InvokeOnUiThreadIfRequired(() => b.Focus());
            }
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Text = args.Title);
        }

        public void DisplayOutput(string output)
        {
            this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
            Close();
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            browser.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e)
        {
            browser.Forward();
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
        }

        private void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                browser.Load(url);
            }
        }

        private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }
    }
}
