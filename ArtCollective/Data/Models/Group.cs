using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArtCollective.ValidationConstraints.Constraints;


namespace ArtCollective.Data.Models
{
    public class Group
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GroupTitleMaxLength)]
        public string Title { get; set; } = null!;


        [Required]
        public DateTime StartedOn { get; set; }



        public virtual ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();
        public virtual ICollection<ArtistGroup> ArtistsGroups { get; set; } = new HashSet<ArtistGroup>();




    }
}
