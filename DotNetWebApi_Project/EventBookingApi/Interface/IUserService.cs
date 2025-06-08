using System;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Interface;

public interface IUserService
{
    public Task<User> AddUser(UserAddRequestDTO dto);
    public Task<User> AddManager(UserAddRequestDTO dto);
    public Task<User> AddAdmin(UserAddRequestDTO dto);
    public Task<MyUserResponseDTO> GetMe(string email);
    public Task<User> updateUser(Guid Id, UserUpdateRequestDTO dto);

    public Task<User> changePasssword(Guid Id, ChangePasswordDTO dto);

    public Task<User> deleteUser(Guid Id);
}
