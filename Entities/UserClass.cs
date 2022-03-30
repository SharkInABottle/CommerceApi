using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CommerceApi.Entities
{
    [Keyless]
    public class SalesListId
    {
        public int salesId { get; set; }
    }
    public class UserClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime RegistredTime { get; set; }
        [NotMapped]
        public List<int> SalesListId { get; set; }
        [JsonIgnore]
        public List<Sale>? sales { get; set; }
    }
    

}
