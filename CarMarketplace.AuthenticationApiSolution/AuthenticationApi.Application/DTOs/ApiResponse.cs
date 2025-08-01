using Microsoft.AspNetCore.Http;
namespace AuthenticationApi.Application.DTOs
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string Path { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Auto timestamp

        public ApiResponse(int statusCode, string message, T data, string path)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            Path = path;
            // Log the response (optional)
            Console.WriteLine($"[LOG] {Timestamp} - {StatusCode}: {Message} - {Path}");
        }
    }

    public class SuccessResponse<T> : ApiResponse<T>
    {
        public SuccessResponse(T data, string path, string message = "Request successful")
            : base(StatusCodes.Status200OK, message, data, path) { }
    }

    public class NotFoundResponse : ApiResponse<string>
    {
        public NotFoundResponse(string path, string message = "Resource not found")
            : base(StatusCodes.Status404NotFound, message, "", path) { }
    }

    public class BadRequestResponse : ApiResponse<string>
    {
        public BadRequestResponse(string path, string message = "Invalid request")
            : base(StatusCodes.Status400BadRequest, message, "", path) { }
    }
}
