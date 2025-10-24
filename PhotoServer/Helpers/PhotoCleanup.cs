using Microsoft.EntityFrameworkCore;
using PhotoServer.Data;
namespace PhotoServer.Helpers
{
    public static class PhotoCleanup
    {
        public static void CleanupMissingFiles(AppDbContext context, IWebHostEnvironment env)
        {
            var uploadsFolder = Path.Combine(env.WebRootPath ?? "wwwroot", "uploads");

            // Get all filenames on disk
            var filesOnDisk = Directory.Exists(uploadsFolder)
                ? Directory.GetFiles(uploadsFolder)
                .Select(Path.GetFileName)
                .OfType<string>()   // keeps only non-null strings
                .ToHashSet()
                : new HashSet<string>();

            // Find DB entries with missing files
            var photosToRemove = context.Photos
                .Where(p => !filesOnDisk.Contains(p.FileName))
                .ToList();

            if (photosToRemove.Count == 0)
                return;

            try
            {
                context.Photos.RemoveRange(photosToRemove);
                context.SaveChanges();
                Console.WriteLine($"Removed {photosToRemove.Count} missing photo(s) from the database.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Some rows might have already been deleted
                Console.WriteLine($"Concurrency exception during cleanup: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during cleanup: {ex.Message}");
            }
        }
    }
}
