using ReportApp.Common.views;

namespace ReportApp.Common.views
{
    public static class MediatorResultExtensions
    {
        public static EndpointResponse<TResponse> ToEndpointResponse<TResponse, TData>(
            this RequestResult<TData> result,
            Func<TData, TResponse> onSuccess)
        {
            if (!result.isSuccess)
            {
                return EndpointResponse<TResponse>.Failure(result.errorCode, result.message);
            }

            return EndpointResponse<TResponse>.Success(onSuccess(result.data));
        }
    }

}
