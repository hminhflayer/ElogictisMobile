using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ElogictisMobile.Services
{
    public class GeneralKey
    {
        private static GeneralKey instance;

        public static GeneralKey Instance 
        { 
            get { if (instance == null) instance = new GeneralKey(); return instance; }
            private set => instance = value; 
        }

        public GeneralKey()
        {

        }

        public string General(string key = null)
        {
            if(!string.IsNullOrEmpty(key))
            {
               var time = DateTimeOffset.Now.ToUnixTimeSeconds();
                return key + (time);
            }
            return "";
        }

        public ImageSource GetImageSource(string base64 = null)
        {
            byte[] Base64Stream = Convert.FromBase64String(base64);
            return ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }

        public List<Position> DecodePolyline(string encodedPoints)
        {
            if (string.IsNullOrWhiteSpace(encodedPoints))
                return null;

            int index = 0;
            var polylineChars = encodedPoints.ToCharArray();
            var poly = new List<Position>();
            int currentLat = 0;
            int currentLng = 0;
            int next5Bits;

            while (index < polylineChars.Length)
            {
                int sum = 0;
                int shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                sum = 0;
                shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32)
                {
                    break;
                }

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                var mLatLng = new Position(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0);
                poly.Add(mLatLng);
            }

            return poly;
        }
    }
}
