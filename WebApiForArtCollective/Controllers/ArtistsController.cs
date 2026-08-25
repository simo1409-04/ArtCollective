using ArtCollective.Data;
using ArtCollective.Data.Models;
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
    public class ArtistsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(ArtCollectiveDbContext context)
        {


            List<ArtistExDto> artistsForExport = await context.Artists.Select(x => new ArtistExDto
            {

                Username = x.Username,
                Collaborations = x.CollaborationWithOne.Count + x.CollaborationWithTwo.Count,
                Artworks = x.Artworks.OrderBy(a => a.Id).Select(a => new ArtworkDTOExport { Title = a.Title, CreatedOn = a.CreatedOn.ToString("dd-MM-yyyy") }).ToList()


            }).OrderBy(a => a.Username).ToListAsync();

         


            return Ok(artistsForExport);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(ArtCollectiveDbContext context, int Id)
        {


            Artist artistForExport = await context.Artists.Include(x=>x.Artworks).FirstOrDefaultAsync(x => x.Id == Id);

            if(artistForExport == null)
            {
                return NotFound();
            }
            ArtistExDto dtoForExport = new ArtistExDto
            {

                Username = artistForExport.Username,
                Collaborations = artistForExport.CollaborationWithOne.Count + artistForExport.CollaborationWithTwo.Count,
                Artworks = artistForExport.Artworks.OrderBy(a => a.Id).Select(a => new ArtworkDTOExport { Title = a.Title, CreatedOn = a.CreatedOn.ToString("dd-MM-yyyy") }).ToList()


            };


         


            return Ok(dtoForExport);
        }


    }
}
