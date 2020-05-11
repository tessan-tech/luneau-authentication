using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LuneauAuthentication.Models
{
    public class Organization
    {
        public Organization()
        {
            ApiKey = Convert.ToBase64String(new HMACSHA256().Key);
        }

        [BsonId]
        public Guid Id { get; set; }
        public string ApiKey { get; set; }
    }
}
