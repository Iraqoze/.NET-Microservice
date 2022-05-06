using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Play.Common.Repositories;
using Play.Identity.Entities;

namespace Play.Identity.Service.Controllers
{
    [ApiController]
    [Route("users")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IPublishEndpoint publishEndPoint;

        public ItemsController(IRepository<User> usersRepository, IPublishEndpoint publishEP)
        {
            _usersRepository = usersRepository;
            publishEndPoint = publishEP;
        }



        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAsync()
        {
            var users = (await _usersRepository.GetAllAsync()).Select(u => u.AsDto());
            return users;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id)
        {
            var user = await _usersRepository.GetAsync(id);
            if (user is null)
                return NotFound();
            return user.AsDto();
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Firstname = createUserDto.Firstname,
                Lastname = createUserDto.Lastname,
                PhoneNumber = createUserDto.PhoneNumber,
                Email = createUserDto.Email,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _usersRepository.CreateAsync(user);
            await publishEndPoint.Publish(new UserCreated(user.Id, user.Firstname, user.Lastname, user.PhoneNumber, user.Email));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(UpdateUserDto updateUserDto, Guid id)
        {
            var existingUser = await _usersRepository.GetAsync(id);
            if (existingUser is null)
                return NotFound();

            existingUser.Firstname = updateUserDto.Firstname;
            existingUser.Lastname = updateUserDto.Lastname;
            existingUser.PhoneNumber = updateUserDto.PhoneNumber;
            existingUser.Email = updateUserDto.Email;

            await _usersRepository.UpdateAsync(existingUser);
            await publishEndPoint.Publish(new UserUpdated(existingUser.Id, existingUser.Firstname, existingUser.Lastname, existingUser.PhoneNumber, existingUser.Email));
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingUser = await _usersRepository.GetAsync(id);
            if (existingUser is null)
                return NotFound();

            await _usersRepository.DeleteAsync(id);
            await publishEndPoint.Publish(new UserDeleted(id));
            return NoContent();
        }

    }
}



//5 HOURs 25 min => RabbitMQ Settings