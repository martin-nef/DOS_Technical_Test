using DOS_Technical_Test.Interfaces;
using DOS_Technical_Test.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

namespace DOS_Technical_Test.Controllers
{
    public class DistanceMatrixApiController : Controller
    {
        // TODO: store all defaults and constants in a configuration file somewhere.
        public static Dictionary<string, string> Config = new Dictionary<string, string>();
        private const string ADDR_SEPARATOR = "|";
        private const string ApiAddress = "https://maps.googleapis.com/maps/api/distancematrix/";
        private static bool firstRun = true;

        public DistanceMatrixApiController()
        {
            if (!firstRun) return;
            firstRun = false;
            Config["Format"] = "json";
            Config["ApiKey"] = "AIzaSyC3ehQuAi8qcXW7vjPdeiI5IjAr2heq8tY";
            Config["Mode"] = "driving";
            Config["Units"] = "metric";
            Config["Language"] = "en-GB";
        }

        // Where the Start button takes you
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Settings()
        {
            // Put current settings into model
            DistanceMatrixApiOptionsModel model = new DistanceMatrixApiOptionsModel();
            model.previousApiKey = Config["ApiKey"];
            model.previousLanguage = Config["Language"];
            model.previousUnits = Config["Units"];
            model.previousMode = Config["Mode"];

            // View the settings page and display current settings.
            return View((IDistanceMatrixApiOptionsModel)model);
        }

        /// <summary>Update current settings.</summary>
        /// <note>Submit button on Settings screen calls this.</note>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateSettings(DistanceMatrixApiOptionsModel model)
        {
            // Update configuration values to ones received from the settings page.
            Config["Mode"] = model.Mode;
            Config["Language"] = model.Language;
            Config["Units"] = model.Units;
            if (model.ApiKey != null && model.ApiKey.Length > 0)
                Config["ApiKey"] = model.ApiKey;
            
            // Go back to address input.
            return View("Index");
        }

        /// <summary>
        /// Makes a request to the Distance Matrix API by Google (c) 2018,
        /// to provide distance and time required to travel between the specified origin and destination.
        /// The data received from the API is put into <see cref="GeoInfoModel"/> and is then passed onto the
        /// </summary>
        /// <note>This method is called when the Submit button is pressed on the address input screen.</note>
        /// <param name="model"><see cref="GeoInfoModel"/> containing the Origin and the Destination to look up.</param>
        public ActionResult RequestData(GeoInfoModel model)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.Accept] = "application/json";

                // If the user left the origin or destination address blank,
                if (model.Origin == null || model.Destination == null)
                {
                    // tell them to fill them both
                    ViewBag.Feedback = "Please, make sure to enter both - an origin and a destination.";

                    // and return to the address input screen
                    return View("Index");
                }

                string url = ConstructRequestString(new[] { model.Origin }, new[] { model.Destination });

                string jsonString;
                try
                {
                    jsonString = client.DownloadString(url);
                }
                catch (WebException)
                {
                    ViewBag.Feedback = "An error has occured while downloading the results. Trying again may help.";
                    View("Index");
                }

                // TODO: handle errors if can't deserialise json, i.e. if API response structure has changed.
                DistanceMatrixApiResponseModel jsonResult = JsonConvert.DeserializeObject<DistanceMatrixApiResponseModel>(jsonString);

                // TODO: handle multiple origins / destinations.
                
                // Fill the model with data received from the API.
                model.HiLevelStatus = jsonResult.status;
                if (jsonResult.origin_addresses.Any()
                    && jsonResult.destination_addresses.Any()
                    && jsonResult.rows.Any())
                {
                    model.Destination = jsonResult.destination_addresses.First();
                    model.Origin = jsonResult.origin_addresses.First();
                    var result = jsonResult.rows.First().elements.First();
                    model.Distance = result.distance.value;
                    model.ReadableDistance = result.distance.text;
                    model.Duration = TimeSpan.FromSeconds(result.duration.value);
                    model.ReadableDuration = result.duration.text;
                    model.Status = result.status;
                }

                // Pass whether the API request was a total success, since there's no need to display status then.
                ViewBag.RequestSuccess = model.Success;
                if (!model.Success && model.Status != null)
                {
                    // Combine two status messages into one string, pass that on to the view.
                    ViewBag.RequestStatus = "";
                    bool x = false;
                    if (model.HiLevelStatus != null)
                    {
                        ViewBag.RequestStatus = $"Download: {model.ReadableHiLevelStatus}";
                        x = true;
                    }
                    if (model.Status != null)
                    {
                        ViewBag.RequestStatus += x ? "\n" : string.Empty + $"Request: {model.ReadableStatus}";
                    }
                }

                return View("DisplayData", (IGeoInfoModel)model);
            }
        }

        /// <summary>
        /// Constructs the string used to get data from the API.
        /// </summary>
        /// <param name="origins"><see cref="IEnumerable{string}"> of Origin addresses.</param>
        /// <param name="destinations"><see cref="IEnumerable{string}"> of Origin addresses.</param>
        /// <param name="optionalParameters"><see cref="Arra"> of Origin addresses.</param>
        /// <returns></returns>
        private string ConstructRequestString(IEnumerable<string> origins, IEnumerable<string>destinations)
        {
            // Example "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=Washington,DC&destinations=New+York+City,NY&key=AIzaSyC3ehQuAi8qcXW7vjPdeiI5IjAr2heq8tY";

            string url = $"{ApiAddress}{Config["Format"]}?"
                       + $"units={Config["Units"]}&"
                       + $"language={Config["Language"]}&"
                       + $"mode={Config["Mode"]}&"
                       + $"key={Config["ApiKey"]}&"
                       + $"origins={string.Join(ADDR_SEPARATOR, origins)}&"
                       + $"destinations={string.Join(ADDR_SEPARATOR, destinations)}";

            return Uri.EscapeUriString(url);
        }
    }
}