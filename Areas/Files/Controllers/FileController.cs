using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ease_admin_cloud.Data;
using ease_admin_cloud.Models;
using Microsoft.AspNetCore.Identity;
using ease_admin_cloud.Areas.Address.Models;
using System.Security.Claims;

namespace FileUpload.MVC.Controllers
{
    [Area("Files")]
    public class FileController : Controller
    {
        private readonly eacDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotyfService _toastNotification;
        public FileController(eacDbContext context,
            UserManager<IdentityUser> userManager,
            INotyfService toastNotification)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "//UploadFiles//");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileModel = new FileOnFileSystemModel
                    {
                        CreatedOn = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        Description = description,
                        FilePath = filePath,
                        UploadedBy = userId

                    };
                    _context.FilesOnFileSystem.Add(fileModel);
                    _context.SaveChanges();
                    _toastNotification.Information("Archivo subido con éxito al sistema de archivos", 5);
                }
                else
                {
                    _toastNotification.Warning("Archivo ya existe, favor de revisar", 5);
                }

            }
          
            //TempData["Message"] = "File successfully uploaded to File System.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UploadToCPs(int id)
        {
            TempData["Message"] = "Favor de esperar ya que se insertaran al rededor de 150,000 registros";
            var BDfilePath = await _context.FilesOnFileSystem
              .FirstOrDefaultAsync(m => m.Id == id);

            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
            bool basePathExists = System.IO.Directory.Exists(basePath);
            var fileName = BDfilePath.Name;
            var filePath = BDfilePath.FilePath;
            var extension = BDfilePath.Extension;

            if (extension == ".txt")
            {
                bool HRData = false;
                bool HRsplit = false;
                string[] columns = null;
                int Idcontrol = 0;
                using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Latin1))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!HRData)
                        {
                            if (line == "El Catálogo Nacional de Códigos Postales, es elaborado por Correos de México y se proporciona en forma gratuita para uso particular, no estando permitida su comercialización, total o parcial, ni su distribución a terceros bajo ningún concepto.")
                            {
                                HRData = true;
                                continue;
                            }
                        }
                        else if (!HRsplit)
                        {
                            if (line == "d_codigo|d_asenta|d_tipo_asenta|D_mnpio|d_estado|d_ciudad|d_CP|c_estado|c_oficina|c_CP|c_tipo_asenta|c_mnpio|id_asenta_cpcons|d_zona|c_cve_ciudad")
                            {
                                HRsplit = true;
                                continue;
                            }
                        }
                        else
                        {
                            columns = line.Split("|");
                            HRsplit = true;

                            cat_codigo_postal CPs;
                            CPs = _context.cat_codigos_postales.Where(s => s.d_codigo == columns[0].ToString()).FirstOrDefault();

                            if (CPs == null)
                            {
                                CPs = new cat_codigo_postal();
                            }

                            Idcontrol = Idcontrol + 1;



                            CPs.id_codigo_postal = Idcontrol;
                            CPs.d_codigo = columns[0].ToString();
                            CPs.d_asenta = columns[1].ToString();
                            CPs.d_tipoAsenta = columns[2].ToString();

                            _context.cat_codigos_postales.Add(CPs);
                            //_context.SaveChanges();
                          



                        }
                    }
                    _toastNotification.Information("Archivo subido con éxito al sistema", 5);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var fileModel = new FileOnDatabaseModel
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description,
                    UploadedBy = userId 
                    
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                _context.FilesOnDatabase.Add(fileModel);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnDatabase = await _context.FilesOnDatabase.ToListAsync();
            viewModel.FilesOnFileSystem = await _context.FilesOnFileSystem.ToListAsync();
            return viewModel;
        }

        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {
            var file = await _context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {
            var file = await _context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            var memory = new MemoryStream();
            //using (var stream = new FileStream(file.FilePath, FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);
            //}
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }

        public async Task<IActionResult> DeleteFileFromFileSystem(int id)
        {
            var file = await _context.FilesOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }
            _context.FilesOnFileSystem.Remove(file);
            _context.SaveChanges();
            _toastNotification.Warning($"{file.Name + file.Extension} eliminado con éxito desde el sistema de archivos", 5);
            //TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {
            var file = await _context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.FilesOnDatabase.Remove(file);
            _context.SaveChanges();
            _toastNotification.Warning($"{file.Name + file.Extension} eliminado con éxito desde el sistema de archivos", 5);
            //TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }
    }
}