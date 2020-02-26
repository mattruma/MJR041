using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace FunctionApp1.Tests.Helpers
{
    public static class HttpRequestHelper
    {
        public static HttpRequest CreateHttpRequest(
            string method,
            string uriString,
            IHeaderDictionary headers = null,
            object body = null)
        {
            var uri = new Uri(uriString);
            var request = new DefaultHttpContext().Request;
            var requestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();
            requestFeature.Method = method;
            requestFeature.Scheme = uri.Scheme;
            requestFeature.Path = uri.GetComponents(UriComponents.KeepDelimiter | UriComponents.Path, UriFormat.Unescaped);
            requestFeature.PathBase = string.Empty;
            requestFeature.QueryString = uri.GetComponents(UriComponents.KeepDelimiter | UriComponents.Query, UriFormat.Unescaped);

            headers ??= new HeaderDictionary();

            if (!string.IsNullOrEmpty(uri.Host))
            {
                headers.Add("Host", uri.Host);
            }

            if (body != null)
            {
                byte[] bytes = null;
                if (body is string bodyString)
                {
                    bytes = Encoding.UTF8.GetBytes(bodyString);
                }
                else if (body is byte[] bodyBytes)
                {
                    bytes = bodyBytes;
                }
                else if (body is IEnumerable bodyArray)
                {
                    bytes = Encoding.UTF8.GetBytes(JArray.FromObject(bodyArray).ToString(Newtonsoft.Json.Formatting.None));
                }
                else if (body is object bodyObject)
                {
                    bytes = Encoding.UTF8.GetBytes(JObject.FromObject(bodyObject).ToString(Newtonsoft.Json.Formatting.None));
                }


                requestFeature.Body = new MemoryStream(bytes);
                request.ContentLength = request.Body.Length;
                headers.Add("Content-Length", request.Body.Length.ToString());
            }

            requestFeature.Headers = headers;

            return request;
        }
    }
}
