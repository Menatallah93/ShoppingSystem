using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShoppingSystem.Models
{
    public class User
    {
        public string? ProfilePicture { get; set; }

        [Key]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser? ApplicationUser { get; set; }




    }
}
