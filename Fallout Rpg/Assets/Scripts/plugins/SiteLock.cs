namespace WebUtilities {
    using UnityEngine;
    using System.Text.RegularExpressions;
    public static class SiteLock {
        /// <summary>
        /// Creates a site lock. If your executable gets stolen and uploaded at an unauthorized site, the application
        /// will cause the offending webpage to redirect to a specified webpage. It is best to call this function as soon as possible in your application.
        /// If the protocol is not "http://" or "https://", the application will automatically be redirected.
        /// </summary>
        /// <param name="redirect">The full URL to redirect the webpage to (usually a website with a legal version or a security notice hosted on your own domain).</param>
        /// <param name="acceptedDomains">A list of domains where this application is allowed to be run. ("chat.kongregate.com", "www.mydomain.com", etc...). If no accepted domains are provided,
        /// the document will instantly be redirected to the redirect URL.</param>
        /// <returns><c>True</c> if the domain is accepted, <c>false</c> otherwise.</returns>
        public static bool Create(string redirect, params string[] acceptedDomains) {
            //usage
            //SiteLock.Create("http://www.mydomain.com", "www.mydomain.com", "chat.kongregate.com", "assets.kongregate.com");

            if (Application.isWebPlayer) {
                if (acceptedDomains.Length > 0) {
                    string host = Application.webSecurityHostUrl;
                    string pattern = "(http)[s]?(://)[^/]+";
                    Match match = Regex.Match(host, pattern);

                    if (!match.Success)
                        Application.ExternalEval("window.top.location='" + redirect + "';");

                    string result = match.Value;
                    if (result.StartsWith("https"))
                        result = result.Substring(8);
                    else if (result.StartsWith("http"))
                        result = result.Substring(7);

                    foreach (string domain in acceptedDomains) {
                        if (result == domain)
                            return true;
                    }

                    Application.ExternalEval("window.top.location='" + redirect + "';");
                } else {
                    Application.ExternalEval("window.top.location='" + redirect + "';");
                }
            }
            return false;
        }
    }
}