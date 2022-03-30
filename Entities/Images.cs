
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CommerceApi.Entities
{
    public class Images
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
        public int SaleId { get; set; }
        
        [JsonIgnore]
        public Sale Sale { get; set; }
    }
}
