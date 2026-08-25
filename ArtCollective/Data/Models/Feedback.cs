using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ArtCollective.ValidationConstraints.Constraints;

namespace ArtCollective.Data.Models
{
    public class Feedback
    {


        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(FeedbackContentMaxength)]
        public string Content { get; set; } = null!;
        public DateTime GivenOn { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        [ForeignKey(nameof(Group))]

        public int GroupId  { get; set; }



        public virtual Group Group { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Artist))]
        public int ArtistId { get; set; }


        public virtual Artist Artist { get; set; } = null!;


    }

}
