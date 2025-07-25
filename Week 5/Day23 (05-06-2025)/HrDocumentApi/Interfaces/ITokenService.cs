using HrDocumentApi.Models;

namespace HrDocumentApi.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}