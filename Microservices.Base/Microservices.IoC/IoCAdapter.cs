using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Microservices.Common;
using System;
using System.Collections.Generic;

namespace Microservices.IoC
{
    public abstract class IoCAdapter : IDisposable
    {
        protected ContainerBuilder builder = new ContainerBuilder();
        protected IContainer container = null;

        protected virtual void Build()
        {
            this.container = this.builder.Build();
        }

        protected virtual IoCAdapter Register<T>(List<DIParameter> parameters = null) where T : class
        {
            this.Register(typeof(T), parameters);
            return this;
        }

        protected virtual IoCAdapter Register<T, TBase>(List<DIParameter> parameters = null) where T : class, TBase
        {
            this.Register(typeof(T), typeof(TBase), parameters);
            return this;
        }

        protected virtual IoCAdapter Register(Type type, List<DIParameter> parameters = null)
        {
            this.Register(type, type.BaseType, parameters);
            return this;
        }

        protected virtual IoCAdapter Register(Type type, Type baseType, List<DIParameter> parameters = null)
        {
            if (baseType.IsAssignableFrom(type))
                try
                {
                    var builder = SetLifeTime(this.builder.RegisterType(type).As(baseType));

                    if (parameters != null && parameters.Count > 0)
                        foreach (var param in parameters)
                        {
                            builder.WithParameter(
                            new ResolvedParameter(
                               (p, c) => param.Predicate(p),
                               (p, c) => param.Value
                           ));
                        }
                }
                catch (Exception e)
                {
                    Log.Logger.Error($"IoC Adapter Error: [{type.FullName}]", e);
                }
            else
                Log.Logger.Error($"IoC Adapter Error: [{type.FullName}] {type.Name} is not assignable to {baseType.Name}");
            return this;
        }

        protected virtual T Resolve<T>()
        {
            return this.container.Resolve<T>();
        }

        protected virtual object Resolve(Type type)
        {
            return this.container.Resolve(type);
        }

        protected virtual T Resolve<T>(Type type) where T : class
        {
            return this.container.Resolve(type) as T;
        }

        protected delegate IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> SetLifeTimeDelegate(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder);
        protected static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> SetPerLifeTimeScope(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder) { return builder.InstancePerLifetimeScope(); }
        protected static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> SetSingle(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder) { return builder.SingleInstance(); }
        protected static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> SetPerDependency(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder) { return builder.InstancePerDependency(); }
        protected SetLifeTimeDelegate SetLifeTime = new SetLifeTimeDelegate(SetPerLifeTimeScope);
        public virtual IoCAdapter UsePerLifeTimeScope() { SetLifeTime = new SetLifeTimeDelegate(SetPerLifeTimeScope); return this; }
        public virtual IoCAdapter UseSingle() { SetLifeTime = new SetLifeTimeDelegate(SetSingle); return this; }
        public virtual IoCAdapter UsePerDependency() { SetLifeTime = new SetLifeTimeDelegate(SetPerDependency); return this; }

        public void Dispose()
        {
            this.container.Dispose();
            this.builder = null;
            this.container = null;
        }
    }
}
