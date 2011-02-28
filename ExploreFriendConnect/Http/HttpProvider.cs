using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExploreFriendConnect.Http
{
    public abstract class HttpProvider
    {
        const string USER_AGENT = "chowdown-php 1.0";

        /// <summary>
        /// Sends an HTTP request to the server.
        /// </summary>
        /// <param name="url">The request url.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="postBody">The POST body, if the method is POST. Ignored if method is
        /// not POST.</param>
        /// <param name="headers">The HTTP header parameters as an array.</param>
        /// <returns>The response as a string, or empty string if response can't be obtained.</returns>
        public abstract string send(string url, string method, string postBody, string[] headers);
    }
}