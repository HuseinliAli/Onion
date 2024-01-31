using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public record UserForAuthDto(
       [Required] string UserName,
       [Required] string Password
       );

}
