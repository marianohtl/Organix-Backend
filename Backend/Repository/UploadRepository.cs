    using System.IO;
    using System.Net.Http.Headers;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    namespace backend.Repositories
    {
        public class UploadRepository : ControllerBase
        {
            public string Upload (IFormFile arquivo, string savingFolder) {
                
               if(savingFolder == null) {
                    savingFolder = Path.Combine ("Images");                
                }

                // var pathToSave = Path.Combine (Directory.GetCurrentDirectory (), savingFolder);

                if (arquivo.Length > 0) {
                    var fileName = ContentDispositionHeaderValue.Parse (arquivo.ContentDisposition).FileName.Trim ('"');
                    var RelativePath = Path.Combine (savingFolder, fileName);

                    using (var stream = new FileStream (RelativePath, FileMode.Create)) {
                        arquivo.CopyTo (stream);
                    }                    
            
                    return RelativePath;
                } else {
                    return null;
                }           
            }
        }
    }