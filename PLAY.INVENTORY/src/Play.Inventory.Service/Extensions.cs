using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item, string fname, string lname, string email, string phone, string name, string description)
        {
            return new InventoryItemDto(fname, lname, email, phone, item.CatalogItemId, name, description, item.Quantity, item.AcquiredDate);
        }

    }

}