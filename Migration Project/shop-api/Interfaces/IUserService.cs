using System;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface IUserService
{

    public Task<UserResponseDTO> AddUser(UserAddRequestDTO dto);
    public Task<UserResponseDTO> AddAdmin(UserAddRequestDTO dto);
    public Task<UserResponseDTO> GetMe(int Id);

}
