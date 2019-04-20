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
        //private readonly ChromiumWebBrowser browser;
        //public static string url_bak = "";
        //public static string url = "kb.fzyun.io";
        //public static int flag = 0;

        //public const string GIT_URL = "http://git.fzyun.io/api/v4/projects/491/repository/files/current%2Fconfig%2Eyml/raw?ref=monitor-test";
        //public const string PRIVATE_TOKEN = "9UqYy7kdvgAnEi2AhPJ_";

        public BrowserForm(string url, int[] position)
        {
            ChromiumWebBrowser browser;
            InitializeComponent();

            Text = "大屏展示";
            //WindowState = FormWindowState.Maximized;
            SetDesktopLocation(position[0], position[1]);
            browser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill,
            };
            toolStripContainer.ContentPanel.Controls.Add(browser);
            //browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            //browser.StatusMessage += OnBrowserStatusMessage;
            //browser.TitleChanged += OnBrowserTitleChanged;
        }
        /*
        private void changeURL()
        {
            url_bak = url;
            url = GetSTHFromGit();
            if (url != url_bak)
            {
                LoadUrl(url);
            }
            Console.WriteLine("OK, test event is fired at: " + DateTime.Now.ToString());
        }
        private string GetSTHFromGit()
        {
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(GIT_URL);
            request.Method = "GET";
            request.Headers.Add("PRIVATE-TOKEN", PRIVATE_TOKEN);
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            var responseStream = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
            var responseStr = new StringReader(responseStream);
            var yaml = new YamlStream();
            yaml.Load(responseStr);
            
            var mapping =(YamlMappingNode)yaml.Documents[0].RootNode;
            var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("clients")];
            //var rtg1 = items.Children[new YamlScalarNode("part_no")];
            //var rtg2 = items.Children[new YamlScalarNode("descrip")];

            string[] sArray = responseStream.Split(new char[2] { ' ', '\n' });
            return sArray[11];

        }
        */
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

        //private void ExitMenuItemClick(object sender, EventArgs e)
        //{
        //    browser.Dispose();
        //    Cef.Shutdown();
        //    Close();
        //}

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
        }

        //private void LoadUrl(string url)
        //{
        //    if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
        //    {
        //        browser.Load(url);
        //    }
        //}

        //private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
        //{
        //    browser.ShowDevTools();
        //}
    }
}
