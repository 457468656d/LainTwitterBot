using LainTwitterBot.Interfaces;
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

        public ImagesProvider(string imageDirectory)
        {
            _imageDirectory = imageDirectory;
        }



        public byte[] GetRandomImageAsByteArray()
        {
            string[] files = Directory.GetFiles(_imageDirectory, "*.png");

            int maxRange= files.Length - 1;

            Random r = new Random();
            int rInt = r.Next(0, maxRange);

            byte[] imageByteArray = File.ReadAllBytes(files[rInt]);

            return imageByteArray;
        }
    }
}
