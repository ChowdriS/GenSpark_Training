using HrDocumentApi.Models;
using HrDocumentApi.Models.DTOs;

namespace HrDocumentApi.Interfaces;

public interface IUserService
{
    public Task<User> CreateUser(UserRequestDto userRequestDto);
    public Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto);
    public Task<User> GetUser(string name);
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<User> UpdateUser(int id, UserRequestDto userRequestDto);
    public Task DeleteUser(string name);
}