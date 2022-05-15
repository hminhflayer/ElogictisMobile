using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

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
                return key + (-1*time);
            }
            return "";
        }

        public ImageSource GetImageSource(string base64 = null)
        {
            byte[] Base64Stream = Convert.FromBase64String(base64);
            return ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }
    }
}
