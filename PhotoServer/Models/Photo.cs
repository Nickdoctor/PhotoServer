namespace PhotoServer.Models
{
    public class Photo  // This Model will be used to represent a photo object.
    {
        public int Id { get; set; }                   // Primary Key
        public string FileName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;  // Path or link to photo
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Foreign key relationship
        public int AlbumId { get; set; } // Connecting Photo to a Album
        public Album? Album { get; set; } // Navigation property Photo.Album is a reference navigation property
                                          // that allows you to access the Album object associated with a specific Photo.
    }
}
