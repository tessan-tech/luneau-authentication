using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Security.Cryptography;
using System.Web;

namespace LuneauAuthentication.Models
{
    public class Organization
    {
        public Organization(string name)
        {
            ApiKey = HttpUtility.UrlEncode(Convert.ToBase64String(new HMACSHA256().Key));
            Name = name;
        }

        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
    }
}
