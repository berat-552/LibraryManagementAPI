namespace LibraryManagementAPI.Responses;

public sealed class PartialResponse<T>
{
    public bool PartialData { get; set; }
    public List<T> Items { get; set; } = [];
}
