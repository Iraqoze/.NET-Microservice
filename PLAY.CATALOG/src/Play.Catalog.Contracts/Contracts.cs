using System;

namespace Play.Catalog.Contracts
{

    public record CatalogItemCreated(Guid ItemId, string Name, string Description);
    public record CatalogItemUpdated(Guid ItemId, string Name, string Description);
    public record CatalogItemDeleted(Guid ItemId);


    public record UserCreated(Guid Id, string Firstname, string Lastname, string PhoneNumber, string Email);
    public record UserUpdated(Guid Id, string Firstname, string Lastname, string PhoneNumber, string Email);
    public record UserDeleted(Guid userId);
}
