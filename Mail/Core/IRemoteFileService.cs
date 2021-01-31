using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mail.Api.Core
{
    public interface IRemoteFileService
    { 
        Task<Stream> GetRemoteFile(string url);
    }

    public class RemoteFileService : IRemoteFileService
    {
        private HttpClient _httpClient;
        public RemoteFileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Stream> GetRemoteFile(string url)
        {

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                HttpContent content = response.Content;
                return await content.ReadAsStreamAsync(); // get the actual content stream

            }
            throw new FileNotFoundException(response.ReasonPhrase);
        }
    }
}
