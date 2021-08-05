using System.IO;

namespace BlazingDocs.Utils
{
    public class FormFile
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public Stream Content { get; set; }

        public FormFile() { }

        public FormFile(string name, Stream content) : this(name)
        {
            Content = content;
        }

        public FormFile(string name) : this()
        {
            Name = name;
        }
    }
}
