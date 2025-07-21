using System;
using Microsoft.EntityFrameworkCore;
using streamingApp.Context;
using streamingApp.Models;

namespace streamingApp.Repository;


public class VideoRepository : Repository<Guid, Video>
{
    public VideoRepository(StreamContext context):base(context)
    {
        
    }
    public override async Task<Video> GetById(Guid id)
    {
        return await _streamContext.Videos.SingleOrDefaultAsync(u => u.Id == id);
    }
    public override async Task<IEnumerable<Video>> GetAll()
    {
        return await _streamContext.Videos.ToListAsync();
    }
            
}
