namespace Microservices.Base
{
    public interface IResponse : IDefinition
    {
        ProcessCode Code { get; set; }
        string Message { get; set; }
    }
}
