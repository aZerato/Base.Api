namespace Base.Api.Controllers
{
    using Base.Api.Crosscutting.Core.Configuration;
    using Base.Api.Crosscutting.Ioc;
    using System.Web.Mvc;

    /// <summary>
    /// The HomeController class.
    /// </summary>
    public class HomeController : Controller
    {
        #region attributes

        private ISettingsApi settingsApi;

        #endregion attributes

        #region constructors

        public HomeController()
        {
            this.settingsApi = IocUnityContainer.Instance.Resolve<ISettingsApi>();
        }

        #endregion constructors

        #region publics

        /// <summary>
        /// Index action.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        /// <summary>
        /// Configuration action : return api configuration.
        /// </summary>
        /// <returns></returns>
        public ActionResult Configuration()
        {
            ViewBag.Title = "Configuration Page";

            this.ViewBag.WebSErvicesName = this.settingsApi.ApplicationSettings.GlobalSettings.WebServicesName;
            this.ViewBag.PrefixHeaderKey = this.settingsApi.ApplicationSettings.GlobalSettings.PrefixHeaderKey;
            this.ViewBag.RequestTimeValidity = this.settingsApi.ApplicationSettings.GlobalSettings.RequestTimeValidity;
            this.ViewBag.MaxResultsReturn = this.settingsApi.ApplicationSettings.GlobalSettings.MaxResultsReturn;

            return View();
        }

        #endregion publics

    }
}
