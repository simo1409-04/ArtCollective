using ArtCollective.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtCollective.DataProcessor.ExportDTOs
{
    public class GroupDtoForExport
    {

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public DateTime StartedOn { get; set; }

        public List<FeedbackDTOExport> Feedbacks { get; set; } = new();

    }
}
