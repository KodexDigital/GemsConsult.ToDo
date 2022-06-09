namespace Todo.Core.Common
{
    public class ResponseModel
    {
        public bool Status { get; set; }
        public string? Message { get; set; }

        public static ResponseModel Success(string? message = null)
        {
            return new ResponseModel()
            {
                Status = true,
                Message = message ?? "Request was Successful",
            };
        }

        public static ResponseModel Failure(string? message = null)
        {
            return new ResponseModel()
            {
                Message = message ?? "Request was not completed",
            };

        }
        public static ResponseModel ValidationError(string? message = null)
        {
            return new ResponseModel()
            {
                Message = message ?? "One or more validation error occurred",
            };

        }

        public static ResponseModel Information(string? message = null)
        {
            return new ResponseModel
            {
                Message = message ?? "Request was completed.",
            };
        }
    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }

        public static ResponseModel<T> Success(T data, string? message = null)
        {
            return new ResponseModel<T>()
            {
                Status = true,
                Message = message ?? "Request was Successful",
                Data = data,
            };
        }
    }

    public class ResponseErrorModel : ResponseModel
    {
        public IDictionary<string, string[]>? Errors { get; set; }

        public static ResponseModel Failure(IDictionary<string, string[]>? errors = null, string? message = null)
        {
            return new ResponseErrorModel()
            {
                Message = message ?? "Request was not completed",
                Errors = errors ?? new Dictionary<string, string[]>(),
            };
        }
    }
}
