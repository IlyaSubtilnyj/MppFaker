using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{
    [Generator(typeof(Uri))]
    public class UriGenerator : IGenerator
    {
        StringGenerator stringGenerator = new();
        public object Generate()
        {
            var path = stringGenerator.Generate();
            UriBuilder uriBuilder = new UriBuilder("https", "example.com", 8080, $"/{path}", "extra");

            return uriBuilder.Uri;
        }
    }
}
