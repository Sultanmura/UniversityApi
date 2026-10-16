using System.Text.Json.Serialization;

namespace UniversityApi.Common
{
    public class ReturnResult<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T? Result { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string TraceId { get; set; } = Guid.NewGuid().ToString();

        // Быстрый хелпер для формирования успешного ответа
        public static ReturnResult<T> Success(T result, int statusCode = 200) => new()
        {
            IsSuccess = true,
            StatusCode = statusCode,
            Result = result
        };

        // Быстрый хелпер для формирования ответа с ошибкой
        public static ReturnResult<T> Failure(string errorCode, string errorMessage, int statusCode = 400) => new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage
        };
    }
}