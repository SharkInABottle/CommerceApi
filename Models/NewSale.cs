using CommerceApi.Entities;
using System.Text.Json.Serialization;

namespace CommerceApi.Models
{
    public class NewSale
    {  
        public string Title { get; set; }
        public string Body { get; set; }             
        public string Region { get; set; }
        public int Price { get; set; }
        public string Categorie { get; set; }
        public int PhoneNumber { get; set; }
        public List<NewImages> Images { get; set; }

    }
    public class NewImages
    {
        public string Id { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
    }
}
