using Microsoft.AspNetCore.Components.Forms;

namespace FileUpload.Services
{
	public interface IFileUpload
	{
		Task UploadFile(IBrowserFile file);
		Task<string> GeneratePreviewUrl(IBrowserFile file);
	}
	public class File_Upload : IFileUpload
	{
		private IWebHostEnvironment _webHostEnvironment;
		private readonly ILogger<File_Upload> _logger;

		public File_Upload(IWebHostEnvironment webHostEnvironment, ILogger<File_Upload> 
			logger)
		{
			_webHostEnvironment = webHostEnvironment;
			_logger = logger;
		}

		public async Task UploadFile(IBrowserFile file)
		{
		   if(file is not null)
			{
				try
				{
					var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath,"Uploads", file.Name);
					using (var stream = file.OpenReadStream())
					{
						var fileStream = File.Create(uploadPath);
						await fileStream.CopyToAsync(fileStream);
						fileStream.Close();
					}

				}catch(Exception ex)
				{
					_logger.LogError(ex.ToString());
				}
			}
		}

		public async Task<string> GeneratePreviewUrl(IBrowserFile file)
		{
			if (!file.ContentType.Contains("image"))
			{
				if (file.ContentType.Contains("pdf"))
				{
					return "Images/pdf_logo.png";

				}

			}
			var resizedImage = await file.RequestImageFileAsync(file.ContentType,100, 100);
			var buffer = new byte[resizedImage.Size];
			await resizedImage.OpenReadStream().ReadAsync(buffer);
			return $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";

		}
	}
}
