using DOS_Technical_Test.Interfaces;
using System;
using System.Collections.Generic;

namespace DOS_Technical_Test.Models
{
    public class DistanceMatrixApiOptionsModel : IDistanceMatrixApiOptionsModel
    {
        public string Mode { get; set; }
        public string Language { get; set; }
        public string Units { get; set; }
        public string ApiKey { get; set; }
        public string previousMode { get; set; }
        public string previousLanguage { get; set; }
        public string previousUnits { get; set; }
        public string previousApiKey { get; set; }
    }
}
