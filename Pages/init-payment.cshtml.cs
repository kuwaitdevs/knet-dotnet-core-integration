using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace knet_dotnet_core_integration
{
    public class init_payment : PageModel
    {
        public static string GetDomain(HttpRequest request) => (request.IsHttps ? "https://" : "http://") + request.Host.Value;
        public void OnPost([FromForm] string items)
        {
            items ??= "";
            var itemsList = items.Split(",");
            var totalPrice = 0.000;

            foreach (var item in itemsList)
            {
                if (item == "a")
                    totalPrice += 1.000;
                else if (item == "b")
                    totalPrice += 2.000;
                else if (item == "c")
                    totalPrice += 4.000;
            }

            var knet = new KnetUtil();
            var knetPaymentUrl = knet.GenerateKnetUrl(
                amount: totalPrice,
                trackId: $"{DateTime.Now:yyyyMMddHHmmss}",
                lang: "USA",
                currency: "414",
                onResponseUrl: $"{GetDomain(Request)}/payment-response",
                onErrorUrl: $"{GetDomain(Request)}/payment-error",
                udf1: items,
                udf2: "",
                udf3: "",
                udf4: "",
                udf5: "");

            Response.Redirect(knetPaymentUrl);
        }
    }
}
