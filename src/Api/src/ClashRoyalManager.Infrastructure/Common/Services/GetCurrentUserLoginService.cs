using System.IdentityModel.Tokens.Jwt;
using ClashRoyalManager.Application.Common.Interfaces.Repository;
using ClashRoyalManager.Application.Entities;
using ClashRoyalManager.Application.Services;

namespace ClashRoyalManager.Infrastructure.Services;

public class GetCurrentUserLoginService: IGetCurrentUserLoginService
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserLoginService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(string token)
    {
        // Decodifica el token para obtener el ID del usuario
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub);
        var userId = Guid.Parse(userIdClaim.Value);

        // Busca al usuario en la base de datos usando el ID obtenido
        if (await _userRepository.GetUserById(userId) is not User user)
        {
            return null;
        }

        return user;
    }
}