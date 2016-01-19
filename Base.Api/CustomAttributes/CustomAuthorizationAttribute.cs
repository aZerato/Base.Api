namespace Base.Api.CustomAttributes
{
    using Crosscutting.Core.Exceptions.Types;
    using Crosscutting.Ioc;
    using Data.Managers.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class CustomAuthorizationAttribute : AuthorizationFilterAttribute
    {
        #region properties

        private ICryptoAuthorizeManager cryptoAuthorizeManager;

        #endregion properties

        #region constructor

        public CustomAuthorizationAttribute()
        {
            this.cryptoAuthorizeManager = IocUnityContainer.Instance.Resolve<ICryptoAuthorizeManager>();
        }

        #endregion constructor

        #region override publics

        /// <summary>
        /// Handle function on authorization request.
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (AuthorizeRequest(actionContext))
            {
                return;
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        #endregion publics overrided

        #region protected

        /// <summary>
        /// Unauthorize request.
        /// </summary>
        /// <param name="filterContext"></param>
        protected void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        #endregion protected

        #region privates

        /// <summary>
        /// Perform authorization request.
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool AuthorizeRequest(HttpActionContext actionContext)
        {
            // Actions with no CustomAuthorizationAttribute && controllers not too OR just with [AllowAnonymous] attribute.
            if (
                (
                    actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<CustomAuthorizationAttribute>().Any() == false &&
                    actionContext.ActionDescriptor.GetCustomAttributes<CustomAuthorizationAttribute>().Any() == false
                )
                ||
                actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() == true
            )
            {
                return true;
            }

            // Check the authorization header
            IEnumerable<string> authorizationKey = null;
            actionContext.Request.Headers.TryGetValues("Authorization", out authorizationKey);

            if (authorizationKey == null)
            {
                // need authorization key/signature
                throw new RequestParameterException("No Authorization signature specified in headers");
            }
            else
            {
                // Check the timestamp
                string timestamp = cryptoAuthorizeManager.GetTimestampHeader(actionContext.Request.Headers);
                if (String.IsNullOrEmpty(timestamp))
                {
                    // need to specify an timestamp attribute in header
                    throw new RequestParameterException("No Timestamp specified in headers");
                }

                string[] splited = cryptoAuthorizeManager.SplitAuthorizationHearder(authorizationKey.ElementAt(0));
                string applicationKeyRequest = splited[0];
                string signatureRequest = splited[1];

                // TODO :  DEMO SAMPLE -- get the secretKey with applicationkey or applicationkey/username
                string secretClientKey = string.Empty;
                string applicationKey = "AccessKey";
                if (applicationKeyRequest == applicationKey)
                {
                    secretClientKey = "SecretKey";
                }
                else
                {
                    // no user with this application key found
                    throw new UnknowUserException("No user with this combinaison keys exists.");
                }
                // TODO : DEMO SAMPLE

                // Check Signatures // applicationkey
                bool correctSignature = cryptoAuthorizeManager.CompareSignatures(signatureRequest, secretClientKey, actionContext.Request);
                if (correctSignature == false)
                {
                    // incorrect signature
                    throw new RequestParameterException("Invalid signature.");
                }

                // if signature OK, test Timestamp
                bool valideTimestamp = cryptoAuthorizeManager.CheckRequestTimeStampValidity(timestamp);
                if (valideTimestamp == false)
                {
                    throw new RequestTimeException("Request time too skewed.");
                }

                // signature ok => authenticated
                return true;
            }
        }

        #endregion privates
    }
}
