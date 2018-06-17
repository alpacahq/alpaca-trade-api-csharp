using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatus
    {
        [EnumMember(Value = "accepted")]
        Accepted,

        [EnumMember(Value = "new")]
        New,

        [EnumMember(Value = "partially_filled")]
        PartiallyFilled,

        [EnumMember(Value = "filled")]
        Filled,

        [EnumMember(Value = "done_for_day")]
        DoneForDay,

        [EnumMember(Value = "canceled")]
        Canceled,

        [EnumMember(Value = "replaced")]
        Replaced,

        [EnumMember(Value = "pending_cancel")]
        PendingCancel,

        [EnumMember(Value = "stopped")]
        Stopped,

        [EnumMember(Value = "rejected")]
        Rejected,

        [EnumMember(Value = "suspended")]
        Suspended,

        [EnumMember(Value = "pending_new")]
        PendingMew,

        [EnumMember(Value = "calculated")]
        Calculated,

        [EnumMember(Value = "expired")]
        Expired,

        [EnumMember(Value = "accepted_for_bidding")]
        AcceptedForBidding,

        [EnumMember(Value = "pending_replace")]
        PendingReplace
    }
}