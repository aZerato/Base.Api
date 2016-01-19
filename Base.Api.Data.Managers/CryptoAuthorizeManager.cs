namespace Base.Api.Data.Managers
{
    using Crosscutting.Core.Configuration;
    using Crosscutting.Core.Exceptions.Types;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// CryptoAuthorizeManager class.
    /// </summary>
    public class CryptoAuthorizeManager : ICryptoAuthorizeManager
    {
        #region properties

        private ISettingsApi settingsApi;

        #endregion

        #region attributes

        /// <summary>
        /// Gets the PrefixHeader key.
        /// </summary>
        private string _prefixHeaderKey;
        public string PrefixHeaderKey
        {
            get
            {
                if (String.IsNullOrEmpty(this._prefixHeaderKey))
                {
                    this._prefixHeaderKey = this.settingsApi.ApplicationSettings.GlobalSettings.PrefixHeaderKey;
                }
                return this._prefixHeaderKey;
            }
            set
            {
                this._prefixHeaderKey = value;
            }
        }

        /// <summary>
        /// Gets the WebServices name.
        /// </summary>
        private string _webServicesName;
        public string WebServicesName
        {
            get
            {
                if (String.IsNullOrEmpty(this._webServicesName))
                {
                    this._webServicesName = this.settingsApi.ApplicationSettings.GlobalSettings.WebServicesName;
                }
                return this._webServicesName;
            }
            set
            {
                this._webServicesName = value;
            }
        }

        /// <summary>
        /// Gets the Request time validity.
        /// </summary>
        private double? _requestTimeValidity;
        public double RequestTimeValidity
        {
            get
            {
                if (this._requestTimeValidity == null)
                {
                    double requestTimeValidityC = 60; // default 60 sec
                    Double.TryParse(this.settingsApi.ApplicationSettings.GlobalSettings.RequestTimeValidity, out requestTimeValidityC);

                    this._requestTimeValidity = requestTimeValidityC;
                }

                return this._requestTimeValidity.Value;
            }
            set
            {
                this._requestTimeValidity = value;
            }
        }

        /// <summary>
        /// Gets the string/char value used for separate sts values.
        /// </summary>
        private string _valueRequestSeparator;
        public string ValueRequestSeparator
        {
            get
            {
                if (this._valueRequestSeparator == null)
                {
                    this._valueRequestSeparator = this.settingsApi.ApplicationSettings.GlobalSettings.ValueRequestSeparator;
                }

                return this._valueRequestSeparator;
            }
            set
            {
                this._valueRequestSeparator = value;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// Initiaze a new instance of CryptoAuthorizeManager class.
        /// </summary>
        /// <param name="settingsApi"></param>
        public CryptoAuthorizeManager(ISettingsApi settingsApi)
        {
            this.settingsApi = settingsApi;
        }

        #endregion constructor

        #region publics

        /// <summary>
        /// Compare the client signature with signature generated with server.
        /// </summary>
        /// <param name="signatureRequest"></param>
        /// <param name="secretClientKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool CompareSignatures(string signatureRequest, string secretClientKey, HttpRequestMessage request)
        {
            bool correct = false;

            string baseSignature = this.GenerateBaseSignature(secretClientKey, request);

            // Compare hash
            if (signatureRequest == baseSignature)
            {
                correct = true;
            }

            return correct;
        }

        /// <summary>
        /// Generate hash with base logic encryption.
        /// </summary>
        /// <param name="secretClientKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateBaseSignature(string secretClientKey, HttpRequestMessage request)
        {
            string signatureServeur = string.Empty;

            // Server : base hash
            string canonicalizedmwsHeaders = this.GetCanonicalizedHeaders(request.Headers);
            string sts = StringToSign(request, canonicalizedmwsHeaders);

            // hmacSha1 : base64
            signatureServeur = GenerateHashSha1(sts, secretClientKey);

            return signatureServeur;
        }

        /// <summary>
        /// Get from headers the attribute 'timestamp' with the specific prefix value.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public string GetTimestampHeader(HttpRequestHeaders headers)
        {
            string prefixHeaderKey = this.PrefixHeaderKey;

            string timestamp = string.Empty;
            IEnumerable<string> timestampHeader = null;
            headers.TryGetValues(prefixHeaderKey + "timestamp", out timestampHeader);

            if (timestampHeader != null)
            {
                timestamp = timestampHeader.ElementAt(0);
            }
            
            return timestamp;
        }

        /// <summary>
        /// Split applicationKey & signature.
        /// </summary>
        /// <param name="authorizationHeader"></param>
        /// <returns></returns>
        public string[] SplitAuthorizationHearder(string authorizationHeader)
        {
            string publicKeyWithSignature = authorizationHeader.Replace(this.WebServicesName + " ", "");

            return publicKeyWithSignature.Split(':');
        }


        /// <summary>
        /// Check if the request is valid in time.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public bool CheckRequestTimeStampValidity(string timestamp)
        {
            bool result = false;
            try
            {
                double tsD = 0;
                Double.TryParse(timestamp, out tsD);

                if (tsD != 0)
                {
                    DateTime validity = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(tsD);

                    if (validity >= DateTime.UtcNow.AddSeconds(-this.RequestTimeValidity) &&
                        validity <= DateTime.UtcNow.AddSeconds(this.RequestTimeValidity))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new RequestTimeException("Invalid timestamp. " + ex.Message);
            }

            return result;
        }

        #endregion publics

        #region privates

        /// <summary>
        /// Generate the canonicalized from request header keys.
        /// </summary>
        /// <param name="currHeaders"></param>
        /// <returns></returns>
        private string GetCanonicalizedHeaders(HttpRequestHeaders currHeaders)
        {
            string prefixHeaderKey = this.PrefixHeaderKey;
            string canonicalizedmwsHeaders = "";

            IEnumerable<KeyValuePair<string, IEnumerable<string>>> wsHeaderKeys = currHeaders
                .Where(k => k.Key.StartsWith(prefixHeaderKey, System.StringComparison.OrdinalIgnoreCase))
                .OrderBy(k => k.Key.ToLower())
                .ToArray();

            string previouskey = "";
            foreach (var key in wsHeaderKeys)
            {
                if (key.Key == previouskey)
                {
                    canonicalizedmwsHeaders += "," + key.Value.ElementAtOrDefault(0);
                }
                else
                {
                    canonicalizedmwsHeaders += this.ValueRequestSeparator +
                                                key.Key.ToLower() + ":" + key.Value.ElementAtOrDefault(0);
                }
                previouskey = key.Key;
            }

            return canonicalizedmwsHeaders;
        }

        /// <summary>
        /// Generate StringToSign.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="canonicalizedmwsHeaders"></param>
        /// <returns></returns>
        private string StringToSign(HttpRequestMessage request, string canonicalizedmwsHeaders)
        {
            string verb = request.Method.Method;

            // 'content' attributes in header are not directly in request.headers but in request.content.headers
            string contentType = string.Empty;
            MediaTypeHeaderValue contentTypeValue = request.Content.Headers.ContentType;
            if (contentTypeValue != null)
            {
                contentType = contentTypeValue.ToString();
            }

            var sts = verb + this.ValueRequestSeparator +
                    contentType +
                    canonicalizedmwsHeaders;

            return sts;
        }

        /// <summary>
        /// Generate hmac-sha1 convert to base64 string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GenerateHashSha1(string value, string key)
        {
            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));
            }
        }

        #endregion privates
    }
}