using _08_VuTriDung_Assignment01.DTOs;
using _08_VuTriDung_Assignment01.Models;
using Microsoft.Extensions.Configuration;

namespace _08_VuTriDung_Assignment01.Repositories
{
    public interface ISystemAccountRepository
    {
        IEnumerable<SystemAccount> GetAll();
        SystemAccount? GetById(short id);
        SystemAccount? GetByEmail(string email);
        SystemAccount Create(SystemAccount account);
        SystemAccount? Update(SystemAccount account);
        (bool Success, string? ErrorMessage) Delete(short id);
        LoginResponseDTO? Authenticate(string email, string password, IConfiguration configuration);
    }
}
