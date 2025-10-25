namespace Common.Services.IServices
{
    public interface IFileService
    {
        string AddAttachment(IFormFile file);
        Task<string> AddCompressAttachment(IFormFile file);
        void DeleteAttachment(string url);
    }
}
