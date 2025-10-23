namespace PhotoServer.Models
{
    public class Album // This Model will be used to represent a photo album object, the idea is that I will be able to 
                       // select which album I will be adding my photos into.
    {
        public int Id { get; set; }                   // Primary Key
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property — one album can have many photos
        public List<Photo> Photos { get; set; } = new(); // List of photo objects into one album "Album.Photo is a collection
                                                         // navigation property that allows you to access all Photo objects
                                                         // in a specific Album."
    }
}

