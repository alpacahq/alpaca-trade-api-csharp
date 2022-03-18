namespace Alpaca.Markets;

/// <summary>
/// Encapsulates corporate action announcement information from Alpaca REST API.
/// </summary>
[CLSCompliant(false)]
[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
public interface IAnnouncement
{
    /// <summary>
    /// Gets ID that is specific to a single announcement.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets ID that remains consistent across all announcements for the same corporate action.
    /// Unlike <see cref="Id"/>, this can be used to connect multiple announcements to see how
    /// the terms have changed throughout the lifecycle of the corporate action event.
    /// </summary>
    String CorporateActionId { get; }

    /// <summary>
    /// Gets corporate action type.
    /// </summary>
    CorporateActionType Type { get; }

    /// <summary>
    /// Gets corporate action sub-type.
    /// </summary>
    CorporateActionSubType SubType { get; }

    /// <summary>
    /// Gets symbol of the company initiating the announcement.
    /// </summary>
    String InitiatingSymbol { get; }

    /// <summary>
    /// Gets CUSIP of the company initiating the announcement.
    /// </summary>
    String InitiatingCusip { get; }

    /// <summary>
    /// Gets symbol of the child company involved in the announcement.
    /// </summary>
    String TargetSymbol { get; }

    /// <summary>
    /// Gets CUSIP of the child company involved in the announcement.
    /// </summary>
    String TargetCusip { get; }

    /// <summary>
    /// Gets the amount of cash to be paid per share held by an account on the record date.
    /// </summary>
    Decimal Cash { get; }

    /// <summary>
    /// Gets the denominator to determine any quantity change ratios in positions.
    /// </summary>
    Decimal OldRate { get; }

    /// <summary>
    /// Gets the numerator to determine any quantity change ratios in positions.
    /// </summary>
    Decimal NewRate { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateType"></param>
    /// <returns></returns>
    public DateOnly? GetDate(
        CorporateActionDateType dateType);
}
