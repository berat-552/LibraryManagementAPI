namespace LibraryManagementAPI.Responses;

public class PartialResponse<T>
{
    public bool PartialData { get; set; }
    public List<T> Items { get; set; } = [];
}
