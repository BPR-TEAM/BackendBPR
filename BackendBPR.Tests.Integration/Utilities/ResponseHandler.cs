﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace BackendBPR.Tests.Integration.Utilities
{
    public static class ResponseHandler<T>
    {
        public static T GetObject(HttpResponseMessage response)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            var returnObject = JsonConvert.DeserializeObject<T>(responseString);

            return returnObject;
        }

        public static HttpContent SerializeObject(T response)
        {
            var returnObject = JsonConvert.SerializeObject(response);
            var buffer = Encoding.UTF8.GetBytes(returnObject);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }
    }
}