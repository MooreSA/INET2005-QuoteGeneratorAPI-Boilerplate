using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteGeneratorAPI.Models {

    public class Quote {

        public int id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string author { get; set; }

        [Required]
        [StringLength(500)]
        public string content { get; set; }
        public string permaLink { get; set; }
        
        public string image { get; set; }


        public Quote() {
        }

        public Quote(int quoteId, string author, string content, string permaLink, string image) {
            this.id = quoteId;
            this.author = author;
            this.content = content;
            this.permaLink = permaLink;
            this.image = image;
        }

        public bool isValid() {
            if (author.Length > 100 || author == null) {
                Console.WriteLine("Author is invalid");
                return false;
            } else if (content.Length > 500 || content == null) {
                Console.WriteLine("Content is invalid");
                return false;
            } else if (permaLink != null) {
                if (permaLink.Length > 500 || !Uri.IsWellFormedUriString(permaLink, UriKind.Absolute)) {
                    Console.WriteLine("PermaLink is invalid");
                    return false;
                }
            } else if (image == null) {
                return false;
            }
            return true;
        }
    }
}
