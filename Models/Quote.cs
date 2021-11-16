using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteGeneratorAPI.Models {

    public class Quote {

        [Key]
        private int _id;

        [Required]
        [MaxLength(100)]
        private string _author;

        [Required]
        [MaxLength(500)]
        private string _content;

        [Url]
        private string _permaLink;

        [Required]
        [MaxLength(100)]
        private string _image;

        public Quote() {
        }

        public Quote(int id, string author, string content, string permaLink, string image) {
            this._id = id;
            this._author = author;
            this._content = content;
            this._permaLink = permaLink;
            this._image = image;
        }

        public int Id {
            get { return _id; }
            set { _id = value; }
        }

        public string Author {
            get { return _author; }
            set { _author = value; }
        }
        public string Content {
            get { return _content; }
            set { _content = value; }
        }

        public string PermaLink {
            get { return _permaLink; }
            set { _permaLink = value; }
        }
        public string Image {
            get { return _image; }
            set { _image = value; }
        }
    }
}
