using ArtCollective.Data;
using ArtCollective.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtCollective.DataProcessor.ExportDTOs
{
    public class FeedbackDTOExport
    {

public string Content { get; set; } = null!;
        public DateTime GivenOn { get; set; }

        public int Status { get; set; }


        public string ArtistUsername { get; set; } = null!;


    }
}
