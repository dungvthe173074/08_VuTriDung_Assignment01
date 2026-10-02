using System.ComponentModel.DataAnnotations;

namespace _08_VuTriDung_Assignment01.DTOs
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDTO
    {
        public short AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public int AccountRole { get; set; } // 0 = Admin, 1 = Staff, 2 = Lecturer
        public string RoleName { get; set; } = string.Empty;
    }
}
