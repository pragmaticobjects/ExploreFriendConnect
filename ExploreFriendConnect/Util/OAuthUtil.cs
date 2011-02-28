using System;
using System.Text;
using System.Collections;
using System.Security.Cryptography;

namespace ExploreFriendConnect.Util
{
    public class OAuthUtil
    {
        /// <summary>
        /// Sign an OAuth request.
        /// </summary>
        /// <param name="url">The request url.</param>
        /// <param name="consumerKey">OAuth consumer key for signing purposes.</param>
        /// <param name="consumerSecret">OAuth consumer secret for signing purposes.</param>
        /// <param name="tokenSecret">OAuth token secret for signing purposes.</param>
        /// <param name="userId">OAuth requestor id for signing and authentication purposes.</param>
        /// <param name="extraParams">The additional parameters to be appended to the url before 
        /// signing the request.</param>
        /// <returns>A signed request, compatible with OAuth Spec, HMAC signature.</returns>
        public static string signRequest(string url, string consumerKey, string consumerSecret,
            string tokenSecret, string userId, Hashtable extraParams)
        {

            string combinedSecret = myUrlEncode(consumerSecret) + "&" + myUrlEncode(tokenSecret);
            HMACSHA1 signer = new HMACSHA1(new ASCIIEncoding().GetBytes(combinedSecret));

            SortedList dict = new SortedList();

            dict.Add("oauth_consumer_key", consumerKey);
            dict.Add("oauth_nonce", Guid.NewGuid().ToString());
            dict.Add("oauth_signature_method", "HMAC-SHA1");
            // convert ticks to epoch()
            dict.Add("oauth_timestamp",
                ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString());
            dict.Add("oauth_version", "1.0");
            dict.Add("xoauth_requestor_id", userId);

            foreach (string paramName in extraParams.Keys)
            {
                dict.Add(paramName, (string)extraParams[paramName]);
            }

            string paramString = "";

            foreach (string paramKey in dict.Keys)
            {
                paramString += paramKey + "=" + myUrlEncode((string)dict[paramKey]) + "&";
            }

            paramString = paramString.Trim('&');
            string methodName = "GET";

            string urlToSign = methodName + "&" + myUrlEncode(url) + "&" + myUrlEncode(paramString);
            byte[] dataBuffer = Encoding.ASCII.GetBytes(urlToSign);
            byte[] signeddata = signer.ComputeHash(dataBuffer);
            string signature = Convert.ToBase64String(signeddata);

            return url + "?" + paramString + "&oauth_signature=" + myUrlEncode(signature);
        }

        /// <summary>
        /// Performs a OAuth url encoding. This is the same as HttpUtility.UrlEncode(), except that
        /// all hex escaping are done in uppercase, as required by OAuth.
        /// </summary>
        /// <param name="url">Url to be encoded.</param>
        /// <returns>The encoded url.</returns>
        private static string myUrlEncode(string url)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < url.Length; i++)
            {
                if (char.IsLetterOrDigit(url[i]) || url[i] == '_' || url[i] == '-' ||
                    url[i] == '.' || url[i] == '~')
                {
                    builder.Append(url[i]);
                }
                else
                {
                    builder.AppendFormat("%{0:X2}", (int)url[i]);
                }
            }
            return builder.ToString();
        }
    }
}