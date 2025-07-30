using System.Text.Json;
using shop_api.Context;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Services;

public class ContactService : IContactService
{
    private readonly IRepository<int, ContactU> _contactRepository;

    public ContactService(IRepository<int, ContactU> contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<ContactU> SubmitContact(ContactRequestDTO dto)
    {
        if (dto == null)
            throw new Exception("ContactRequestDTO cannot be null.");

        if (dto.Name == null || dto.Name.Trim() == "")
            throw new Exception("Name is required.");

        if (dto.Email == null || dto.Email.Trim() == "")
            throw new Exception("Email is required.");

        if (dto.Phone == null || dto.Phone.Trim() == "")
            throw new Exception("Phone is required.");

        if (dto.Content == null || dto.Content.Trim() == "")
            throw new Exception("Content is required.");

        var contact = new ContactU
        {
            name = dto.Name.Trim(),
            email = dto.Email.Trim(),
            phone = dto.Phone.Trim(),
            content = dto.Content.Trim()
        };

        await _contactRepository.Add(contact);

        return contact;
    }
}
