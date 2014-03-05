using System;
using OpenTextSummarizer.Interfaces;

namespace OpenTextSummarizer
{
    public class FileContentProvider : IContentProvider
    {
        public string FilePath { get; set; }

        public string Content { get; private set; }

        public FileContentProvider(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }

            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException(string.Empty, filePath);
            }

            FilePath = FilePath;
            Content = System.IO.File.ReadAllText(filePath);
        }
    }
}