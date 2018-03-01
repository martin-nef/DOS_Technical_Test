using System;
using System.Collections.Generic;

namespace DOS_Technical_Test.Interfaces
{
    public interface IDistanceMatrixApiOptionsModel
    {
        string Mode { get; set; }
        string Language { get; set; }
        string Units { get; set; }
        string ApiKey { get; set; }
        string previousMode { get; set; }
        string previousLanguage { get; set; }
        string previousUnits { get; set; }
        string previousApiKey { get; set; }
    }
}
