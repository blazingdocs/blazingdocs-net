using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazingDocs.Models
{
    public class OperationModel
    {
        public Guid Id { get; set; }
        public OperationTypeModel Type { get; set; }
        public int PageCount { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public string RemoteIpAddress { get; set; }

        public List<FileModel> Files { get; set; }

        public async Task SaveToAsync(Stream stream)
        {
            var client = new HttpClient();

            using (var source = await client.GetStreamAsync(Files[0].DownloadUrl))
            {
                await source.CopyToAsync(stream);
            }
        }
    }
}
