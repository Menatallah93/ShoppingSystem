using ShoppingSystem.Repository;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShoppingSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now.Date;
        public bool IsDeleted { get; set; } = false;
        public string? ProductPicture { get; set; }


        [ForeignKey("Categories")]
        public int CategoriesID { get; set; }

        [JsonIgnore]
        public Categories? Categories { get; set; }

    }
}
