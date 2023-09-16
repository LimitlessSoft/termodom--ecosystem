using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Http.Interfaces;
using TD.Core.Contracts.Requests;

namespace TD.Core.Contracts.Extensions
{
    public static class RequestExtensions
    {
        public static bool IdsNotMatch(this IdRequest request, int id, bool allowNullAsId = true)
        {
            var mockSaveRequest = new SaveRequest(request.Id);
            var notMatch = mockSaveRequest.IdsNotMatch(id, allowNullAsId);

            request.Id = mockSaveRequest.Id ?? -1;

            return notMatch;
        }
        public static bool IdsNotMatch(this SaveRequest request, int id, bool allowNullAsId = true)
        {
            if (request == null)
                return true;

            var notMatch = allowNullAsId == false && request.Id != id;

            if(request.Id == null)
                request.Id = id;

            return notMatch;
        }

        public static bool IdsNotMatch<TResponse>(this IdRequest request, int id, TResponse response, bool allowNullAsId = true)
            where TResponse : IResponse
        {
            var mockSaveRequest = new SaveRequest(request.Id);
            var notMatch = mockSaveRequest.IdsNotMatch(id, allowNullAsId);

            request.Id = mockSaveRequest.Id ?? -1;

            return notMatch;
        }

        public static bool IdsNotMatch<TResponse>(this SaveRequest request, int id, TResponse response, bool allowNullAsId = true)
            where TResponse : IResponse
        {
            var notMatch = request.IdsNotMatch(id, allowNullAsId);
            if(notMatch)
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                response.Errors = new List<string>()
                {
                    string.Format(RequestValidationCodes.RVC_001.GetDescription(""), nameof(request))
                };
                return true;
            }

            return notMatch;
        }
    }
}
