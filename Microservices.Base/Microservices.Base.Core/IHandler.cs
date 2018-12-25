namespace Microservices.Base
{
    public interface IHandler : IDefinition
    {
        IResponse Execute(IRequest request);
    }
}
