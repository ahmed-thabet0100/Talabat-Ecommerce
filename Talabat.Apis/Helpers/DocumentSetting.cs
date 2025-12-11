namespace Talabat.APIs.Helpers
{
    public class DocumentSetting
    {
        public static string UploadFile(IFormFile File , string FolderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot//images", FolderName/*Products*/);

            var fileName = $"{Guid.NewGuid()}{File.FileName}";

            var filePath = Path.Combine(folderPath, fileName);

            //4. save file as stream [data per time]
            using var filestream = new FileStream(filePath, FileMode.Create);

            File.CopyTo(filestream);

            return $"/images/{FolderName}/{fileName}";

        }

        public static void DeleteFile(string FolderName , string FileName)
        {
            var filePath = Path.Combine( $"images", FolderName, FileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
