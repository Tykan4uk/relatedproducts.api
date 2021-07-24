using System;

namespace RelatedProductsApi.Common.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}
