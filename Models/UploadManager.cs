using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace QuoteGeneratorAPI.Models {
    public class UploadManager {

        // Constant Variables
        public const int ERROR_NO_FILE = 0;
        public const int ERROR_FILE_EXISTS = 1;
        public const int ERROR_FILETYPE = 2;
        public const int ERROR_FILESIZE = 3;
        public const int ERROR_NAME_LENGTH = 4;
        public const int ERROR_SAVING = 5;
        public const int VALID = 6;
        public const int SUCCESS = 7;

        public const int UPLOAD_MAX_SIZE = 10485760; // 10MB
        public const int UPLOAD_MAX_NAME_LENGTH = 255;

        private string targetPath;
        private string rootPath;

        // Constructor
        public UploadManager(IWebHostEnvironment env, string targetPath) {
            this.targetPath = targetPath;
            this.rootPath = env.WebRootPath;

            // Create the target directory if it doesn't exist
            if (!Directory.Exists (this.rootPath + this.targetPath)) {
                Directory.CreateDirectory (this.rootPath + this.targetPath);
            }
        }


        // Validate incomming file
        private int validateFile(IFormFile file) {
            // Check if file is null
            if (file == null) {
                return ERROR_NO_FILE;
            }
            // Check if file exists already
            if (File.Exists (this.rootPath + this.targetPath + file.FileName)) {
                return ERROR_FILE_EXISTS;
            }
            // Check if file is of correct type
            if (!file.FileName.EndsWith (".png") && !file.FileName.EndsWith (".jpg") && !file.FileName.EndsWith (".jpeg")) {
                return ERROR_FILETYPE;
            }
            // Check if file is of correct size
            if (file.Length > UPLOAD_MAX_SIZE) {
                return ERROR_FILESIZE;
            }
            // Check if file name is too long
            if (file.FileName.Length > UPLOAD_MAX_NAME_LENGTH) {
                return ERROR_NAME_LENGTH;
            }
            // If all checks pass, return success
            return VALID;
        }

        // Upload a file to the server
        public int uploadFile(IFormFile fileToUpload) {
            // Validate the file
            int result = validateFile (fileToUpload);
            if (result != VALID) {
                return result;
            }
            // Create a FileStream to write to the file
            FileStream fs = new FileStream(this.rootPath + this.targetPath + fileToUpload.FileName, FileMode.Create);
            try {
                // Use the FileStream to write to the file
                fileToUpload.CopyTo(fs);
                return SUCCESS;
            } catch (Exception e) {
                Console.WriteLine(">>> Error when writing file");
                Console.WriteLine(e.Message);
                return ERROR_SAVING;
            } finally {
                // Close the FileStream
                fs.Close();
            }
        }
    }
}