using System;

namespace DOS_Technical_Test.Interfaces
{
    // Interface for Geological Information.
    public interface IGeoInfoModel
    {
        // Distance between origin and destination.
        int Distance { get; set; }

        // Readable version of Distance.
        string ReadableDistance { get; set; }

        // Duration of the trip between origin and destination.
        TimeSpan Duration { get; set; }

        // Readable version of Duration.
        string ReadableDuration { get; set; }

        // Indicates whether the request for Geological information was a success.
        bool Success { get; }

        // Origin address.
        string Origin { get; set; }

        // Destination address.
        string Destination { get; set; }
    }
}
