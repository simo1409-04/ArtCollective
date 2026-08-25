using ArtCollective.Data;
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
    public class FeedbackDtoJsonImport
    {
       


        [MaxLength(FeedbackContentMaxength)]
        [MinLength(FeedbackContentMinength)]
        [Required]
            public string Content { get; set; } = null!;

        [Required]

        public string GivenOn { get; set; } = null!;

        [Required]
        public string Status { get; set; } = null!;

        [Required]

        public int GroupId { get; set; }




        [Required]

        public int ArtistId { get; set; }




        

    
}
}
