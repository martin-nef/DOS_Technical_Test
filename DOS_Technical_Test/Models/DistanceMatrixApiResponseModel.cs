using System;
using System.Collections.Generic;

namespace DOS_Technical_Test.Models
{
    /// <summary>
    /// Structure for deserialising API response JSON into.
    /// </summary>
    public struct DistanceMatrixApiResponseModel
    {
        public IEnumerable<string> destination_addresses { get; set; }
        public IEnumerable<string> origin_addresses { get; set; }
        public IEnumerable<Row> rows { get; set; }
        public string status { get; set; }
    }

    public struct Row
    {
        public IEnumerable<Element> elements { get; set; }
    }

    public struct Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }

    public struct Distance
    {
        public int value { get; set; }
        public string text { get; set; }
    }

    public struct Duration
    {
        public int value { get; set; }
        public string text { get; set; }
    }
}