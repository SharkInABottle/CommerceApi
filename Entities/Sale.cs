using CommerceApi.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CommerceApi.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string Categorie { get; set; }
        public int Price { get; set; }
        public int PhoneNumber { get; set; }
        
        //foreign keys and navigation properties
        public string UserClassID { get; set; }
        
        public UserClass UserClass { get; set; }       
        public List<Images> Images { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public bool ImagesDeleteError { get; set; }
        
        
    }
}
