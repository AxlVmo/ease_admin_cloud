using System.Collections.Generic;

namespace ease_admin_cloud.Models
{
    public class FileUploadViewModel
    {
        public List<FileOnFileSystemModel>? FilesOnFileSystem { get; set; }
        public List<FileOnDatabaseModel>? FilesOnDatabase { get; set; }
    }
}