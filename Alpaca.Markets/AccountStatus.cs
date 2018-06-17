using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AccountStatus
    {
        [EnumMember(Value = "ONBOARDING")]
        Onboarding,

        [EnumMember(Value = "SUBMISSION_FAILED")]
        SubmissionFailed,

        [EnumMember(Value = "SUBMITTED")]
        Submitted,

        [EnumMember(Value = "ACCOUNT_UPDATED")]
        AccountUpdated,

        [EnumMember(Value = "APPROVAL_PENDING")]
        ApprovalPending,

        [EnumMember(Value = "ACTIVE")]
        Active,

        [EnumMember(Value = "REJECTED")]
        Rejected
    }
}