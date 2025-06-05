using HrDocumentApi.Repositories;
using HrDocumentApi.Contexts;
using HrDocumentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HrDocumentApi.Repositories;
public class FileRepository : Repository<int, FileModel>
{
    public FileRepository(HrDocumentApiContext HrDocumentApiContext) : base(HrDocumentApiContext)
    {
    }

    public override async Task<FileModel> Get(int key)
    {
        var file = await _HrDocumentApiContext.Files.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == key);
        return file ?? throw new KeyNotFoundException($"File with ID {key} not found.");
    }

    public override async Task<IEnumerable<FileModel>> GetAll()
    {
        var files = await _HrDocumentApiContext.Files.Include(f => f.User).ToListAsync();
        return files ?? new List<FileModel>();
    }
}