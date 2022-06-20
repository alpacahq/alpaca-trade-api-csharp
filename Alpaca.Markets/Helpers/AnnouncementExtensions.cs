namespace Alpaca.Markets;

/// <summary>
/// Set of extensions methods for the <see cref="IAnnouncement"/> interface.
/// </summary>
public static class AnnouncementExtensions
{
    /// <summary>
    /// Gets the corporate action declaration date (<see cref="CorporateActionDateType.DeclarationDate"/>)
    /// or <c>null</c> if date not specified or not applicable for this corporate action type.
    /// </summary>
    /// <param name="announcement">Corporate action announcement record.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="announcement"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly? GetDeclarationDate(
        this IAnnouncement announcement) =>
        announcement.EnsureNotNull().GetDate(CorporateActionDateType.DeclarationDate);

    /// <summary>
    /// Gets the corporate action execution date (<see cref="CorporateActionDateType.ExecutionDate"/>)
    /// or <c>null</c> if date not specified or not applicable for this corporate action type.
    /// </summary>
    /// <param name="announcement">Corporate action announcement record.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="announcement"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly? GetExecutionDate(
        this IAnnouncement announcement) =>
        announcement.EnsureNotNull().GetDate(CorporateActionDateType.ExecutionDate);

    /// <summary>
    /// Gets the corporate action payable date (<see cref="CorporateActionDateType.PayableDate"/>)
    /// or <c>null</c> if date not specified or not applicable for this corporate action type.
    /// </summary>
    /// <param name="announcement">Corporate action announcement record.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="announcement"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly? GetPayableDate(
        this IAnnouncement announcement) =>
        announcement.EnsureNotNull().GetDate(CorporateActionDateType.PayableDate);

    /// <summary>
    /// Gets the corporate action record date (<see cref="CorporateActionDateType.RecordDate"/>)
    /// or <c>null</c> if date not specified or not applicable for this corporate action type.
    /// </summary>
    /// <param name="announcement">Corporate action announcement record.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="announcement"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly? GetRecordDate(
        this IAnnouncement announcement) =>
        announcement.EnsureNotNull().GetDate(CorporateActionDateType.RecordDate);
}
