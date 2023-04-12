using LainTwitterBot.Comparer;
using LainTwitterBot.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LainTwitterBot.Services
{
    public class ImagesProvider : IImagesProvider
    {
        private readonly string _imageDirectory;
        private readonly ILogger<IImagesProvider> _logger;
        private static int _currentImageIndex;
        private int _maxImageIndex;
        public ImagesProvider(string imageDirectory, ILogger<IImagesProvider> logger)
        {
            _imageDirectory = imageDirectory;
            _logger = logger;
        }

        public byte[] GetNextImageAsByteArray()
        {
            string[] files = Directory.GetFiles(_imageDirectory, "*.png");

            // Sorting the Array like it is shown in the Windows Explorer.
            Array.Sort(files, new FileComparer());

            string currentFile = files[_currentImageIndex];
            _maxImageIndex = files.Length - 1;

            _logger.Log(LogLevel.Information, $"Current file ({_currentImageIndex} out of {_maxImageIndex}): {currentFile}");

            byte[] imageByteArray = File.ReadAllBytes(currentFile);

            if(_currentImageIndex == _maxImageIndex)
            {
                _currentImageIndex = 0;
                return imageByteArray;
            }
            
            _currentImageIndex++;

            return imageByteArray;
        }
    }
}
