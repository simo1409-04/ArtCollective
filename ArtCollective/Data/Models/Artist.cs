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
    public class Artist
    {
        [Key]

        public int Id { get; set; }

        [MaxLength(ArtistUsernameMaxLength)]
        [Required]
        public string Username  { get; set; }= null!;

        [MaxLength(ArtistEmailMaxLength)]
        [Required]
        public string Email { get; set; } = null!;


        [Required]
        public string Password { get; set; } = null!;



        
        public virtual ICollection<Artwork> Artworks{ get; set; } = new HashSet<Artwork>(); 
        public virtual ICollection<Feedback> Feedbacks{ get; set; } = new HashSet<Feedback>(); 
        public virtual ICollection<ArtistGroup> ArtistsGroups { get; set; } = new HashSet<ArtistGroup>();

        [InverseProperty(nameof(Collaboration.ArtistOne))]

        public virtual ICollection<Collaboration> CollaborationWithTwo { get; set; } = new HashSet<Collaboration>();

        [InverseProperty(nameof(Collaboration.ArtistTwo))]
        public virtual ICollection<Collaboration> CollaborationWithOne { get; set; } = new HashSet<Collaboration>(); 




    }

}
