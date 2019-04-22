using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.RepresentationModel;
using System.Net;
using CefSharp.WinForms;
using System.IO;
using System.Drawing;
using System.Threading;

namespace CefSharp.MinimalExample.WinForms
{
    class MultiFormApplictionStart : ApplicationContext
    {
        private void onFormClosed(object sender, EventArgs e)
        {
            if (Application.OpenForms.Count == 0)
            {
                ExitThread();
            }
        }
        public MultiFormApplictionStart()
        {
            //从git中获取yaml文件
            YamlSequenceNode yaml = MyTool.GetYAMLFromGit();
            //窗口个数
            int number = MyTool.GetClientNumber(yaml);
            //启动窗口
            var formList = new List<Form>();
            int i = 0;
            while (i < number)
            {
                newForm(yaml,i,formList);
                i++;
            }
            //里面添加启动的窗口
            foreach (var item in formList)
            {
                item.FormClosed += onFormClosed;
            }
            foreach (var item in formList)
            {
                item.Show();
            }

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;
            timer.Tick += delegate (object o, EventArgs args)
            {
                YamlSequenceNode new_yaml = MyTool.GetYAMLFromGit();
                int new_number = MyTool.GetClientNumber(new_yaml);
                if (new_number > number) {
                    int j = number;
                    while (j < new_number)
                    {
                        newForm(new_yaml, j, formList);
                        formList[j].Show();
                        j++;
                    }
                } else if (new_number < number) {
                    int j = number;
                    while (j<new_number) {
                        int last = formList.Count;
                        formList.RemoveAt(last-1);
                        formList[last - 1].Close();
                    }
                }
            };
            timer.Start();
        }
        private void newForm(YamlSequenceNode yaml, int i, List<Form> formList) {
            //第N个窗口的信息
            string url = MyTool.GetURL(yaml, i);
            int[] position = MyTool.GetPosition(yaml, i);
            var browser = new BrowserForm(i, url, position);
            //var browser = new Form1(url, position);
            //Size size = new Size(position[2], position[3]);
            //browser.Size = size;
            formList.Add(browser);
        }
        private void changeBrowser(List<Form> formList, int number) {
            //从git中获取yaml文件
            YamlSequenceNode yaml = MyTool.GetYAMLFromGit();
            //窗口个数
            int newNumber = MyTool.GetClientNumber(yaml);
            int i = 0;
            foreach (var item in formList) {
                string new_url = MyTool.GetURL(yaml, i);
                int[] new_position = MyTool.GetPosition(yaml, i);
                if (!new_url.Equals(item.Text)) {
                    //item.FormClosed += onFormClosed;
                }
                i++;
            }
        }
    }
}
