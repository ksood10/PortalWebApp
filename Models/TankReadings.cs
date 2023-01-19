using System.Collections.Specialized;
using System;
using System.Collections.Generic;

namespace PortalWebApp.Models
{

    public class Reading {
        public string Height { get; set; }
        public string Volume { get; set; }
        public string NormalizedHeight { get; set; }
        public string NormalizedVolume { get; set; }

        public Reading() { }

}
    public static class TankReadings
    {

        private static List<Reading> lstReading { get; set; }

        static TankReadings()
        {
            lstReading = new List<Reading>();
        }

        public static List<Reading> Readings()
        {
            return lstReading;
        }

        public static void Add(string height, string volume, string normalizedheight, string normalizedvolume)
        {
            var reading = new Reading
            {
                Height = height,
                Volume = volume,
                NormalizedHeight = normalizedheight,
                NormalizedVolume = normalizedvolume
            };
            lstReading.Add(reading);
        }

        internal static void Clear()
        {
            lstReading.Clear();
        }
    }
}
