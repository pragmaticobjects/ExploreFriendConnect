using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;

namespace ExploreFriendConnect.Conf
{
    public sealed class ApplicationConfiguration : IConfigurationSectionHandler
    {
        /// <summary>
        /// Static constructor for the object.
        /// </summary>
        static ApplicationConfiguration()
        {
            Object obj = ConfigurationManager.GetSection("explorefriendconnect");
        }

        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public Object Create(Object parent, object configContext, XmlNode section)
        {
            NameValueCollection settings;
            try
            {
                NameValueSectionHandler baseHandler = new NameValueSectionHandler();
                settings = (NameValueCollection)baseHandler.Create(parent,
                    configContext, section);
            }
            catch
            {
                settings = null;
            }

            if (settings != null)
            {
                // Read Application settings from configuration file.
                friendConnectLibVersion = ReadSetting(settings, FRIENDCONNECT_LIB_VERSION, "");
                friendConnectSiteId = ReadSetting(settings, FRIENDCONNECT_SITE_ID, "");
                friendConnectCookie = ReadSetting(settings, FRIENDCONNECT_COOKIE, "");
                mysqlConnectionString = ReadSetting(settings, MYSQL_CONNECTION_STRING, "");
                yahooSearchKey = ReadSetting(settings, YAHOO_SEARCH_KEY, "");
                yahooSearchUrl = ReadSetting(settings, YAHOO_SEARCH_URL, "");
                googleMapsKey = ReadSetting(settings, GOOGLE_MAPS_KEY, "");
                friendConnectOAuthKey = ReadSetting(settings, FRIENDCONNECT_OAUTH_KEY, "");
                friendConnectOAuthSecret = ReadSetting(settings, FRIENDCONNECT_OAUTH_SECRET, "");
                friendConnectApiUrl = ReadSetting(settings, FRIENDCONNECT_API_URL, "");
                googleMapsApiUrl = ReadSetting(settings, GOOGLEMAPS_API_URL, "");
            }
            return null;
        }

        /// <summary>
        /// Reads a setting from a given NameValueCollection, and sets
        /// default value if the key is not available in the collection.
        /// </summary>
        /// <param name="settings">The settings collection from which the keys
        /// are to be read.</param>
        /// <param name="key">Key name to be read.</param>
        /// <param name="defaultValue">Default value for the key.</param>
        /// <returns>Actual value from settings, or defaultValue if settings
        /// does not have this key.</returns>
        internal static String ReadSetting(NameValueCollection settings,
            String key, String defaultValue)
        {
            if (settings == null || key == null)
                throw new ArgumentNullException();
            try
            {
                Object setting = settings[key];
                return (setting == null) ? defaultValue : (String)setting;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// FriendConnect site id.
        /// </summary>
        public static String FriendConnectLibVersion
        {
            get
            {
                return ApplicationConfiguration.friendConnectLibVersion;
            }
        }

        /// <summary>
        /// Gets the FriendConnect site id.
        /// </summary>
        public static String FriendConnectSiteId
        {
            get
            {
                return ApplicationConfiguration.friendConnectSiteId;
            }
        }

        /// <summary>
        /// Gets the FriendConnect site id. This is usually fcAuth + FriendConnectSiteId or
        /// fcAuth_s + FriendConnectSiteId.
        /// </summary>
        public static String FriendConnectCookie
        {
            get
            {
                return ApplicationConfiguration.friendConnectCookie;
            }
        }

        /// <summary>
        /// Gets the MySql connection string for website.
        /// </summary>
        public static String MySQLConnectionString
        {
            get
            {
                return ApplicationConfiguration.mysqlConnectionString;
            }
        }

        /// <summary>
        /// Gets the Developer key for Yahoo! Local Search API.
        /// </summary>
        public static String YahooSearchKey
        {
            get
            {
                return ApplicationConfiguration.yahooSearchKey;
            }
        }

        /// <summary>
        /// Gets the Yahoo! Local Search API endpoint.
        /// </summary>
        public static String YahooSearchUrl
        {
            get
            {
                return ApplicationConfiguration.yahooSearchUrl;
            }
        }

        /// <summary>
        /// Gets the Developer key for Google maps API.
        /// </summary>
        public static String GoogleMapsKey
        {
            get
            {
                return ApplicationConfiguration.googleMapsKey;
            }
        }

        /// <summary>
        /// Gets OAuth key for doing 2-legged OAuth calls to FriendConnect REST endpoints.
        /// </summary>
        public static String FriendConnectOAuthKey
        {
            get
            {
                return ApplicationConfiguration.friendConnectOAuthKey;
            }
        }

        /// <summary>
        /// Gets OAuth secret for doing 2-legged OAuth calls to FriendConnect REST endpoints.
        /// </summary>
        public static String FriendConnectOAuthSecret
        {
            get
            {
                return ApplicationConfiguration.friendConnectOAuthSecret;
            }
        }

        /// <summary>
        /// Gets the FriendConnect API url.
        /// </summary>
        public static String FriendConnectApiUrl
        {
            get
            {
                return ApplicationConfiguration.friendConnectApiUrl;
            }
        }

        /// <summary>
        /// Gets the Google Maps API url.
        /// </summary>
        public static String GoogleMapsApiUrl
        {
            get
            {
                return ApplicationConfiguration.googleMapsApiUrl;
            }
        }

        /// <summary>
        /// FriendConnect lib version
        /// </summary>
        private static String friendConnectLibVersion;

        /// <summary>
        /// FriendConnect site id.
        /// </summary>
        private static String friendConnectSiteId;

        /// <summary>
        ///FriendConnect site id. This is usually fcAuth + FriendConnectSiteId or
        /// fcAuth_s + FriendConnectSiteId.
        /// </summary>
        private static String friendConnectCookie;

        /// <summary>
        /// MySql connection string for website.
        /// </summary>
        private static String mysqlConnectionString;

        /// <summary>
        /// Developer key for Yahoo! Local Search API.
        /// </summary>
        private static String yahooSearchKey;

        /// <summary>
        /// Yahoo! Local Search API endpoint.
        /// </summary>
        private static String yahooSearchUrl;

        /// <summary>
        /// Developer key for Google maps API.
        /// </summary>
        private static String googleMapsKey;

        /// <summary>
        /// OAuth key for doing 2-legged OAuth calls to FriendConnect REST endpoints.
        /// </summary>
        private static String friendConnectOAuthKey;

        /// <summary>
        /// OAuth secret for doing 2-legged OAuth calls to FriendConnect REST endpoints.
        /// </summary>
        private static String friendConnectOAuthSecret;

        /// <summary>
        /// FriendConnect API url.
        /// </summary>
        private static String friendConnectApiUrl;

        /// <summary>
        /// Google Maps API url.
        /// </summary>
        private static String googleMapsApiUrl;

        /// <summary>
        /// Key name in app.config for FriendConnect site id.
        /// </summary>
        internal const String FRIENDCONNECT_LIB_VERSION = "FriendConnectLibVersion";        

        /// <summary>
        /// Key name in app.config for FriendConnect site id.
        /// </summary>
        internal const String FRIENDCONNECT_SITE_ID = "FriendConnectSiteId";

        /// <summary>
        /// Key name in app.config for FriendConnect cookie.
        /// </summary>
        internal const String FRIENDCONNECT_COOKIE = "FriendConnectCookie";

        /// <summary>
        /// Key name in app.config for MySql connection string.
        /// </summary>
        internal const String MYSQL_CONNECTION_STRING = "MySQLConnectionString";

        /// <summary>
        /// Key name in app.config for Yahoo Search API developer tokne.
        /// </summary>
        internal const String YAHOO_SEARCH_KEY = "YahooSearchKey";

        /// <summary>
        /// Key name in app.config for Yahoo Search API end point.
        /// </summary>
        internal const String YAHOO_SEARCH_URL = "YahooSearchUrl";

        /// <summary>
        /// Key name in app.config for Google maps API end point.
        /// </summary>
        internal const String GOOGLE_MAPS_KEY = "GoogleMapsKey";

        /// <summary>
        /// Key name in app.config for FriendConnect OAuth key.
        /// </summary>
        internal const String FRIENDCONNECT_OAUTH_KEY = "FriendConnectOAuthKey";

        /// <summary>
        /// Key name in app.config for FriendConnect OAuth secret.
        /// </summary>
        internal const String FRIENDCONNECT_OAUTH_SECRET = "FriendConnectOAuthSecret";

        /// <summary>
        /// Key name in app.config for FriendConnect API url.
        /// </summary>
        internal const String FRIENDCONNECT_API_URL = "FriendConnectApiUrl";

        /// <summary>
        /// Key name in app.config for Google Maps API url.
        /// </summary>
        internal const String GOOGLEMAPS_API_URL = "GoogleMapsApiUrl";
    }
}