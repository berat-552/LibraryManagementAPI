namespace LibraryManagementAPI.Responses;

public sealed record PartialResponse<T>
{
    public bool PartialData { get; init; }
    public List<T> Items { get; init; } = [];
}
