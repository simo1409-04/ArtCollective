using ArtCollective.Data;
using ArtCollective.Data.Models;
using ArtCollective.DataProcessor.ImportDTOs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArtCollective.DataProcessor
{
    public class Deserializer
    {
        public const string ErrorMessage = "Invalid data format.";
        public const string DuplicatedData = "Data is duplicated.";
        public const string SuccessfullyImportedFeedbackEntity = "Successfully imported feedback (Given on: {0}, Status: {1})";
        public const string SuccessfullyImportedArtworkEntity = "Successfully imported artwork (Artist: {0}, Created on: {1})";

        public static string ImportFeedbacks(ArtCollectiveDbContext dbContext, string jsonString)
        {

            StringBuilder builder = new StringBuilder();
            List<FeedbackDtoJsonImport> fedbacksForValidate = JsonConvert.DeserializeObject<List<FeedbackDtoJsonImport>>(jsonString);

            List<Feedback> feedbacksInDatabase = dbContext.Feedbacks.AsNoTracking().ToList();
            List<Feedback> feedbacksForImportInDatabase = new List<Feedback>();

            List<int> groupIds = dbContext.Groups.Select(x => x.Id).ToList();
            List<int> artistsIds = dbContext.Artists.Select(x => x.Id).ToList();

            



            foreach(var feedback in fedbacksForValidate)
            {


                if(IsValid(feedback))
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

                    if(isEnumValid&& isDateValid&& isGroupIdExistingInDb && isArtistIdExistsInDb)
                    {
                        Status status = Enum.Parse<Status>(feedback.Status);

                        bool isDuplicate = feedbacksInDatabase.Any(x => (x.Content == feedback.Content && x.GivenOn == parsedDate && status == x.Status) && x.ArtistId == feedback.ArtistId && x.GroupId == feedback.GroupId);

                        if(isDuplicate)
                        {

                            builder.AppendLine(DuplicatedData);

                        }

                        else
                        {

                            Feedback feedbackForDb = new Feedback { Content = feedback.Content, GivenOn = parsedDate, Status = status, GroupId = feedback.GroupId, ArtistId = feedback.ArtistId };


                            feedbacksInDatabase.Add(feedbackForDb);
                            feedbacksForImportInDatabase.Add(feedbackForDb);
                            builder.AppendLine(string.Format(SuccessfullyImportedFeedbackEntity, parsedDate.ToString("yyyy-MM-dd"), status));

                        }


                    }
                    else
                    {
                        builder.AppendLine(ErrorMessage);

                    }






                }
                else
                {
                    builder.AppendLine(ErrorMessage);

                }


            }


            dbContext.Feedbacks.AddRange(feedbacksForImportInDatabase);
            dbContext.SaveChanges();

            return builder.ToString();

        }

        public static string ImportArtworks(ArtCollectiveDbContext dbContext, string jsonString)
        {

            StringBuilder builder = new StringBuilder();
            List<ArtworkDtoForImport> artworksForValidate = JsonConvert.DeserializeObject<List<ArtworkDtoForImport>>(jsonString);

            List<Artwork> ArtworksInDatabase = dbContext.Artworks.Include(x=>x.Artist).AsNoTracking().ToList();
            List<Artwork> artworksForImportInDatabase = new List<Artwork>();

            var artistsIdsAndThierUserNames = dbContext.Artists.Select(x => new { Id = x.Id, Username = x.Username }).ToList();





            foreach (var artowrk in artworksForValidate)
            {


                if (IsValid(artowrk))
                {

                    string requiredFormat = "yyyy-MM-dd";


                    bool isDateValid = DateTime.TryParseExact(
                        artowrk.CreatedOn,
                        requiredFormat,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime parsedDate);

                    bool isArtistIdExistsInDb = artistsIdsAndThierUserNames.Select(x=>x.Id).Contains(artowrk.ArtistId);

                    if (isDateValid  && isArtistIdExistsInDb)
                    {

                        bool isDuplicate = ArtworksInDatabase.Any(x=>x.Title==artowrk.Title && x.ArtistId==artowrk.ArtistId);

                        if (isDuplicate)
                        {

                            builder.AppendLine(DuplicatedData);

                        }

                        else
                        {

                            Artwork artworkForDb = new Artwork { Title=artowrk.Title, Description=artowrk.Description, CreatedOn=parsedDate, ArtistId=artowrk.ArtistId};
                            string artistUsername = artistsIdsAndThierUserNames.Where(x => x.Id == artworkForDb.ArtistId).Select(x=>x.Username).First().ToString();

                            ArtworksInDatabase.Add(artworkForDb);
                            artworksForImportInDatabase.Add(artworkForDb);
                            builder.AppendLine(string.Format(SuccessfullyImportedArtworkEntity, artistUsername,parsedDate.ToString("yyyy-MM-dd")));

                        }


                    }
                    else
                    {
                        builder.AppendLine(ErrorMessage);

                    }






                }
                else
                {
                    builder.AppendLine(ErrorMessage);

                }


            }


            dbContext.Artworks.AddRange(artworksForImportInDatabase);
            dbContext.SaveChanges();

            return builder.ToString();



        }

        public static bool IsValid(object dto)
        {
            ValidationContext validationContext = new ValidationContext(dto);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            List<string> errorMessages = new List<string>();
            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            errorMessages = validationResults.Select(r => r.ErrorMessage!).ToList();

            return isValid;
        }
    }
}
