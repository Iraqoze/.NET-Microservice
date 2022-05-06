using Play.Common.Repositories;
using Play.Inventory.Service.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Play.Inventory.Service.Clients;

namespace Play.Inventory.Service.Controllers
{
    [Route("items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> inventoryItemsRepository;
        private readonly IRepository<CatalogItem> catalogItemsRepository;
        private readonly IRepository<UserEntity> userEntityRepository;
        public ItemsController(IRepository<InventoryItem> inventoryItemsRepo, IRepository<CatalogItem> catalogItemsRepo, IRepository<UserEntity> userEntityRepo)
        {
            inventoryItemsRepository = inventoryItemsRepo;
            catalogItemsRepository = catalogItemsRepo;
            userEntityRepository = userEntityRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();

            var user = await userEntityRepository.GetAsync(userId);
            var inventoryItemEntities = await inventoryItemsRepository.GetAllAsync(item => item.UserId == userId);
            var itemIds = inventoryItemEntities.Select(item => item.CatalogItemId);
            var catalogItemEntities = await catalogItemsRepository.GetAllAsync(item => itemIds.Contains(item.Id));


            var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
            {

                var catalogItem = catalogItemEntities.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(user.Firstname, user.Lastname, user.Email, user.PhoneNumber, catalogItem.Name, catalogItem.Description);

            });
            return Ok(inventoryItemDtos);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await inventoryItemsRepository.GetAsync(item => item.UserId == grantItemsDto.UserID && item.CatalogItemId == grantItemsDto.CatalogItemId);
            if (inventoryItem == null)
            {
                InventoryItem item = new InventoryItem
                {
                    UserId = grantItemsDto.UserID,
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    Id = Guid.NewGuid(),
                    AcquiredDate = DateTimeOffset.UtcNow,
                    Quantity = grantItemsDto.Quantity
                };

                await inventoryItemsRepository.CreateAsync(item);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await inventoryItemsRepository.UpdateAsync(inventoryItem);
            }


            return Ok();
        }
    }
}


//5hours 43 Min => Defining Message Consumer