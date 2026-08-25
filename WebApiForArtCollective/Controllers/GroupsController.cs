using ArtCollective.Data;
using ArtCollective.DataProcessor.ExportDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiForArtCollective.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> GetGroups(ArtCollectiveDbContext context)
        {


            List<GroupDtoForExport> groupDtoForExport = await context.Groups.Select(x => new GroupDtoForExport
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

            }).OrderBy(x => x.StartedOn).ToListAsync();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd",
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()


            };

            string json = JsonConvert.SerializeObject(groupDtoForExport, settings);
            return Ok(json);



        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetGroup(ArtCollectiveDbContext context, int Id)
        {


            var groupForExport = await context.Groups.Include(x=>x.Feedbacks).ThenInclude(x=>x.Artist).FirstOrDefaultAsync(x => x.Id == Id);



            if(groupForExport==null)
            {
                return NotFound();
            }

            GroupDtoForExport groupDtoForExport =


            new GroupDtoForExport
            {

                Id = groupForExport.Id,
                Title = groupForExport.Title,
                StartedOn = groupForExport.StartedOn,
                Feedbacks = groupForExport.Feedbacks.Select(f => new FeedbackDTOExport
                {

                    Content = f.Content,
                    GivenOn = f.GivenOn,
                    Status = (int)f.Status,
                    ArtistUsername = f.Artist.Username

                }).OrderBy(g => g.GivenOn).ToList()

            };

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd",
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()


            };

            string json = JsonConvert.SerializeObject(groupDtoForExport, settings);
            return Ok(json);



        }

    }
}
