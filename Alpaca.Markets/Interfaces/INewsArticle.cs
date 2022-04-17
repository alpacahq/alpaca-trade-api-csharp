namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the basic news article information from Alpaca APIs.
/// </summary>
public interface INewsArticle
{
    /// <summary>
    /// Gets news article unique identifier.
    /// </summary>
    [UsedImplicitly]
    public Int64 Id { get; }

    /// <summary>
    /// Gets headline or title of the article.
    /// </summary>
    [UsedImplicitly]
    public String Headline { get; }

    /// <summary>
    /// Gets news article creation timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    public DateTime CreatedAtUtc { get; }

    /// <summary>
    /// Gets news article updating timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    public DateTime UpdatedAtUtc { get; }

    /// <summary>
    /// Gets original author of news article.
    /// </summary>
    [UsedImplicitly]
    public String Author { get; }

    /// <summary>
    /// Gets summary text for the article (may be first sentence of content).
    /// </summary>
    [UsedImplicitly]
    public String Summary { get; }

    /// <summary>
    /// Gets content of the news article (might contain HTML).
    /// </summary>
    [UsedImplicitly]
    public String Content { get; }

    /// <summary>
    /// Gets URL of article (if applicable).
    /// </summary>
    [UsedImplicitly]
    public Uri? ArticleUrl { get; }

    /// <summary>
    /// Gets source where the news originated from.
    /// </summary>
    [UsedImplicitly]
    public String Source { get; }

    /// <summary>
    /// Gets list of related or mentioned symbols.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyList<String> Symbols { get; }

    /// <summary>
    /// Gets the thumbnail image URL for the news article.
    /// </summary>
    [UsedImplicitly]
    public Uri? ThumbImageUrl { get; }

    /// <summary>
    /// Gets the small image URL for the news article.
    /// </summary>
    [UsedImplicitly]
    public Uri? SmallImageUrl { get; }

    /// <summary>
    /// Gets the large image URL for the news article.
    /// </summary>
    [UsedImplicitly]
    public Uri? LargeImageUrl { get; }
}
