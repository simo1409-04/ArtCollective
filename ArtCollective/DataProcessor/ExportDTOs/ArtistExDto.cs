using ArtCollective.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArtCollective.ValidationConstraints.Constraints;


namespace ArtCollective.DataProcessor.ExportDTOs
{
    public class ArtistExDto
    {
       

  
        public string Username { get; set; } = null!;

        public int Collaborations { get; set; }

        public virtual ICollection<ArtworkDTOExport> Artworks { get; set; } = new List<ArtworkDTOExport>();
        





    }
}
