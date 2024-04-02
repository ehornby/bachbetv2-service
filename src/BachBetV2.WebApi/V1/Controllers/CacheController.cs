using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using Microsoft.AspNetCore.Mvc;
using WebPush;

namespace BachBetV2.WebApi.V1.Controllers
{
    [Route("v1")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICachingService _cachingService;

        public CacheController(ICachingService cachingService)
        {
            _cachingService = cachingService;
        }


        [HttpPost]
        [Route("[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearCache()
        {
            _cachingService.SetCacheStatus<Bet>(false);
            _cachingService.SetCacheStatus<Challenge>(false);
            _cachingService.SetCacheStatus<ChallengeClaim>(false);

            return new OkResult();
        }

        [HttpPost]
        [Route("[controller]/push")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult TestPushNotif()
        {
            /*
             *  {"endpoint":"","expirationTime":null,"keys":{"p256dh":"","auth":""}}
             */
            var pushEndpoint = @"https://fcm.googleapis.com/fcm/send/dEGzOJtsr4Q:APA91bF64oSe3scz15ES0NCJQNAg7roy8mxI7NvdbLz_V2kPLUdsfQAlCCBgf57bR5Zw6SygZoom2UQRZTxDU6gXQRE1kLQom9iJIHXmJlsLVFMHuH2XKix7YmDlCbFX_oqKWbWYQ8LA";
            var p256dh = @"BIS7Araj6_vD1DR09sbotLC8JQbb6kwmWOrbUZdiH3YWtExzRUz1wZ9W5bM7MXhZOksW0VjNfg3zcpEj7gn1Enk";
            var auth = @"NloALTr13v30jVAsYztl1g";
            var vapidPublicKey = @"BJuxBEDSilFABWe-c2Q_JbcCXzA57JBaNO3wTE2Zpe0PBDz0iwFSLZBaLB2ayz0oUDmcvt8z2t_8scNzT9Orvos";
            var vapidPrivateKey = @"DdqDFOt3fM8Vpgs9s2Zv-N1CBdON6VXPYO1eyMD-6IM";

            var subject = "http://www.google.ca";

            var pushSubscription = new PushSubscription(pushEndpoint, p256dh, auth);
            var vapidDetails = new VapidDetails(subject, vapidPublicKey, vapidPrivateKey);

            var webPushClient = new WebPushClient();

            try
            {
                webPushClient.SendNotification(pushSubscription, "testing raw string", vapidDetails);
            }
            catch (WebPushException exception)
            {
                Console.WriteLine("Http STATUS code" + exception.StatusCode);
            }

            return new OkResult();
        }
    }
}
