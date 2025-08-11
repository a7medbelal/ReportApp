using Linkoo.Common.Enum;

namespace Linkoo.Common.views
{
    public record RequestResult<T>(T data, bool isSuccess, string message, ErrorCode errorCode)
    {

        public static RequestResult<T> Success(T data, string message = "")
        {
            return new RequestResult<T>(data, true, message, ErrorCode.none);
        }


        public static RequestResult<T> Failure(ErrorCode errorCode, string message = "")
        {
            return new RequestResult<T>(default, false, message, errorCode);
        }
    }
}