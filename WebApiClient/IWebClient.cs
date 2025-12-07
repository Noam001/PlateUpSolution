using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    internal interface IWebClient<T>
    {
        //sycronous methods
        T Get();
        bool Post(T data);
        bool Post(T data, FileStream file);
        bool Post(T data, List<FileStream> files);

        //asyncronous
        Task<T> GetAsync();
        Task<bool> PostAsync(T data);
        Task<bool> PostAsync(T data, FileStream file);
        Task<bool> PostAsync(T data, List<FileStream> files);

    }
}
