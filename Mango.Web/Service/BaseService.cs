using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MangoAPI");
                var requestMessage = new HttpRequestMessage();
                requestMessage.Headers.Add("Accept", "application/json");

                requestMessage.RequestUri = new Uri(requestDto.Url);

                if (requestDto.Data != null)
                {
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                switch (requestDto.ApiType)
                {
                    case Utility.ApiType.GET:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                    case Utility.ApiType.POST:
                        requestMessage.Method = HttpMethod.Post;
                        break;
                    case Utility.ApiType.PUT:
                        requestMessage.Method = HttpMethod.Put;
                        break;
                    case Utility.ApiType.DELETE:
                        requestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        break;
                }

                var responseMessage = await client.SendAsync(requestMessage);

                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Not Found"
                        };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Access denied"
                        };
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Unauthorized"
                        };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Server error"
                        };
                    case System.Net.HttpStatusCode.BadRequest:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Server error"
                        };
                    default:
                        var apiContent = await responseMessage.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<ResponseDto?>(apiContent);
                }
            }
            catch (Exception e)
            {
                return new()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

    }
}
