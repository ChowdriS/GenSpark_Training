using System;
using shop_api.Models;

namespace shop_api.Interfaces;

public interface IEncryptionService
    {
        public Task<EncryptModel> EncryptData(EncryptModel data);
    }