using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazingDocs.Models
{
    public class FileModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string DownloadUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public long Length { get; set; }

        public async Task SaveToAsync(Stream stream)
        {
            var client = new HttpClient();

            using (var source = await client.GetStreamAsync(DownloadUrl))
            {
                await source.CopyToAsync(stream);
            }
        }
    }
}
