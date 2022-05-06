using System;
using Play.Common.Entities;

namespace Play.Identity.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

}