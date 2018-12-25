namespace Microservices.Base
{
    public enum ProcessCode : ushort
    {
        Continue = 100,
        OK = 200,
        Created = 201,
        Accepted = 202,
        MovedPermanently = 301,
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        RequestTimeout = 408,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504
    }
}
