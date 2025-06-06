using System;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Interface;

public interface IUserService
{
    public Task<User> addUser(UserAddRequestDTO dto);
    public Task<User> addAdmin(UserAddRequestDTO dto);

    public Task<User> updateUser(string email, UserUpdateRequestDTO dto);

    public Task<User> changePasssword(string email, ChangePasswordDTO dto);

    public Task<User> deleteUser(string email);
}
