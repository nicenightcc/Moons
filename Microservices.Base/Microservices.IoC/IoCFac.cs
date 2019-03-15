using Microservices.Base;
using Microservices.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microservices.IoC
{
    public class IoCFac
    {
        public static readonly IoCFac Instance = new IoCFac();
        protected List<Type> types = new List<Type>();
        protected IoCFac() { }

        [Obsolete("使用ServerBuilder.LoadFile")]
        public IoCFac LoadFile(string file)
        {
            if (File.Exists(file))
            {
                Log.Logger.Info("Loading Assembly from: " + file);
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    if (assembly != null)
                        Load(assembly);
                }
                catch (Exception e)
                {
                    Log.Logger.Error($"Load Assembly Error: [{Path.GetFileName(file)}]", e);
                }
            }
            return this;
        }

        [Obsolete("使用ServerBuilder.LoadAssembly")]
        public IoCFac LoadAssembly(string assembly)
        {
            try
            {
                Assembly ass = Assembly.Load(new AssemblyName(assembly));
                if (ass != null)
                    Load(ass);
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Load Assembly Error: [{assembly}]", e);
            }
            return this;
        }

        [Obsolete("使用ServerBuilder.LoadDir")]
        public IoCFac LoadDir(string path)
        {
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path).Where(en => en.EndsWith(".dll"));
                if (files.Count() == 0)
                    return this;

                foreach (var file in files)
                    LoadFile(file);
            }
            return this;
        }

        private void Load(Assembly assembly)
        {
            try
            {
                var types = assembly.GetTypes().Where(en => en.IsAssignableTo<IDefinition>());
                if (types.Count() == 0) return;
                var name = assembly.GetName().Name;
                Log.Logger.Info($"Loaded Assembly: [{name}]");

                this.types.AddRange(types);
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Load Assembly Error: [{assembly.FullName}]", e);
            }
        }

        public Type Get(string fullname)
        {
            return types.FirstOrDefault(en => en.FullName == fullname);
        }

        public Type Get<T>()
        {
            return types.FirstOrDefault(en => en.IsAssignableTo<T>());
        }

        public Type Get(Type type)
        {
            return types.FirstOrDefault(en => en.IsAssignableTo(type));
        }

        public IList<Type> GetList(Type type)
        {
            return types.Where(en => en.IsAssignableTo(type))?.ToList();
        }

        public IList<Type> GetList<T>()
        {
            return types.Where(en => en.IsAssignableTo<T>())?.ToList();
        }

        public IList<Type> GetList(string @namespace)
        {
            return types.Where(en => en.Namespace == @namespace)?.ToList();
        }

        public IList<Type> GetAll()
        {
            return types;
        }

        public Type GetClass(string fullname)
        {
            return types.FirstOrDefault(en => en.FullName == fullname && en.IsClass);
        }

        public Type GetClass<T>()
        {
            return types.FirstOrDefault(en => en.IsAssignableTo<T>() && en.IsClass);
        }

        public Type GetClass(Type type)
        {
            return types.FirstOrDefault(en => en.IsAssignableTo(type) && en.IsClass);
        }

        public IList<Type> GetClassList(Type type)
        {
            return types.Where(en => en.IsAssignableTo(type) && en.IsClass)?.ToList();
        }

        public IList<Type> GetClassList<T>()
        {
            return types.Where(en => en.IsAssignableTo<T>() && en.IsClass)?.ToList();
        }

        public IList<Type> GetClassList(string @namespace)
        {
            return types.Where(en => en.Namespace == @namespace && en.IsClass)?.ToList();
        }

        public IList<Type> GetAllClass()
        {
            return types.Where(en => en.IsClass)?.ToList();
        }
    }
}
