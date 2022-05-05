using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IRecordweb.Models
    {
    public class UploadBhavCopyModel
        {
        public class Rootobject
            {
            public D d { get; set; }
            }

        public class D
            {
            public Summary Summary { get; set; }
            public Datum[] Data { get; set; }
            }

        public class Summary
            {
            public DateTime AsOn { get; set; }
            public int Count { get; set; }
            public object Status { get; set; }
            }

        public class Datum
            {
            public string __type { get; set; }
            public string Date { get; set; }
            public string Name { get; set; }
            public string Symbol { get; set; }
            public string ExpiryDate { get; set; }
            public float Open { get; set; }
            public float High { get; set; }
            public float Low { get; set; }
            public float Close { get; set; }
            public float PreviousClose { get; set; }
            public int Volume { get; set; }
            public string VolumeInThousands { get; set; }
            public float Value { get; set; }
            public int OpenInterest { get; set; }
            public string DateDisplay { get; set; }
            public string InstrumentName { get; set; }
            public float StrikePrice { get; set; }
            public string OptionType { get; set; }
            }

        }
    }