namespace Common.Helper.EntityParams
{
    public class UserParams : PaginationParams
    {
        public string? UserCode { get; set; }
        public string? FullName { get; set; }
    }
}
