using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Repositories;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreated>
    {
        private readonly IRepository<UserEntity> repository;

        public UserCreatedConsumer(IRepository<UserEntity> repo)
        {
            repository = repo;
        }


        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var message = context.Message;
            var user = await repository.GetAsync(message.Id);
            if (user != null)
                return;
            user = new UserEntity
            {
                Id = message.Id,
                Firstname = message.Firstname,
                Lastname = message.Lastname,
                Email = message.Email,
                PhoneNumber = message.PhoneNumber
            };
            await repository.CreateAsync(user);
        }

    }

}