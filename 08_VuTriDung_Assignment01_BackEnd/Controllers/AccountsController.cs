using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using _08_VuTriDung_Assignment01.DTOs;
using _08_VuTriDung_Assignment01.Models;
using _08_VuTriDung_Assignment01.Repositories;

namespace _08_VuTriDung_Assignment01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("odata/[controller]")]
    public class AccountsController : ODataController
    {
        private readonly ISystemAccountRepository _accountRepository;

        public AccountsController(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var accounts = _accountRepository.GetAll().AsQueryable();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public IActionResult Get([FromRoute] short id)
        {
            var account = _accountRepository.GetById(id);
            if (account == null)
            {
                return NotFound(new { message = $"Account with ID {id} not found." });
            }
            return Ok(account);
        }

        [HttpPost]
        public IActionResult Post([FromBody] SystemAccountDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = _accountRepository.GetByEmail(dto.AccountEmail);
            if (existing != null)
            {
                return BadRequest(new { message = "Email already exists in the system." });
            }

            var account = new SystemAccount
            {
                AccountID = dto.AccountID,
                AccountName = dto.AccountName,
                AccountEmail = dto.AccountEmail,
                AccountRole = dto.AccountRole,
                AccountPassword = dto.AccountPassword ?? "@1"
            };

            var created = _accountRepository.Create(account);
            return CreatedAtAction(nameof(Get), new { id = created.AccountID }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] short id, [FromBody] SystemAccountDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = _accountRepository.GetById(id);
            if (existing == null)
            {
                return NotFound(new { message = $"Account with ID {id} not found." });
            }

            var emailCheck = _accountRepository.GetByEmail(dto.AccountEmail);
            if (emailCheck != null && emailCheck.AccountID != id)
            {
                return BadRequest(new { message = "Email already in use by another account." });
            }

            existing.AccountName = dto.AccountName;
            existing.AccountEmail = dto.AccountEmail;
            existing.AccountRole = dto.AccountRole;
            if (!string.IsNullOrEmpty(dto.AccountPassword))
            {
                existing.AccountPassword = dto.AccountPassword;
            }

            var updated = _accountRepository.Update(existing);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] short id)
        {
            var result = _accountRepository.Delete(id);
            if (!result.Success)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(new { message = "Account deleted successfully." });
        }
    }
}
