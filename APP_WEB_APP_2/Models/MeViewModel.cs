using System.Collections.Generic;

namespace APP_WEB_APP_2.Models
{
    public class MeViewModel
    {
        public string Username { get; set; }

        public bool SdkAvailable { get; set; }

        public dynamic UserInfo { get; set; }

        public IEnumerable<string> Groups { get; set; }
    }
}
