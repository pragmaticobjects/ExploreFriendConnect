using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jayrock.Json;

using ExploreFriendConnect.Auth;
using ExploreFriendConnect.Conf;
using ExploreFriendConnect.Models;
using ExploreFriendConnect.Http;

namespace ExploreFriendConnect.Util
{
    public class FriendConnectUtil
    {
        public static User getViewer(string fcAuthToken, bool addIfMissing)
        {
            return getFCUser(fcAuthToken, "@viewer", addIfMissing);
        }

        /// <summary>
        /// Gets the FriendConnect user corresponding to fcAuth or userId. If fcAuth
        /// is set, then it will use FriendConnect API calls to retrieve the FriendConnect
        /// user, otherwise it will fetch the user from database using userId, and
        /// then use its fcid along with 2-legged OAuth to fetch the FriendConnect
        /// user.
        /// </summary>
        /// <param name="fcAuthToken">The FriendConnect token.</param>
        /// <param name="userId">The local user id.</param>
        /// <param name="addIfMissing">Should a new user be created in local database if
        ///  there is no such user in the database? Default value of this param is True.
        ///  </param>
        /// <returns>The FriendConnect user.</returns>
        public static User getFCUser(string fcAuthToken, string userId, bool addIfMissing)
        {
            HttpProvider httpProvider =
                (HttpProvider)ObjectFactory.getInstance(StringConstants.HttpProvider);

            string url = "";
            // try to retrieve the user using fcAuth
            if (!string.IsNullOrEmpty(fcAuthToken))
            {
                url = string.Format("{0}/people/{1}/@self?fcauth={2}",
                    ApplicationConfiguration.FriendConnectApiUrl, userId, fcAuthToken);
            }
            else
            {
                // use 2 legged OAuth to fetch the user.
                url = string.Format("{0}/people/{1}/@self",
                    ApplicationConfiguration.FriendConnectApiUrl, userId);

                url = OAuthUtil.signRequest(url, ApplicationConfiguration.FriendConnectOAuthKey,
                    ApplicationConfiguration.FriendConnectOAuthSecret, "",
                    ApplicationConfiguration.FriendConnectSiteId, new Hashtable());
            }

            string response = httpProvider.send(url, "GET", "", null);
            User retVal = null;

            if (!string.IsNullOrEmpty(response))
            {
                JsonTextReader reader = new JsonTextReader(new StringReader(response));
                JsonObject data = new JsonObject();
                data.Import(reader);

                retVal = new User();
                JsonObject entry = (JsonObject)data["entry"];
                retVal.FCId = entry["id"].ToString();
                retVal.Image = entry["thumbnailUrl"].ToString();
                retVal.Name = entry["displayName"].ToString();

                UserDBContext UserDBContext = new Models.UserDBContext();
                string entryId = entry["id"].ToString();
                var users = UserDBContext.Users.Where(u => u.FCId == entryId);                    
                if (users.Count() == 0)
                {
                    // this is a friendconnect user who isn't part of our site. Add him
                    // as a new user. Store only the fcid.
                    if (addIfMissing == true)
                    {
                        User newUser = new User();
                        newUser.FCId = entry["id"].ToString();                        
                        UserDBContext.Users.Add(newUser);
                        UserDBContext.SaveChanges();
                        retVal.Id = UserDBContext.Users.Where(u=> u.FCId ==  newUser.FCId).First().Id;
                    }
                }
                else
                {
                    var localUser = users.First();
                    // this is a registered user, but he has signed up as a FC user. We
                    // could identify him as a local user, or a FC user, based on the
                    // context.
                    retVal.Id = localUser.Id;
                    retVal.Password = localUser.Password;
                }                
            }
            return retVal;
        }

        /// <summary>
        /// Gets the list of viewer friends.
        /// </summary>
        /// <param name="fcAuthToken">The FriendConnect token.</param>
        /// <param name="start">Start index for friend list.</param>
        /// <param name="count">Number of friends to be retrieved.</param>
        /// <returns>An array holding startIndex, count, totalResults, and the
        /// list of friends.</returns>
        public static Hashtable getViewerFriends(string fcAuthToken, int start, int count)
        {
            HttpProvider httpProvider =
                (HttpProvider)ObjectFactory.getInstance(StringConstants.HttpProvider);
            string url = string.Format("{0}/people/@viewer/@friends?fcauth={1}&startIndex={2}" +
              "&count={3}", ApplicationConfiguration.FriendConnectApiUrl, fcAuthToken, start, count);

            string response = httpProvider.send(url, "GET", "", null);

            Hashtable retVal = new Hashtable();

            if (!string.IsNullOrEmpty(response))
            {
                JsonTextReader reader = new JsonTextReader(new StringReader(response));
                JsonObject data = new JsonObject();
                data.Import(reader);

                retVal["startIndex"] = int.Parse(data["startIndex"].ToString());
                retVal["count"] = ((JsonArray)data["entry"]).Length;
                retVal["totalResults"] = int.Parse(data["totalResults"].ToString());

                List<User> users = new List<User>();
                JsonArray entry = (JsonArray)data["entry"];
                for (int i = 0; i < (int)retVal["count"]; i++)
                {
                    JsonObject jUser = (JsonObject)entry[i];
                    User user = new User();
                    user.FCId = jUser["id"].ToString();
                    user.Image = jUser["thumbnailUrl"].ToString();
                    user.Name = jUser["displayName"].ToString();

                    {
                        UserDBContext UserDBContext = new Models.UserDBContext();
                        var localUsers = UserDBContext.Users.Where(u => u.FCId == user.FCId);                        
                        if (localUsers.Count() > 0)
                        {
                            // return as a friend only if this user is registered on our
                            // site as well.
                            user.Id = localUsers.First().Id;
                        }
                        else
                        {
                            // our database is out of sync with the fc database. This should
                            // not happen normally, but syncing itself is simple.

                            User newUser = new User();
                            newUser.FCId = jUser["id"].ToString();
                            UserDBContext.Users.Add(newUser);
                            UserDBContext.SaveChanges();
                            user.Id = UserDBContext.Users.Where(u => u.FCId == newUser.FCId).First().Id;
                        }
                    }

                    {
                        UserDBContext UserDBContext = new Models.UserDBContext();
                        var localUsers = UserDBContext.Users.Where(u => u.FCId == user.FCId);

                        if (localUsers.Count() > 0)
                        {
                            // return as a friend only if this user is registered on our
                            // site as well.
                            user.Id = localUsers.First().Id;
                        }
                        else
                        {
                            // our database is out of sync with the fc database. This should
                            // not happen normally, but syncing itself is simple.

                            User newUser = new User();
                            newUser.FCId = jUser["id"].ToString();

                            UserDBContext.Users.Add(newUser);
                            UserDBContext.SaveChanges();
                            user.Id = UserDBContext.Users.Where(u => u.FCId == newUser.FCId).First().Id;                            
                        }
                        users.Add(user);
                    }
                }
                retVal["friends"] = users.ToArray();
            }
            return retVal;
        }
    }
}