namespace Base.Api.Data.Managers.Interfaces
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    public interface ICryptoAuthorizeManager
    {
        /// <summary>
        /// Gets the PrefixHeader key.
        /// </summary>
        string PrefixHeaderKey { get; set; }

        /// <summary>
        /// Gets the WebServices name.
        /// </summary>
        string WebServicesName { get; set; }
        
        /// <summary>
        /// Gets the Request time validity.
        /// </summary>
        double RequestTimeValidity { get; set; }
        
        /// <summary>
        /// Gets the string/char value used for separate sts values.
        /// </summary>
        string ValueRequestSeparator { get; set; }

        /// <summary>
        /// Compare the client signature with signature generated with server.
        /// </summary>
        /// <param name="signatureRequest"></param>
        /// <param name="secretClientKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        bool CompareSignatures(string signatureRequest, string secretClientKey, HttpRequestMessage request);

        /// <summary>
        /// Generate hash with base logic encryption.
        /// </summary>
        /// <param name="secretClientKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        string GenerateBaseSignature(string secretClientKey, HttpRequestMessage request);

        /// <summary>
        /// Get from headers the attribute 'timestamp' with the specific prefix value.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        string GetTimestampHeader(HttpRequestHeaders headers);

        /// <summary>
        /// Split applicationKey & signature.
        /// </summary>
        /// <param name="authorizationHeader"></param>
        /// <returns></returns>
        string[] SplitAuthorizationHearder(string authorizationHeader);

        /// <summary>
        /// Check if the request is valid in time.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        bool CheckRequestTimeStampValidity(string timestamp);
    }
}