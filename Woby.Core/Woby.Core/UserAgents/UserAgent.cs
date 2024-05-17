using FluentResults;
using System.Collections.Immutable;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.CommonLanguage.Signaling.Roles;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Signaling.Dialogs;
using Woby.Core.Signaling.UserAgents.ValueObjects;
using Woby.Core.Signaling.Primitives;
using Woby.Core.Signaling.UserAgents.DomainEvents;
using Woby.Core.Signaling.Errors;

namespace Woby.Core.Signaling.UserAgents
{
    public abstract class UserAgent : AggregateRoot<UserAgentId>
    {

        public ICollection<Dialog> Dialogs { get; init; }

        internal UserAgent() { }

        public Result<MessageBase> HandleNotifyIncomingCall(RequestBase request)
        {
            var dialog = GetDialog(request.Signaling.DialogId);

            if (dialog is null) // create dialog for notifying incoming call 
                dialog = new Dialog
                {
                    Id = request.Signaling.DialogId,
                    Callee = Id,
                    Caller = UserAgentId.CreateUserAgentIdFromRoute(request.Signaling.From),
                    Messages = (new List<MessageBase>() { request }).ToImmutableList(),
                };

            var notifyIncomingCallResult = NotifyIncomingCall(request.Signaling.From);

            MessageBase response;
            switch (notifyIncomingCallResult)
            {
                case DialogUpdateRequestOptions.Ignore:
                    dialog.AppendMessage((response = new MessageBase
                    {
                        Signaling = new SignalingSection
                        {
                            DialogId = dialog.Id,
                            NegitiationId = request.Signaling.NegitiationId,
                            From = request.Signaling.To, // change message direction
                            To = request.Signaling.From, // change message direction
                            Proxies = request.Signaling.Proxies,
                            Role = RoleType.NotifyIncomingCallRequestIgnored,
                            Sequence = request.Signaling.Sequence.Increment(),
                        },
                        Content = null
                    }));

                    break;
                case DialogUpdateRequestOptions.Notified:
                    dialog.AppendMessage((response = new MessageBase
                    {
                        Signaling = new SignalingSection
                        {
                            DialogId = dialog.Id,
                            NegitiationId = request.Signaling.NegitiationId,
                            From = request.Signaling.To, // change message direction
                            To = request.Signaling.From, // change message direction
                            Proxies = request.Signaling.Proxies,
                            Role = RoleType.NotifyIncomingCallRequestNotified,
                            Sequence = request.Signaling.Sequence.Increment(),
                        },
                        Content = null
                    }));

                    break;
                default:
                    dialog.AppendMessage((response = new MessageBase
                    {
                        Signaling = new SignalingSection
                        {
                            DialogId = dialog.Id,
                            NegitiationId = request.Signaling.NegitiationId,
                            From = request.Signaling.To, // change message direction
                            To = request.Signaling.From, // change message direction
                            Proxies = request.Signaling.Proxies,
                            Role = RoleType.NotifyIncomingCallRequestNotSupported,
                            Sequence = request.Signaling.Sequence.Increment(),
                        },
                        Content = null
                    }));

                    break;
            }

            RaiseDomainEvent(new IncomingCallNotifiedDomainEvent
            {
                DialogId = dialog.Id,
                UserAgentId = Id,
                Response = response,
            });

            return response;
        }

        public Result<MessageBase> HandleIncomingCallRequest(RequestBase request)
        {
            var dialog = GetDialog(request.Signaling.DialogId);

            if (dialog is null) // create dialog for incoming call 
                dialog = new Dialog
                {
                    Id = request.Signaling.DialogId,
                    Callee = Id,
                    Caller = UserAgentId.CreateUserAgentIdFromRoute(request.Signaling.From),
                    Messages = (new List<MessageBase>() { request }).ToImmutableList(),
                };

            var incomingCallResult = IncomingCall(request);

            MessageBase response;
            switch (incomingCallResult)
            {
                case DialogCreationOptions.Decline:
                    dialog.AppendMessage((response = new MessageBase
                    {
                        Signaling = new SignalingSection
                        {
                            DialogId = dialog.Id,
                            NegitiationId = request.Signaling.NegitiationId,
                            From = request.Signaling.To, // change message direction
                            To = request.Signaling.From, // change message direction
                            Proxies = request.Signaling.Proxies,
                            Role = RoleType.DialogCreationRequestDeclined,
                            Sequence = request.Signaling.Sequence.Increment(),
                        },
                        Content = null
                    }));
                    break;
                case DialogCreationOptions.Answer:
                    dialog.AppendMessage((response = new MessageBase
                    {
                        Signaling = new SignalingSection
                        {
                            DialogId = dialog.Id,
                            NegitiationId = request.Signaling.NegitiationId,
                            From = request.Signaling.To, // change message direction
                            To = request.Signaling.From, // change message direction
                            Proxies = request.Signaling.Proxies,
                            Role = RoleType.DialogCreationRequestAccepted,
                            Sequence = request.Signaling.Sequence.Increment(),
                        },
                        Content = null
                    }));
                    break;
                default:
                    dialog.AppendMessage((response = MessageBase.NoMessage(
                        new SignalingSection
                        {
                            DialogId = dialog.Id,
                            NegitiationId = request.Signaling.NegitiationId,
                            From = request.Signaling.To, // change message direction
                            To = request.Signaling.From, // change message direction
                            Proxies = request.Signaling.Proxies,
                            Role = RoleType.DialogCreationRequestIgnored,
                            Sequence = request.Signaling.Sequence.Increment(),
                        }
                    )));
                    break;
            }

            RaiseDomainEvent(
                new DialogCreatedDomainEvent
                {
                    UserAgent = this,
                    DialogId = request.Signaling.DialogId,
                    Response = response,
                });

            return Result.Ok(response);
        }

        public Result<MessageBase> HandleDialogUpdateRequest(RequestBase request) => throw new NotSupportedException();

        public bool DoesDialogExists(DialogId dialogId) => Dialogs.Any(dialog => dialog.Id == dialogId);

        private Dialog? GetDialog(DialogId dialogId) => Dialogs.FirstOrDefault(dialog => dialog.Id == dialogId);

        public Result HandleDialogRequest(RequestBase request)
        {
            var dialog = GetDialog(request.Signaling.DialogId);



            return Result.Fail(new LoopDetectedError());
        }

        public abstract DialogCreationOptions IncomingCall(RequestBase request);

        public abstract DialogUpdateRequestOptions DialogUpdateRequest(Dialog previous, Dialog updated);

        public abstract DialogUpdateRequestOptions NotifyIncomingCall(Route from);

        public enum NotifyIncomingCallOptions
        {
            NotSupported,
            Ignored,
            Notified,
        }

        public enum DialogCreationOptions
        {
            Ignore,
            Decline,
            Answer,
        }

        public enum DialogUpdateRequestOptions
        {
            Ignore,
            Notified,
        }
    }
}
