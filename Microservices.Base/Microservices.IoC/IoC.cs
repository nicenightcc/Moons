using Microservices.IoC;

namespace Microservices.Builder
{
    public static class IoC
    {
        public static ServiceBuilder LoadFile(this ServiceBuilder builder, string file)
        {
            IoCFac.Instance.LoadFile(file);
            return builder;
        }
        public static ServiceBuilder LoadAssembly(this ServiceBuilder builder, string assembly)
        {
            IoCFac.Instance.LoadAssembly(assembly);
            return builder;
        }
        public static ServiceBuilder LoadDir(this ServiceBuilder builder, string dir)
        {
            IoCFac.Instance.LoadDir(dir);
            return builder;
        }
    }
}
