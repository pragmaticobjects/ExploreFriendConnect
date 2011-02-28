using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace ExploreFriendConnect.Http
{
    public class DefaultHttpProvider : HttpProvider
    {
        public override string send(string url, string method, string postBody, string[] headers)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method;

            if (method == "POST")
            {
                Stream outStream = request.GetRequestStream();
                byte[] contents = Encoding.UTF8.GetBytes(postBody);
                outStream.Write(contents, 0, contents.Length);
                outStream.Close();
                request.ContentLength = contents.Length;
            }

            if (headers != null)
            {
                foreach (string header in headers)
                {
                    request.Headers.Add(header);
                }
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader inStream = new StreamReader(response.GetResponseStream());
                string contents = inStream.ReadToEnd();
                inStream.Close();
                return contents;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}