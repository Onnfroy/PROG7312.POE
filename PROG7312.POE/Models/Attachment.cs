using System;                                  // Guid

namespace PROG7312.POE.Models
{
    public sealed class Attachment
    {
        public Guid Id { get; }                // Unique id.
        public string FilePath { get; }        // Full path from file dialog.
        public string FileName { get; }        // For display.

        public Attachment(string filePath)
        {
            Id = Guid.NewGuid();               // Create id.
            FilePath = filePath;               // Save path.
            FileName = System.IO.Path.GetFileName(filePath); // Extract name.
        }
    }
}