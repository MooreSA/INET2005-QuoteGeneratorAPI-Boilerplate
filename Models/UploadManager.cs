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

        // Set the max upload size to 4mb
        public const int MAX_UPLOAD_SIZE = 4194304;
        public const int UPLOAD_MAX_NAME_LENGTH = 100;

        private string targetPath;
        private string rootPath;

        private string _destination;
        private string _fileName;
        private string _fileType;

        // Constructor
        public UploadManager(IWebHostEnvironment env, string targetPath) {
            this.targetPath = targetPath;
            this.rootPath = env.WebRootPath;

            // Create the target directory if it doesn't exist
            if (!Directory.Exists (this.rootPath + "/" + this.targetPath)) {
                Directory.CreateDirectory (this.rootPath + "/" + this.targetPath);
            }
        }

        public string Destination {
            get {
                return _destination;
            }
        }

        public string getFileName() {
            return _fileName;
        }


        // Validate incomming file
        private int validateFile(IFormFile file) {
            // Check if file is null
            if (file == null) {
                Console.WriteLine ("File is null");
                return ERROR_NO_FILE;
            }
            // Check if file is of correct type
            if (!file.FileName.EndsWith (".png") 
            && !file.FileName.EndsWith (".jpg") 
            && !file.FileName.EndsWith (".jpeg") 
            && !file.FileName.EndsWith (".gif")) {
                return ERROR_FILETYPE;
            }
            // Check if file is of correct size
            if (file.Length > MAX_UPLOAD_SIZE) {
                return ERROR_FILESIZE;
            }
            // If all checks pass, return success
            return VALID;
        }

        private void setFileType(IFormFile file) {
            if (file.FileName.EndsWith (".png")) {
                _fileType = "png";
            } else if (file.FileName.EndsWith (".jpg") || file.FileName.EndsWith (".jpeg")) {
                _fileType = "jpg";
            } else if (file.FileName.EndsWith (".gif")) {
                _fileType = "gif";
            }
        }

        private void generatefileName() {
            _fileName = Guid.NewGuid ().ToString() + "." + _fileType;
        }


        // Upload a file to the server
        public int uploadFile(IFormFile fileToUpload) {
            // Validate the file
            int result = validateFile(fileToUpload);

            if (result != VALID) {
                return result;
            }

            setFileType(fileToUpload); // set the file type
            generatefileName(); // generate the file name

            // set the destination
            _destination = this.rootPath + "/" + this.targetPath + "/" + _fileName;

            // Create a FileStream to write to the file
            FileStream fs = new FileStream(_destination, FileMode.Create);
            try {
                fileToUpload.CopyTo(fs); // Use the FileStream to write to the file
                return SUCCESS;
            } catch (Exception e) {
                Console.WriteLine(">>> Error when writing file");
                Console.WriteLine(e.Message);
                return ERROR_SAVING;
            } finally {
                fs.Close(); // Close the FileStream
            }
        }

        public int deleteFile (string fileName) {
            Console.WriteLine("Deleting file: " + fileName);
            if (fileName == null) {
                return ERROR_NO_FILE;
            }
            string filePath = this.rootPath + "/" + this.targetPath + "/" + fileName;
            if (!File.Exists (filePath)) {
                return ERROR_NO_FILE;
            }
            try {
                File.Delete(filePath);
            } catch (Exception e) {
                Console.WriteLine("Error Deleting file: " + fileName);
                Console.WriteLine(">>> MESSAGE:" + e.Message);
                return ERROR_SAVING;
            }
            return SUCCESS;
        }
    }
}