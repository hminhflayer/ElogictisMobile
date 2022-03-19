using System;
using System.Collections.Generic;
using System.Text;

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
                return key + time;
            }
            return "";
        }
    }
}
