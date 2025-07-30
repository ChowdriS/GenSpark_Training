using System;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface IContactService
{
    public Task<ContactU> SubmitContact(ContactRequestDTO dto);
}
