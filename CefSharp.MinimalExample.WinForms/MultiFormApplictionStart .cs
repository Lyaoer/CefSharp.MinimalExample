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

namespace CefSharp.MinimalExample.WinForms
{
    class MultiFormApplictionStart : ApplicationContext
    {
        public const string GIT_URL = "http://git.fzyun.io/api/v4/projects/491/repository/files/current%2Fconfig%2Eyml/raw?ref=monitor-test";
        public const string PRIVATE_TOKEN = "9UqYy7kdvgAnEi2AhPJ_";
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
            YamlSequenceNode yaml = GetYAMLFromGit();
            //窗口个数
            int number = GetClientNumber(yaml);
            
            //启动窗口
            var formList = new List<Form>();
            int i = 0;
            while (i < number)
            {
                //第N个窗口的信息
                string url = GetURL(yaml, i);
                int[] position = GetPosition(yaml, i);
                var browser = new BrowserForm(url, position);
                //var browser = new Form1(url, position);
                Size size = new Size(position[2], position[3]);
                browser.Size = size;
                formList.Add(browser);
                i++;
            }
            /*
             *里面添加启动的窗口
             */
            foreach (var item in formList)
            {
                item.FormClosed += onFormClosed;
            }
            foreach (var item in formList)
            {
                item.Show();
            }

        }
        private YamlSequenceNode GetYAMLFromGit()
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
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("clients")];
            
            return items;

        }
        private int GetClientNumber(YamlSequenceNode yaml)
        {
            return yaml.Children.Count;
        }
        private string GetURL(YamlSequenceNode yaml, int index)
        {
            YamlMappingNode mapping = (YamlMappingNode)yaml.Children[index];
            string url = (String)mapping.Children[new YamlScalarNode("url")];
            return url;
        }
        private int[] GetPosition(YamlSequenceNode yaml, int index)
        {
            int[] position = new int[4];
            YamlMappingNode mapping = (YamlMappingNode)yaml.Children[index];
            var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("position")];
            int i = 0;
            foreach (YamlScalarNode item in items)
            {
                position[i] = int.Parse(item.Value);
                i++;
            }
            return position;
        }
    }
}
