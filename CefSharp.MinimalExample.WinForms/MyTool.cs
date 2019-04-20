using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using System.Net;
using System.IO;

namespace CefSharp.MinimalExample.WinForms
{
    class MyTool
    {
        public const string GIT_URL = "http://git.fzyun.io/api/v4/projects/491/repository/files/current%2Ftest%2Eyml/raw?ref=monitor-test";
        public const string PRIVATE_TOKEN = "9UqYy7kdvgAnEi2AhPJ_";
        public static YamlSequenceNode GetYAMLFromGit()
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
        public static int GetClientNumber(YamlSequenceNode yaml)
        {
            return yaml.Children.Count;
        }
        public static string GetClientId(YamlSequenceNode yaml, int index)
        {
            YamlMappingNode mapping = (YamlMappingNode)yaml.Children[index];
            string clientId = (String)mapping.Children[new YamlScalarNode("client")];
            return clientId;
        }
        public static string GetURL(YamlSequenceNode yaml, int index)
        {
            int count = GetClientNumber(yaml);
            if (index < count)
            {
                YamlMappingNode mapping = (YamlMappingNode)yaml.Children[index];
                string url = (String)mapping.Children[new YamlScalarNode("url")];
                return url;
            }
            else {
                return "null";
            }
            
        }
        public static int[] GetPosition(YamlSequenceNode yaml, int index)
        {
            int[] position = new int[4];
            int count = GetClientNumber(yaml);
            if (index < count)
            {
                YamlMappingNode mapping = (YamlMappingNode)yaml.Children[index];
                var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("position")];
                int i = 0;
                foreach (YamlScalarNode item in items)
                {
                    position[i] = int.Parse(item.Value);
                    i++;
                }
            }
            return position;
        }
    }
}
