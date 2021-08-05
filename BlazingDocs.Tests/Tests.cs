using BlazingDocs.Enums;
using BlazingDocs.Parameters;
using BlazingDocs.Utils;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlazingDocs.Tests
{
    public class Tests
    {
        private BlazingClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new BlazingClient("api-key");
        }

        [Test]
        public async Task GetAccount()
        {
            Assert.IsNotNull(await _client.GetAccountAsync());
        }

        [Test]
        public async Task GetTemplates()
        {
            Assert.IsNotNull(await _client.GetTemplatesAsync());
        }

        [Test]
        public async Task GetUsage()
        {
            Assert.IsNotNull(await _client.GetUsageAsync());
        }

        [Test]
        public async Task Merge()
        {
            using (var source = File.OpenRead("PO-Template.docx"))
            {
                using (var reader = new StreamReader("PO-Template.json", Encoding.UTF8))
                {
                    var data = reader.ReadToEnd();

                    var parameters = new MergeParameters
                    {
                        Sequence = false, // data is object
                        DataSourceType = DataSourceType.Json, // data in json format
                        Strict = true // keep json types
                    };

                    var template = new FormFile("PO-Template.docx", source);

                    var result = await _client.MergeAsync(data, "output.pdf", parameters, template);

                    foreach (var file in result.Files)
                    {
                        using (var stream = new FileStream(file.Name, FileMode.Create))
                        {
                            file.SaveToAsync(stream).Wait();
                        }
                    }

                    Assert.NotNull(result);
                }
            }
        }
    }
}