using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Repositories;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{

    public class UserDeletedConsumer : IConsumer<UserDeleted>
    {
        private readonly IRepository<UserEntity> repository;

        public UserDeletedConsumer(IRepository<UserEntity> repo)
        {
            repository = repo;
        }
        public async Task Consume(ConsumeContext<UserDeleted> context)
        {
            var message = context.Message;
            var user = await repository.GetAsync(message.userId);
            if (user == null)
                return;
            await repository.DeleteAsync(message.userId);
        }
    }
}