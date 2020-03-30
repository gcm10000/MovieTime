using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieTimeLibraryCore
{
    public class DownloadData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Quality { get; set; }
        public string Audio { get; set; }
        public string Format { get; set; }
        public string Size { get; set; }
        public int SeasonTV { get; set; }
        public string EpisodeTV { get; set; }
        [Required]
        public string Magnet { get; set; }
        [JsonIgnore]
        public Watch Watch { get; set; }
    }
}
