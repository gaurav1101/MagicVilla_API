using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Models
{
    public class APIRequest
    {
        public APIType apiType { get; set; }
        public string url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}
