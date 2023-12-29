namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    private const Decimal AnnouncementCash = 100_000M;

    [Fact]
    public async Task ListAnnouncementsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var into = DateOnly.FromDateTime(DateTime.Today);
        var from = into.AddMonths(-1);

        mock.AddGet("/v2/corporate_actions/announcements", new JArray(
            createAnnouncement(from), createAnnouncement(into)));
        mock.AddGet("/v2/corporate_actions/announcements", new JArray(
            createAnnouncement(from), createAnnouncement(into)));

        var announcementsOne = await mock.Client.ListAnnouncementsAsync(
            new AnnouncementsRequest(CorporateActionType.Dividend, new Interval<DateOnly>(from, into))
            {
                DateType = CorporateActionDateType.DeclarationDate,
                Symbol = Stock
            });

        var announcementsTwo = await mock.Client.ListAnnouncementsAsync(
            new AnnouncementsRequest(Enum.GetValues<CorporateActionType>(), new Interval<DateOnly>(from, into))
            {
                DateType = CorporateActionDateType.RecordDate,
                Cusip = Stock
            });

        Assert.NotNull(announcementsOne);
        Assert.NotEmpty(announcementsOne);
        Assert.NotNull(announcementsTwo);
        Assert.NotEmpty(announcementsTwo);

        Assert.Equal(announcementsOne.Count, announcementsTwo.Count);

        foreach (var announcement in announcementsOne)
        {
            validate(announcement);
        }
    }

    [Fact]
    public async Task GetAnnouncementAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/corporate_actions/announcements/**", 
            createAnnouncement(DateOnly.FromDateTime(DateTime.Today)));

        var announcement = await mock.Client.GetAnnouncementAsync(Guid.NewGuid());

        validate(announcement);
    }

    private static JObject createAnnouncement(
        DateOnly recordDate) =>
        new(
            new JProperty("corporate_action_id", Guid.NewGuid().ToString("B")[..8]),
            new JProperty("ca_sub_type", CorporateActionSubType.DividendCash),
            new JProperty("id", Guid.NewGuid().ToString("D")),
            new JProperty("record_date", recordDate.ToString("O")),
            new JProperty("ca_type", CorporateActionType.Dividend),
            new JProperty("initiating_original_cusip", Stock),
            new JProperty("initiating_symbol", Stock),
            new JProperty("cash", AnnouncementCash));

    private static void validate(
        IAnnouncement announcement)
    {
        Assert.NotNull(announcement);

        Assert.False(String.IsNullOrEmpty(announcement.CorporateActionId));
        Assert.NotEqual(Guid.Empty, announcement.Id);

        Assert.Equal(Stock, announcement.InitiatingSymbol);
        Assert.Equal(Stock, announcement.InitiatingCusip);
        Assert.Equal(AnnouncementCash, announcement.Cash);
        Assert.NotNull(announcement.GetRecordDate());

        Assert.Null(announcement.GetDate(
            Enum.GetValues<CorporateActionDateType>().Max() + 1));
        Assert.Null(announcement.GetDeclarationDate());
        Assert.Null(announcement.GetExecutionDate());
        Assert.Null(announcement.GetPayableDate());

        Assert.Equal(String.Empty, announcement.TargetSymbol);
        Assert.Equal(String.Empty, announcement.TargetCusip);
        Assert.Equal(0M, announcement.OldRate);
        Assert.Equal(0M, announcement.NewRate);

    }
}
