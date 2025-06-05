using HrDocumentApi.Models;

namespace HrDocumentApi.Interfaces
{
    public interface IEncryptionService
    {
         public Task<EncryptModel> EncryptData(EncryptModel data);
    }
}