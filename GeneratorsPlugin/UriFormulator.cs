using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{
    public class UriFormulator : IFormulator<Uri>
    {
        StringFormulator stringGenerator = new();
        public Uri Generate()
        {
            var path = stringGenerator.Generate();
            UriBuilder uriBuilder = new UriBuilder("https", "example.com", 8080, $"/{path}", "extra");

            return uriBuilder.Uri;
        }
    }
}
