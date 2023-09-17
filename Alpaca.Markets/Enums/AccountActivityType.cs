namespace Alpaca.Markets;

#pragma warning disable CA1027

/// <summary>
/// Types of account activities
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
[SuppressMessage("ReSharper", "CommentTypo")]
[SuppressMessage("ReSharper", "IdentifierTypo")]
[SuppressMessage("ReSharper", "StringLiteralTypo")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum AccountActivityType
{
    /// <summary>
    /// Order fills (both partial and full fills)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "FILL")]
    Fill,

    /// <summary>
    /// Cash transactions (both <see cref="CashDeposit"/> and <see cref="CashWithdrawal"/>)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "TRANS")]
    Transaction,

    /// <summary>
    /// Miscellaneous or rarely used activity types (All types except those in
    /// <see cref="Transaction"/>, <see cref="Dividend"/>, or <see cref="Fill"/>)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "MISC")]
    Miscellaneous,

    /// <summary>
    /// ACATS IN/OUT (Cash)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ACATC")]
    ACATCash,

    /// <summary>
    /// ACATS IN/OUT (Securities)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ACATS")]
    ACATSecurities,

    /// <summary>
    /// Cash deposit (+)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "CSD")]
    CashDeposit,

    /// <summary>
    /// Cash withdrawal (-)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "CSW")]
    CashWithdrawal,

    /// <summary>
    /// Dividends
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DIV")]
    Dividend,

    /// <summary>
    /// Dividend (capital gains long term)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DIVCGL")]
    DividendCapitalGainsLongTerm,

    /// <summary>
    /// Dividend (capital gain short term)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DIVCGS")]
    DividendCapitalGainsShortTerm,

    /// <summary>
    /// Dividend fee
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DIVFEE")]
    DividendFee,

    /// <summary>
    /// Dividend adjusted (Foreign Tax Withheld)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DIVFT")]
    DividendForeignTaxWithheld,

    /// <summary>
    /// Dividend adjusted (NRA Withheld)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DIVNRA")]
    DividendNRAWithheld,

    /// <summary>
    /// Dividend return of capital
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DIVROC")]
    DividendReturnOfCapital,

    /// <summary>
    /// Dividend adjusted (Tefra Withheld)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DIVTW")]
    DividendTefraWithheld,

    /// <summary>
    /// Dividend (tax exempt)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "DIVTXEX")]
    DividendTaxExempt,

    /// <summary>
    /// Interest (credit/margin)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "INT")]
    Interest,

    /// <summary>
    /// Interest adjusted (NRA Withheld)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "INTNRA")]
    InterestNRAWithheld,

    /// <summary>
    /// Interest adjusted (Tefra Withheld)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "INTTW")]
    InterestTefraWithheld,

    /// <summary>
    /// Journal entry
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "JNL")]
    JournalEntry,

    /// <summary>
    /// Journal entry (cash)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "JNLC")]
    JournalEntryCash,

    /// <summary>
    /// Journal entry (stock)
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "JNLS")]
    JournalEntryStock,

    /// <summary>
    /// Merger/Acquisition
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "MA")]
    MergerAcquisition,

    /// <summary>
    /// Name change
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "NC")]
    NameChange,

    /// <summary>
    /// Pass Thru Charge
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "PTC")]
    PassThruCharge,

    /// <summary>
    /// Pass Thru Rebate
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "PTR")]
    PassThruRebate,

    /// <summary>
    /// Reorganization
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "REORG")]
    Reorg,

    /// <summary>
    /// Symbol change
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "SC")]
    SymbolChange,

    /// <summary>
    /// Stock spinoff
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "SSO")]
    StockSpinoff,

    /// <summary>
    /// Stock split
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "SSP")]
    StockSplit,

    /// <summary>
    /// Option assignment
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "OPASN")]
    OptionAssignment = 31,

    /// <summary>
    /// Option expiration
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "OPEXP")]
    OptionExpiration,

    /// <summary>
    /// Option exercise
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "OPXRC")]
    OptionExercise,

    /// <summary>
    /// Fee denominated in USD
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "FEE")]
    FeeInUsd,

    /// <summary>
    /// Crypto Fee
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "CFEE")]
    CryptoFee
}

#pragma warning restore CA1027
