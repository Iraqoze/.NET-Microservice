using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Repositories;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{

    public class UserUpdatedConsumer : IConsumer<UserUpdated>
    {
        private readonly IRepository<UserEntity> repository;

        public UserUpdatedConsumer(IRepository<UserEntity> repo)
        {
            repository = repo;
        }
        public async Task Consume(ConsumeContext<UserUpdated> context)
        {
            var message = context.Message;
            var user = await repository.GetAsync(message.Id);
            if (user == null)
            {
                user = new UserEntity
                {
                    Id = message.Id,
                    Firstname = message.Firstname,
                    Lastname = message.Lastname,
                    Email = message.Email,
                    PhoneNumber = message.PhoneNumber
                };
                await repository.UpdateAsync(user);
            }
            else
            {
                user.Firstname = message.Firstname;
                user.Lastname = message.Lastname;
                user.Email = message.Email;
                user.PhoneNumber = message.PhoneNumber;

                await repository.UpdateAsync(user);
            }

        }
    }
}