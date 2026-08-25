using ArtCollective.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArtCollective.ValidationConstraints.Constraints;


namespace ArtCollective.DataProcessor.ImportDTOs
{
    public class ArtworkDtoForImport
    {

        [MaxLength(ArtworkTitleMaxLength)]
        [MinLength(ArtworkTitleMinLength)]
        [Required]
        public string Title { get; set; } = null!;


        [MaxLength(ArtworkDescriptionMaxLength)]
        [MinLength(ArtworkDescriptionMinLength)]
        public string? Description { get; set; }

        [Required]
        public string CreatedOn { get; set; } = null!;

        [Required]
        public int ArtistId { get; set; }




    }
}
