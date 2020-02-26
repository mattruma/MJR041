using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace FunctionApp1.Helpers
{
    internal static class StreamExtensions
    {
        internal static async Task<T> DeserializeAsync<T>(
           this Stream stream)
        {
            var sr =
                new StreamReader(stream);
            var json =
                await sr.ReadToEndAsync();
            var data =
                JsonConvert.DeserializeObject<T>(json);
            return data;
        }
    }
}
