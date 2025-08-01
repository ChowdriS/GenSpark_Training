using System;
using shop_api.Interfaces;
using shop_api.misc;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Services;

public class UserService : IUserService
{
    private readonly IRepository<int, User> _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly ObjectMapper _mapper;

    public UserService(IRepository<int, User> userRepository,
                        IEncryptionService encryptionService,
                        ObjectMapper mapper)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _mapper = mapper;
    }

    private async Task<UserResponseDTO> Add(UserAddRequestDTO dto, UserRole role)
    {
        if (dto == null) throw new Exception("All fields are Required!");
        bool flag = false;
        try
        {
            var allUsers = await _userRepository.GetAll();
            var olduser = allUsers.FirstOrDefault(u => u.Username!.Equals(dto.Username));
            if (olduser != null) flag = true;
        }
        catch (Exception) { }
        if(flag == true)    throw new Exception("UserName Already Taken!");
        var passwordhash = await _encryptionService.EncryptData(new EncryptModel
        {
            Data = dto.Password
        });
        var user = new User
        {
            Username = dto.Username,
            Password = passwordhash.EncryptedData,
            Role = role
        };

        user = await _userRepository.Add(user);
        return _mapper.UserResponseDTOMapper(user);
    }

    

    public async Task<UserResponseDTO> AddUser(UserAddRequestDTO dto)
    {
        return await Add(dto, UserRole.User);
    }


    public async Task<UserResponseDTO> AddAdmin(UserAddRequestDTO dto)
    {
        return await Add(dto, UserRole.Admin);
    }

    public async Task<UserResponseDTO> GetMe(int Id)
    {
        var user = await _userRepository.GetById(Id);
        return _mapper.UserResponseDTOMapper(user);
    }

}
