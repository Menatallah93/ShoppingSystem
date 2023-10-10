namespace ShoppingSystem.Models
{
    public class ImagesHelper
    {
        public static string UploadImg(IFormFile file, string FolderName)
        {
            string FolderPathe = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName);

            string FileName = $"{Guid.NewGuid()}{Path.GetFileName(file.FileName)}";

            string FilePath = Path.Combine(FolderPathe, file.FileName);


            using FileStream FS = new FileStream(FilePath, FileMode.Create);

            file.CopyTo(FS);

            return file.FileName;

        }
    }
}
