using _08_VuTriDung_Assignment01.DAOs;
using _08_VuTriDung_Assignment01.DTOs;
using _08_VuTriDung_Assignment01.Models;
using Microsoft.Extensions.Configuration;

namespace _08_VuTriDung_Assignment01.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public IEnumerable<SystemAccount> GetAll()
        {
            return SystemAccountDAO.Instance.GetAll();
        }

        public SystemAccount? GetById(short id)
        {
            return SystemAccountDAO.Instance.GetById(id);
        }

        public SystemAccount? GetByEmail(string email)
        {
            return SystemAccountDAO.Instance.GetByEmail(email);
        }

        public SystemAccount Create(SystemAccount account)
        {
            return SystemAccountDAO.Instance.Create(account);
        }

        public SystemAccount? Update(SystemAccount account)
        {
            return SystemAccountDAO.Instance.Update(account);
        }

        public (bool Success, string? ErrorMessage) Delete(short id)
        {
            // Strict constraint: cannot delete if account has created any news articles
            if (SystemAccountDAO.Instance.HasCreatedArticles(id))
            {
                return (false, "Cannot delete account because it has created news articles.");
            }

            bool deleted = SystemAccountDAO.Instance.Delete(id);
            if (!deleted)
            {
                return (false, "Account not found or could not be deleted.");
            }

            return (true, null);
        }

        public LoginResponseDTO? Authenticate(string email, string password, IConfiguration configuration)
        {
            // 1. Check Admin Credentials from appsettings.json
            var adminEmail = configuration["AdminAccount:Email"] ?? "admin@FUNewsManagementSystem.org";
            var adminPassword = configuration["AdminAccount:Password"] ?? "@@abc123@@";

            if (string.Equals(email.Trim(), adminEmail.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(password, adminPassword))
            {
                return new LoginResponseDTO
                {
                    AccountID = 0,
                    AccountName = "Administrator",
                    AccountEmail = adminEmail,
                    AccountRole = 0, // 0 = Admin
                    RoleName = "Admin"
                };
            }

            // 2. Check Database for Staff / Lecturer
            var account = SystemAccountDAO.Instance.GetByEmail(email);
            if (account != null && account.AccountPassword == password)
            {
                string roleName = account.AccountRole switch
                {
                    1 => "Staff",
                    2 => "Lecturer",
                    _ => "User"
                };

                return new LoginResponseDTO
                {
                    AccountID = account.AccountID,
                    AccountName = account.AccountName ?? string.Empty,
                    AccountEmail = account.AccountEmail ?? string.Empty,
                    AccountRole = account.AccountRole ?? 2,
                    RoleName = roleName
                };
            }

            return null;
        }
    }
}
