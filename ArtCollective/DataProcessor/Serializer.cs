using ArtCollective.Data;
using ArtCollective.DataProcessor.ExportDTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ArtCollective.DataProcessor
{
    public class Serializer
    {
        public static string ExportArtistsWithCollaborationsCountAndTheirArtworks(ArtCollectiveDbContext dbContext)
        {

            List<ArtistExDto> artistsForExport = dbContext.Artists.Select(x => new ArtistExDto
            {

                Username = x.Username,
                Collaborations = x.CollaborationWithOne.Count + x.CollaborationWithTwo.Count,
                Artworks = x.Artworks.OrderBy(a => a.Id).Select(a => new ArtworkDTOExport { Title = a.Title, CreatedOn = a.CreatedOn.ToString() }).ToList()


            }).OrderBy(a=>a.Username).ToList();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd",
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()


            };

            string json = JsonConvert.SerializeObject(artistsForExport, settings);

            return json;

        }
        public static string ExportGroupsWithFeedbacksChronologically(ArtCollectiveDbContext dbContext)
        {

            List<GroupDtoForExport> groupDtoForExport = dbContext.Groups.Select(x => new GroupDtoForExport
            {

                Id = x.Id,
                Title = x.Title,
                StartedOn = x.StartedOn,
                Feedbacks = x.Feedbacks.Select(f => new FeedbackDTOExport
                {

                    Content = f.Content,
                    GivenOn = f.GivenOn,
                    Status = (int)f.Status,
                    ArtistUsername = f.Artist.Username

                }).OrderBy(g => g.GivenOn).ToList()

            }).OrderBy(x => x.StartedOn).ToList();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd",
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()


            };

            string json = JsonConvert.SerializeObject(groupDtoForExport, settings);

            return json;


        }
    }
}
