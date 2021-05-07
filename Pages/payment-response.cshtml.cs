using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace knet_dotnet_core_integration
{
    [IgnoreAntiforgeryToken]
    public class payment_response : PageModel
    {
        public Dictionary<string, string> KnetParamsList { get; set; }
        public string PaymnetId { get; set; }
        public string TrackID { get; set; }
        public string Result { get; set; }
        public void OnPost(
            [FromForm] string trandata,
            [FromForm] string paymentid,
            [FromForm] string trackid,

            [FromForm] string tranid,
            [FromForm] string ErrorText,
            [FromForm] string Error,
            [FromForm] string result
        )
        {
            PaymnetId = paymentid;
            TrackID = trackid;

            var knet = new KnetUtil();
            var decryptedParams = knet.DecryptUrlParams(trandata);
            var paramsArray = decryptedParams.Split("&");
            KnetParamsList = new();
            foreach (var param in paramsArray)
            {
                var key = param;
                var value = "";
                if (!param.Contains("="))
                {
                    KnetParamsList.Add(key: key, value: value);
                    continue;
                }

                var keyAndValue = param.Split("=");
                key = keyAndValue[0];
                if (keyAndValue.Length == 2)
                    value = keyAndValue[1];
                KnetParamsList.Add(key: key, value: value);
            }

            Result = KnetParamsList["result"];
        }
    }
}
