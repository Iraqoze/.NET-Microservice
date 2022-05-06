using System;

namespace Play.Inventory.Service
{
    public record GrantItemsDto(Guid UserID, Guid CatalogItemId, int Quantity);
    public record InventoryItemDto(string Firstname, string Lstname, string Email, string Phone, Guid CatalogItemId, string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);
    public record CatalogItemDto(Guid Id, string Name, string Description);

}