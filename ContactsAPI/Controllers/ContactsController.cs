using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;

        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetContactById([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null)
                return Ok(contact);
         
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(ContactRequest contactReq)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = contactReq.Address,
                Email = contactReq.Email,
                FullName = contactReq.FullName,
                Phone = contactReq.Phone
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, ContactRequest contactReq)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null)
            {
                contact.Address = contactReq.Address;
                contact.Email = contactReq.Email;
                contact.FullName = contactReq.FullName;
                contact.Phone = contactReq.Phone;

                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            
            return NotFound();
        }
    }
}
