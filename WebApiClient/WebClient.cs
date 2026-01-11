using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiClient
{
    public class WebClient<T>: IWebClient<T>
    {
        HttpClient httpClient;
        UriBuilder uriBuilder; //מרכיב את כתובת הבקשה\ הקישור

        public WebClient()
        {
            this.httpClient = new HttpClient();
            this.uriBuilder = new UriBuilder();
            this.uriBuilder.Query = string.Empty;   
        }
        public string Schema 
        {  
            set
            {
                this.uriBuilder.Scheme = value;
            }
        }
        public string Host
        {
            set
            {
                this.uriBuilder.Host = value;
            }
        }
        public int Port
        {
            set
            {
                this.uriBuilder.Port = value;
            }
        }
        public string Path
        {
            set
            {
                this.uriBuilder.Path = value;
            }
        }
        public void AddParameter(string key, string value)
        {

            if(this.uriBuilder.Query == string.Empty)
            {
                this.uriBuilder.Query +="?"+ key + "=" + value;
            }
        
            else
            {
                this.uriBuilder.Query += "&" + key + "=" + value;
            }
        }
        public T Get()
        {
            using(HttpRequestMessage requestMessage = new HttpRequestMessage()) //מילה יוסינג מוודא שהאובייקט שנוצר בתוכו ימחק מהזכרון בסיום הפעולה
            {
                requestMessage.Method = HttpMethod.Get; //שיטת שליחת בקשה GET
                requestMessage.RequestUri = this.uriBuilder.Uri; //מגדיר את כתובת הבקשה
                using(HttpResponseMessage responseMessage = this.httpClient.SendAsync(requestMessage).Result) //באמצעות HTTPCLIENT שולחים את בקשה לשרת התשובה נשמרת בתוך האובייקט RESPONSEMESSAGE
                {
                    if (responseMessage.IsSuccessStatusCode == true) //האם הבקשה הצליחה(קיבלה קוד 200)
                    {
                        string result = responseMessage.Content.ReadAsStringAsync().Result;
                        JsonSerializerOptions options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        T data = JsonSerializer.Deserialize<T>(result,options); //העברת פורמט מגייסון לאובייקט הספציפי
                        return data;
                    }
                    else
                        return default(T); //בהתאם לאובייקט של T מוחזר ערך ברירת מחדל
                }
            }
        }

        public async Task<T> GetAsync()
        {
            using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, this.uriBuilder.Uri)) //מילה יוסינג מוודא שהאובייקט שנוצר בתוכו ימחק מהזכרון בסיום הפעולה
            {
                requestMessage.Method = HttpMethod.Get; //שיטת שליחת בקשה GET
                requestMessage.RequestUri = this.uriBuilder.Uri; //מגדיר את כתובת הבקשה
                using (HttpResponseMessage responseMessage = this.httpClient.SendAsync(requestMessage).Result) //באמצעות HTTPCLIENT שולחים את בקשה לשרת התשובה נשמרת בתוך האובייקט RESPONSEMESSAGE
                {
                    if (responseMessage.IsSuccessStatusCode == true) //האם הבקשה הצליחה (קיבלה קוד 200
                    {
                        string result = await responseMessage.Content.ReadAsStringAsync();
                        T data = JsonSerializer.Deserialize<T>(result); //העברת פורמט מגייסון לאובייקט הספציפי
                        return data;
                    }
                    else
                    {
                        return default(T); //בהתאם לאובייקט של T מוחזר ערך ברירת מחדל
                    }
                }
            }
        }

        public bool Post(T data)
        {
            using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, this.uriBuilder.Uri))
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.RequestUri = this.uriBuilder.Uri;
                string jsondata = JsonSerializer.Serialize(data); //מעביר את הפורמט של האובייקט לפורמט גייסון
                requestMessage.Content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                using (HttpResponseMessage responseMessage = this.httpClient.SendAsync(requestMessage).Result)
                {
                    return responseMessage.IsSuccessStatusCode;
                }
            }
        }

        public bool Post(T data, Stream file)
        {
            using (HttpRequestMessage requestMessage = new HttpRequestMessage())
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.RequestUri = this.uriBuilder.Uri;
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                string jsondata = JsonSerializer.Serialize(data);
                StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
                multipartFormDataContent.Add(stringContent, "data");
                StreamContent fileContent = new StreamContent(file);
                multipartFormDataContent.Add(fileContent, "file", "fileName");
                requestMessage.Content = multipartFormDataContent;
                using (HttpResponseMessage responseMessage = this.httpClient.SendAsync(requestMessage).Result)
                {
                    return responseMessage.IsSuccessStatusCode;
                }
            }
        }

        public bool Post(T data, List<Stream> files)
        {
            using (HttpRequestMessage requestMessage = new HttpRequestMessage())
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.RequestUri = this.uriBuilder.Uri;
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                string jsondata = JsonSerializer.Serialize(data);
                StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
                multipartFormDataContent.Add(stringContent, "data");
                foreach (var file in files)
                {
                    StreamContent fileContent = new StreamContent(file);
                    multipartFormDataContent.Add(fileContent, "file", "fileName");
                }
                requestMessage.Content = multipartFormDataContent;
                using (HttpResponseMessage responseMessage = this.httpClient.SendAsync(requestMessage).Result)
                {
                    return responseMessage.IsSuccessStatusCode;
                }
            }
        }

        public async Task<bool> PostAsync(T data)
        {
            using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, this.uriBuilder.Uri))
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.RequestUri = this.uriBuilder.Uri;
                string jsondata = JsonSerializer.Serialize(data); //מעביר את הפורמט של האובייקט לפורמט גייסון
                requestMessage.Content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                using (HttpResponseMessage responseMessage = await this.httpClient.SendAsync(requestMessage))
                {
                    return responseMessage.IsSuccessStatusCode;
                }
            }
        }

        public async Task<bool> PostAsync(T data, Stream file)
        {
            using (HttpRequestMessage requestMessage = new HttpRequestMessage())
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.RequestUri = this.uriBuilder.Uri;
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                string jsondata = JsonSerializer.Serialize(data);
                StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
                multipartFormDataContent.Add(stringContent, "data");
                StreamContent fileContent = new StreamContent(file);
                multipartFormDataContent.Add(fileContent, "file", "fileName");
                requestMessage.Content = multipartFormDataContent;
                using (HttpResponseMessage responseMessage = await this.httpClient.SendAsync(requestMessage))
                {
                    return responseMessage.IsSuccessStatusCode;
                }
            }
        }

        public async Task<bool> PostAsync(T data, List<Stream> files)
        {
            using (HttpRequestMessage requestMessage = new HttpRequestMessage())
            {
                requestMessage.Method = HttpMethod.Post;
                requestMessage.RequestUri = this.uriBuilder.Uri;
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                string jsondata = JsonSerializer.Serialize(data);
                StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
                multipartFormDataContent.Add(stringContent, "data");
                foreach (var file in files)
                {
                    StreamContent fileContent = new StreamContent(file);
                    multipartFormDataContent.Add(fileContent, "file", "fileName");
                }
                requestMessage.Content = multipartFormDataContent;
                using (HttpResponseMessage responseMessage = await this.httpClient.SendAsync(requestMessage))
                {
                    return responseMessage.IsSuccessStatusCode;
                }
            }
        }
    }
}
