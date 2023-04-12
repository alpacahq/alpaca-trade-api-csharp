namespace Alpaca.Markets;

/// <summary>
/// User account status in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum AccountStatus
{
    /// <summary>
    /// Account opened but not verified.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ONBOARDING")]
    Onboarding,

    /// <summary>
    /// Missed important information.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "SUBMISSION_FAILED")]
    SubmissionFailed,

    /// <summary>
    /// Additional information added.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "SUBMITTED")]
    Submitted,

    /// <summary>
    /// Account data updated.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ACCOUNT_UPDATED")]
    AccountUpdated,

    /// <summary>
    /// Approval request sent.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "APPROVAL_PENDING")]
    ApprovalPending,

    /// <summary>
    /// Account approved and working.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ACTIVE")]
    Active,

    /// <summary>
    /// Account approval rejected.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "REJECTED")]
    Rejected,

    /// <summary>
    /// Account disabled.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DISABLED")]
    Disabled,

    /// <summary>
    /// Disable request sent.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DISABLE_PENDING")]
    DisablePending,

    /// <summary>
    /// Account approved but still not active.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "APPROVED")]
    Approved,

    /// <summary>
    /// Account approved but not active.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "INACTIVE")]
    Inactive,

    /// <summary>
    /// Application was edited (e.g. to match info from uploaded docs).
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "EDITED")]
    Edited,

    /// <summary>
    /// Application requires manual action.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ACTION_REQUIRED")]
    ActionRequired,

    /// <summary>
    /// Account is closed.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ACCOUNT_CLOSED")]
    AccountClosed,

    /// <summary>
    /// Account approved only for paper trading.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "PAPER_ONLY")]
    PaperOnly
}
