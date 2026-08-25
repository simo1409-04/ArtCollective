using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtCollective.ValidationConstraints
{
    public static class Constraints
    {
        public const int ArtistUsernameMaxLength = 30;
        public const int ArtistUsernameMinLength = 5;
        public const int ArtistEmailMinLength = 6;
        public const int ArtistEmailMaxLength = 50;
        public const int ArtistPasswordMinLength = 4;
      
        
        public const int GroupTitleMaxLength = 50;
        public const int GroupTitleMinLength = 3;



        public const int ArtworkTitleMinLength = 3;
        public const int ArtworkTitleMaxLength = 50;
        public const int ArtworkDescriptionMinLength = 10;
        public const int ArtworkDescriptionMaxLength = 300;


        public const int FeedbackContentMinength = 3;
        public const int FeedbackContentMaxength = 200;


    }
}
