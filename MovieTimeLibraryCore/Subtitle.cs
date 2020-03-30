using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieTimeLibraryCore
{
    public class Subtitle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Lang { get; set; }
        public string DownloadText { get; set; }
        [JsonIgnore]
        public Watch Watch { get; set; }

    }
}
