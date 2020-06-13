using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public class Group
    {
        [JsonPropertyName("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [NotMapped]
        [JsonIgnore]
        public bool IsChecked { get; set; }
    }
}
