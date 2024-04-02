using BachBetV2.Application.Enums;
using BachBetV2.Application.Extensions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Models;
using BachBetV2.Application.Models.Results;
using System.Text.Json;
using System.Text.Json.Nodes;
using WebPush;

namespace BachBetV2.Application.Services
{
    public sealed class PushNotificationService : IPushNotificationService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserRepository _userRepository;
        private readonly WebPushClient _webPushClient;

#pragma warning disable S1075 // URIs should not be hardcoded
        private const string BASE_URL = "https://bachbet.chriscardwell.com";
#pragma warning restore S1075 // URIs should not be hardcoded

        public PushNotificationService(ISubscriptionRepository subscriptionRepository, IUserRepository userRepository, WebPushClient webPushClient)
        {
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
            _webPushClient = webPushClient;
        }

        public async Task SendPushNotificationAsync(PushNotification pushNotification, CancellationToken cancellationToken)
        {
            try
            {
                var userSubscription = await _subscriptionRepository.GetPushSubscriptionByUser(pushNotification.UserId, cancellationToken);

                if (userSubscription is null)
                {
                    return;
                }

                PushSubscription subscription = new(userSubscription.PushEndpoint, userSubscription.P256DH, userSubscription.Auth);
                VapidDetails vapidDetails = new(GenerateUrl(pushNotification), userSubscription.PublicKey, userSubscription.PrivateKey);

                Dictionary<string, object> options = new()
                {
                    { "vapidDetails", vapidDetails },
                };

                await _webPushClient.SendNotificationAsync(subscription, GenerateFormattedMessage(pushNotification), options, cancellationToken);
            }
            catch
            {
                // Swallowing exception here to not disrupt the main application flow
            }
        }

        public async Task<Result> SendManualPushNotificationAsync(PushNotification pushNotification, CancellationToken cancellationToken)
        {
            try
            {
                var sendingUser = await _userRepository.GetUserById(pushNotification.RelatedUserId, cancellationToken);
                pushNotification.RelatedUserName = sendingUser!.UserName;

                if(sendingUser.UserName.ToLower() == "shift3d")
                {
                    pushNotification.NotificationType = NotificationType.BachelorAnnouncement;
                }

                var subscriptions = await _subscriptionRepository.GetAllPushSubscriptions(cancellationToken);

                if (subscriptions is null)
                {
                    return new ErrorResult("Subscriptions could not be loaded");
                }

                var tasks = new List<Task>();
                

                foreach (var subscription in subscriptions)
                {
                    try
                    {
                        PushSubscription pushSubscription = new(subscription.PushEndpoint, subscription.P256DH, subscription.Auth);
                        VapidDetails vapidDetails = new(GenerateUrl(pushNotification), subscription.PublicKey, subscription.PrivateKey);

                        Dictionary<string, object> options = new()
                    {
                        { "vapidDetails", vapidDetails },
                    };

                        tasks.Add( _webPushClient.SendNotificationAsync(pushSubscription, GenerateFormattedMessage(pushNotification), options, cancellationToken));
                    }
                    catch(Exception ex)
                    {
                        //push notif failed for one, swallow and keep trying the rest
                    }
                }

                //TODO wait some amount of time for all tasks to finish, then exit and invoke cancellation token?

                return new SuccessResult();

            }
            catch (Exception ex)
            {
                return ex.HandleError();
            }
        }


        private static string GenerateFormattedMessage(PushNotification pushNotification)
        {
            JsonObject message = pushNotification.NotificationType switch
            {
                NotificationType.BetTaken => new()
                {
                    { "title", "🤝 Someone took your bet! 🤝" },
                    { "body", $"{pushNotification.RelatedUserName} wagered {pushNotification.Amount} dbux on \"{pushNotification.EntityName}\"" }
                },
                NotificationType.BetPayoutWin => new()
                {
                    { "title", "🤑 A bet you wagered on has been paid out! 🤑" },
                    { "body", $"{pushNotification.RelatedUserName} paid out \"{pushNotification.EntityName}\" for {pushNotification.Amount} dbux, congratulations"}
                },
                NotificationType.BetPayoutLoss => new()
                {
                    { "title", "💸 A bet you wagered on has been paid out! 💸" },
                    { "body", $"\"{pushNotification.EntityName}\" has been paid out to {pushNotification.RelatedUserName}, you lost" }
                },
                NotificationType.BetClosed => new()
                {
                    { "title", "🔒 A bet you wagered on has been closed! 🔒" },
                    { "body", $"{pushNotification.RelatedUserName} has closed \"{pushNotification.EntityName}\"" }
                },
                NotificationType.ClaimWitnessed => new()
                {
                    { "title", "👀 One of your challenge claims has been witnessed! 👀" },
                    { "body", $"{pushNotification.RelatedUserName} has witnessed your claim for \"{pushNotification.EntityName}\"" }
                },
                NotificationType.Transfer => new()
                {
                    { "title", "🤑 You have received a transfer! 🤑" },
                    { "body", $"{pushNotification.RelatedUserName} has transferred you {pushNotification.Amount} dbux" }
                },
                NotificationType.Alert => new()
                {
                    { "title", "🚨 heads up fuckers 🚨" },
                    { "body", $"{pushNotification.Message}" }
                },
                NotificationType.BachelorAnnouncement => new()
                {
                    { "title", "🤴🏾 a word from the bachelor boi 🤴🏾" },
                    { "body", $"{pushNotification.Message}" }
                },

                _ => new()
            };

            return JsonSerializer.Serialize(message);
        }

        private static string GenerateUrl(PushNotification pushNotification)
        {
            return pushNotification.NotificationType switch
            {
                NotificationType.BetTaken
                or NotificationType.BetPayoutWin
                or NotificationType.BetPayoutLoss
                or NotificationType.BetClosed => $"{BASE_URL}/bets/{pushNotification.EntityId}",
                NotificationType.ClaimWitnessed => $"{BASE_URL}/challenges/{pushNotification.EntityId}",
                NotificationType.Transfer => $"{BASE_URL}/users/{pushNotification.UserId}",
                _ => BASE_URL
            };
        }
    }
}
