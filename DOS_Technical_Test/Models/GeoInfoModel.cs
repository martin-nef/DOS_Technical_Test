using DOS_Technical_Test.Interfaces;
using DOS_Technical_Test.Exceptions;
using System;

namespace DOS_Technical_Test.Models
{
    public class GeoInfoModel : IGeoInfoModel
    {
        public int Distance { get; set; }
        public string ReadableDistance { get; set; }
        public TimeSpan Duration { get; set; }
        public string ReadableDuration { get; set; }
        public string Status { get; set; }
        public string ReadableStatus { get { return GetReadableStatus(Status); } }
        public string HiLevelStatus { get; set; }
        public string ReadableHiLevelStatus { get { return GetReadableStatus(HiLevelStatus); } }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public bool Success => Status == "OK" && HiLevelStatus == "OK";

        public GeoInfoModel()
        {
        }

        public GeoInfoModel(int distance, string readableDistance,
            int duration, string readableDuration, string status)
        {
            Distance = distance;
            ReadableDistance = readableDistance;
            Duration = TimeSpan.FromSeconds(duration);
            ReadableDuration = readableDuration;
            Status = status;
        }

        public GeoInfoModel(IGeoInfoModel model)
        {
            Distance = model.Distance;
            ReadableDistance = model.ReadableDistance;
            Duration = model.Duration;
            ReadableDuration = model.ReadableDuration;
            Origin = model.Origin;
            Destination = model.Destination;
        }

        public string GetReadableStatus(string status)
        {
            switch (status)
            {
                case "OK":
                    return "The response contains a valid result.";
                case "NOT_FOUND":
                    return "The origin and/or destination could not be found.";
                case "ZERO_RESULTS":
                    return "No route could be found between the origin and destination.";
                case "MAX_ROUTE_LENGTH_EXCEEDED":
                    return "The requested route is too long and cannot be processed.";
                case "INVALID_REQUEST":
                    return "The provided request was invalid. Check the origin and destination addresses.";
                case "MAX_ELEMENTS_EXCEEDED":
                    return "The product of origins and destinations exceeds the per-query limit.";
                case "OVER_QUERY_LIMIT":
                    return "The Distance Matrix service daily request limit reached. Cannot process any more requests.";
                case "REQUEST_DENIED":
                    return "Request to use the Distance Matrix service was denied.";
                case null:
                    return "The status is missing.";
                case "UNKNOWN_ERROR":
                default:
                    return "Your request could not be processed due to a server error. The request may succeed if you try again.";
            }
        }
    }
}
