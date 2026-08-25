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
    public class FeedbacksController : ControllerBase
    {
        private readonly ArtCollectiveDbContext context;
        public FeedbacksController(ArtCollectiveDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public async Task<IActionResult> PostFeedBack( List<FeedbackDtoJsonImport> fedbacksForValidate)
        {



            List<FeedbackDtoJsonImport> addedSuccsesfylly = new List<FeedbackDtoJsonImport>();

            List<Feedback> feedbacksInDatabase = await context.Feedbacks.AsNoTracking().ToListAsync();
            List<Feedback> feedbacksForImportInDatabase = new List<Feedback>();

            List<int> groupIds = context.Groups.Select(x => x.Id).ToList();
            List<int> artistsIds = context.Artists.Select(x => x.Id).ToList();





            foreach (var feedback in fedbacksForValidate)
            {


                if (Deserializer.IsValid(feedback))
                {
                    string[] validEnums = Enum.GetNames<Status>();

                    string requiredFormat = "yyyy-MM-dd";

                    bool isEnumValid = validEnums.Contains(feedback.Status);

                    bool isDateValid = DateTime.TryParseExact(
                        feedback.GivenOn,
                        requiredFormat,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime parsedDate);

                    bool isGroupIdExistingInDb = groupIds.Contains(feedback.GroupId);
                    bool isArtistIdExistsInDb = artistsIds.Contains(feedback.ArtistId);

                    if (isEnumValid && isDateValid && isGroupIdExistingInDb && isArtistIdExistsInDb)
                    {
                        Status status = Enum.Parse<Status>(feedback.Status);

                        bool isDuplicate = feedbacksInDatabase.Any(x => (x.Content == feedback.Content && x.GivenOn == parsedDate && status == x.Status) && x.ArtistId == feedback.ArtistId && x.GroupId == feedback.GroupId);

                        if (!isDuplicate)
                        {
                            Feedback feedbackForDb = new Feedback { Content = feedback.Content, GivenOn = parsedDate, Status = status, GroupId = feedback.GroupId, ArtistId = feedback.ArtistId };

                            addedSuccsesfylly.Add(feedback);
                            feedbacksInDatabase.Add(feedbackForDb);
                            feedbacksForImportInDatabase.Add(feedbackForDb);


                        }




                    }
                    






                }
               


            }

            if(feedbacksForImportInDatabase.Count==0)
            {

                return BadRequest();
            }


            context.Feedbacks.AddRange(feedbacksForImportInDatabase);
           await context.SaveChangesAsync();

            


          return  Ok(addedSuccsesfylly);
        


    }

}
}
