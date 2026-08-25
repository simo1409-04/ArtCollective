using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtCollective.Data.Models
{
    public class Collaboration
    {
        [Required]
        [ForeignKey(nameof(ArtistOne))]
        public int ArtistOneId { get; set; }


        public virtual Artist ArtistOne { get; set; } = null!;

        [ForeignKey(nameof(ArtistTwo))]
        [Required]

        public int ArtistTwoId { get; set; }


        public virtual Artist ArtistTwo { get; set; } = null!;

    }
}
