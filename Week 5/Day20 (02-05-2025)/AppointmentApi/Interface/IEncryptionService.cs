using System;
using AppointmentApi.Models;

namespace AppointmentApi.Interface;

public interface IEncryptionService
    {
        public Task<EncryptModel> EncryptData(EncryptModel data);
    }
