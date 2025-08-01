using System;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.misc;

public class ObjectMapper
{
    public UserResponseDTO UserResponseDTOMapper(User user) => new()
    {
        Username = user.Username,
        Role = user.Role.ToString()
    };
}
