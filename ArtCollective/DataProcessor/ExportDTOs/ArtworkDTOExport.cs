using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtCollective.DataProcessor.ExportDTOs
{
    public class ArtworkDTOExport
    {

        public string Title { get; set; } = null!;

        public string CreatedOn { get; set; } = null!;

    }
}
