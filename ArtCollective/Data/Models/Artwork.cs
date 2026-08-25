using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArtCollective.ValidationConstraints.Constraints;


namespace ArtCollective.Data.Models
{
    public class Artwork
    {

        [Key]
        public int Id { get; set; }

        [MaxLength(ArtworkTitleMaxLength)]
        [Required]
        public string Title { get; set; } = null!;


        [MaxLength(ArtworkDescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [ForeignKey(nameof(Artist))]
        [Required]
        public int ArtistId { get; set; }


        public virtual Artist Artist { get; set; } = null!;



    }
}
