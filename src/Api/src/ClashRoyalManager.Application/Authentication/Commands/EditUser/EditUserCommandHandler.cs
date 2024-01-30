using System.IO;
using System.Reflection.Metadata;
using ClashRoyalManager.Domain.Common.Errors;
using ClashRoyalManager.Application.Common.Interfaces.Authentication;
using ClashRoyalManager.Application.Common.Interfaces.Repository;
using ClashRoyalManager.Application.Entities;
using ClashRoyalManager.Application.DTO.Authentication;
using ClashRoyalManager.Application.Authentication.Services;
using FluentValidation;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace ClashRoyalManager.Application.Authentication.Commands.EditUser;

public class EditUserCommandHandler : IRequestHandler<EditUserCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly PasswordService _passwordService;
    private readonly IValidator<EditUserCommand> _validator;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public EditUserCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IValidator<EditUserCommand> validator,
        PasswordService passwordService,
        IWebHostEnvironment hostingEnvironment)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _validator = validator;
        _passwordService = passwordService;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            return Errors.Model.ModelsInvalid;
        }

        if (await _userRepository.GetUserByEmail(command.Email) is not User user)
        {
            return Errors.User.DuplicateEmail;
        }

        string dir = _hostingEnvironment.ContentRootPath;

        if (command.File is not null && command.File.Length >= 0)
        {
            var folderPath = Path.Combine(dir, "src", "assets", "images");
            Directory.CreateDirectory(folderPath);

            string oldFilePath = Path.Combine(dir, user.Photo);
            string filePath = Path.Combine(folderPath, command.File.FileName);

            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }

            using (var stream = File.Create(filePath))
            {
                await command.File.CopyToAsync(stream);
            }
        }

        user.Photo = "src/assets/images/" + command.File.FileName;
        user.Nombres = command.FirstName;
        user.Apellidos = command.LastName;
        user.Correo = command.Email;
        user.Password = _passwordService.HashPassword(command.Password);

        _userRepository.EditUser(user);

        return new AuthenticationResult(
            user,
            "token-edit-user"
        );
    }
}
