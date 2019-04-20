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
using System.Drawing;

namespace CefSharp.MinimalExample.WinForms
{
    public partial class BrowserForm : Form
    {
        private readonly ChromiumWebBrowser browser;
        public BrowserForm(int i, string url, int[] position)
        {
            InitializeComponent();

            Text = url;
            //WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //窗体的位置由Location属性决定
            this.StartPosition = FormStartPosition.Manual;
            //窗体的起始位置 
            this.Location = (Point)new Size(position[0], position[1]);
            //SetDesktopLocation(position[0], position[1]);
            //X为宽度，Y为高度
            this.ClientSize = new System.Drawing.Size(position[2], position[3]);
            browser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill,
            };
            toolStripContainer.ContentPanel.Controls.Add(browser);
            //browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            //browser.StatusMessage += OnBrowserStatusMessage;
            //browser.TitleChanged += OnBrowserTitleChanged;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;
            timer.Tick += delegate (object o, EventArgs args)
            {
                YamlSequenceNode yaml = MyTool.GetYAMLFromGit();
                string new_url = MyTool.GetURL(yaml, i);
                int number = MyTool.GetClientNumber(yaml);
                int[] new_position = MyTool.GetPosition(yaml, i);
                if (new_url == null) {
                    browser.Dispose();
                    Cef.Shutdown();
                    Close();
                } else if (!url.Equals(new_url))
                {
                    changeURL(new_url);
                    url = new_url;
                } else if (position[0] != new_position[0] && position[1] != new_position[1]) {
                    this.Location = (Point)new Size(new_position[0], new_position[1]);
                    position[0] = new_position[0];
                    position[1] = new_position[1];
                } else if (position[2] != new_position[2] && position[3] != new_position[3]) {
                    this.ClientSize = new System.Drawing.Size(new_position[2], new_position[3]);
                    position[2] = new_position[2];
                    position[3] = new_position[3];
                }
            };
            timer.Start();
        }
        
        private void changeURL(string url)
        {
            LoadUrl(url);
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
        
        //private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
        //{
        //    browser.ShowDevTools();
        //}
    }
}
