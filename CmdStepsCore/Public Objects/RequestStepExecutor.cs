using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CmdStepsCore
{
    public class RequestStepExecutor : IStepExecutor
    {
        public async Task<string> ExecuteAsync(string input, Dictionary<string, string> parameters)
        {
            string url = string.Format("{0}{1}{2}", input, input.Contains("?") ? "&" : "?", GetQueryParams(parameters));
            return await GetResponseAsync(url);
        }
        private string GetQueryParams(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0) return string.Empty;
            var str = new StringBuilder();
            int i = 0;

            foreach (var item in parameters)
            {
                if (i++ == 0) str.Append("&");
                str.AppendFormat("{0}={1}", item.Key, Uri.EscapeDataString(item.Value));
            }
            return str.ToString();
        }
        private async Task<string> GetResponseAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string received;

            using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        received = await sr.ReadToEndAsync();
                    }
                }
            }

            return received;
        }
    }
}
