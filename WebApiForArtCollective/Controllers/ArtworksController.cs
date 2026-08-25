using ArtCollective.Data;
using ArtCollective.Data.Models;
using ArtCollective.DataProcessor.ImportDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using ArtCollective.DataProcessor;

namespace WebApiForArtCollective.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworksController : ControllerBase
    {
        private readonly ArtCollectiveDbContext dbContext;

        public ArtworksController(ArtCollectiveDbContext context)
        {

            dbContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var artworks = await dbContext.Artworks.Include(x => x.Artist).Select(x => new { x.Title, x.Description, x.CreatedOn, Artist=x.Artist.Username }).ToListAsync();

            return Ok(artworks);
           



        }
        [HttpPost]
        public async Task<IActionResult> PostArtworks(List<ArtworkDtoForImport> artworksForValidate)
        {

            List<ArtworkDtoForImport> sucsessfullyAddedArtworks = new List<ArtworkDtoForImport>();
            List<Artwork> ArtworksInDatabase = await dbContext.Artworks.Include(x => x.Artist).AsNoTracking().ToListAsync();
            List<Artwork> artworksForImportInDatabase = new List<Artwork>();

            var artistsIdsAndThierUserNames = dbContext.Artists.Select(x => new { Id = x.Id, Username = x.Username }).ToList();





            foreach (var artowrk in artworksForValidate)
            {


                if (Deserializer.IsValid(artowrk))
                {

                    string requiredFormat = "yyyy-MM-dd";


                    bool isDateValid = DateTime.TryParseExact(
                        artowrk.CreatedOn,
                        requiredFormat,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime parsedDate);

                    bool isArtistIdExistsInDb = artistsIdsAndThierUserNames.Select(x => x.Id).Contains(artowrk.ArtistId);

                    if (isDateValid && isArtistIdExistsInDb)
                    {

                        bool isDuplicate = ArtworksInDatabase.Any(x => x.Title == artowrk.Title && x.ArtistId == artowrk.ArtistId);

                        if (!isDuplicate)
                        {


                            Artwork artworkForDb = new Artwork { Title = artowrk.Title, Description = artowrk.Description, CreatedOn = parsedDate, ArtistId = artowrk.ArtistId };
                            string artistUsername = artistsIdsAndThierUserNames.Where(x => x.Id == artworkForDb.ArtistId).Select(x => x.Username).First().ToString();

                            ArtworksInDatabase.Add(artworkForDb);
                            artworksForImportInDatabase.Add(artworkForDb);
                            sucsessfullyAddedArtworks.Add(artowrk);
                        }



                    }
                 






                }
                
           


            }
            if(sucsessfullyAddedArtworks.Count==0)
            {
                return BadRequest();
            }


            dbContext.Artworks.AddRange(artworksForImportInDatabase);
           await dbContext.SaveChangesAsync();

            return Ok(sucsessfullyAddedArtworks);



        }


    }
}
