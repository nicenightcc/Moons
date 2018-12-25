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

        //path is file or directory or namespace
        [Obsolete("使用ServerBuilder.Load")]
        public IoCFac Load(string path)
        {
            if (File.Exists(path))
            {
                LoadFile(path);
            }
            else if (File.Exists(Path.Combine(Environment.CurrentDirectory, path)))
            {
                LoadFile(Path.Combine(Environment.CurrentDirectory, path));
            }
            else if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path).Where(en => en.EndsWith(".dll"));
                if (files.Count() == 0)
                    return this;

                foreach (var file in files)
                    LoadFile(file);
            }
            else
            {
                try
                {
                    Assembly assembly = Assembly.Load(new AssemblyName(path));
                    if (assembly != null)
                        Load(assembly);
                }
                catch (Exception e)
                {
                    Log.Logger.Error($"Load Assembly Error: [{path}]", e);
                }
            }
            return this;
        }

        public void Load(Assembly assembly)
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

        protected void LoadFile(string file)
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
