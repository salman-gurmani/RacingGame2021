using System;

namespace Tapdaq
{
    [Serializable]
    public class TDNetworkStatus
    {
        public string name;
        public string status;
        public TDAdError error;

        public TDNetworkStatus()
        {
        }
    }
}