using LainTwitterBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace LainTwitterBot.Services
{
    public class BotExecution : IBotExecution
    {
        private readonly TwitterClient _twitterClient;
        private readonly IImagesProvider _imagesProvider;

        public BotExecution(TwitterClient twitterClient, IImagesProvider imagesProvider)
        {
            _twitterClient = twitterClient;
            _imagesProvider = imagesProvider;
        }

        public async Task SendImageTweet()
        {
            var imageBytes= _imagesProvider.GetRandomImageAsByteArray();
            var uploadedMedia = await _twitterClient.Upload.UploadBinaryAsync(imageBytes);

            var tweetParams = new PublishTweetParameters
            {
                Medias = new List<IMedia> { uploadedMedia }
            };

            var tweet = await _twitterClient.Tweets.PublishTweetAsync(tweetParams);

        }
    }
}
