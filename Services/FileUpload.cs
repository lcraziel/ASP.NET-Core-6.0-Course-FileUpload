using Microsoft.AspNetCore.Components.Forms;

namespace FileUpload.Services
{
	public interface IFileUpload
	{
		Task UploadFile(IBrowserFile file);

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
					var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, 
						"Uploads", file.Name);

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
	}
}
