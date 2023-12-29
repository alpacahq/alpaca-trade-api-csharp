namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(INewsArticle))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonNewsArticle : INewsArticle
{
    public sealed class Image
    {
        [JsonProperty(PropertyName = "size", Required = Required.Always)]
        public String Size { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "url", Required = Required.Default)]
        public Uri? Url { get; set; }
    }

    [JsonProperty(PropertyName = "id", Required = Required.Always)]
    public Int64 Id { get; set; }

    [JsonProperty(PropertyName = "headline", Required = Required.Always)]
    public String Headline { get; set; } = String.Empty;

    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    [JsonProperty(PropertyName = "created_at", Required = Required.Always)]
    public DateTime CreatedAtUtc { get; set; }

    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    [JsonProperty(PropertyName = "updated_at", Required = Required.Always)]
    public DateTime UpdatedAtUtc { get; set; }

    [JsonProperty(PropertyName = "author", Required = Required.Always)]
    public String Author { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "summary", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public String Summary { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "content", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public String Content { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "url", Required = Required.Default)]
    public Uri? ArticleUrl { get; set; }

    [JsonProperty(PropertyName = "source", Required = Required.Always)]
    public String Source { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "symbols", Required = Required.Always)]
    public List<String> SymbolsList { get; [ExcludeFromCodeCoverage] set; } = [];

    [JsonProperty(PropertyName = "images", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public List<Image> Images { get; [ExcludeFromCodeCoverage] set; } = [];

    [JsonIgnore]
    public IReadOnlyList<String> Symbols => SymbolsList.EmptyIfNull();

    [JsonIgnore]
    public Uri? SmallImageUrl { get; private set; }

    [JsonIgnore]
    public Uri? LargeImageUrl { get; private set; }

    [JsonIgnore]
    public Uri? ThumbImageUrl { get; private set; }

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _)
    {
        SmallImageUrl = getImageUrlBySize("small");
        LargeImageUrl = getImageUrlBySize("large");
        ThumbImageUrl = getImageUrlBySize("thumb");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Uri? getImageUrlBySize(String size) =>
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        Images?.FirstOrDefault(image => String.Equals(size, image.Size, StringComparison.Ordinal))?.Url;

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(INewsArticle)} {{ ID = {Id:B}, Timestamp = {CreatedAtUtc:O}, Author = \"{Author}\", Headline = \"{Headline}\" }}";
}
