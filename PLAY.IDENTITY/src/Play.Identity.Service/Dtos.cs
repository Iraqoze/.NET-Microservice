using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Service
{
    public record UserDto(string Firstname, string Lastname, string PhoneNumber, string Email);
    public record CreateUserDto([Required] string Firstname, [Required] string Lastname, [Required] string PhoneNumber, [Required] string Email);
    public record UpdateUserDto([Required] Guid Id, string Firstname, string Lastname, string PhoneNumber, string Email);
    public record DeleteUserDto([Required] Guid Id);

}