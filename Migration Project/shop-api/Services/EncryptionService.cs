using System;
using shop_api.Interfaces;
using shop_api.Models;

namespace shop_api.Services;


public class EncryptionService : IEncryptionService
    {
        public  async Task<EncryptModel> EncryptData(EncryptModel data)
        {
            data.EncryptedData = BCrypt.Net.BCrypt.HashPassword(data.Data);
            return data;
        }
    }
    
