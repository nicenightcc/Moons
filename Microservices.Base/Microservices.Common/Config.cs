using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Microservices.Common
{
    public class Config
    {
        public static readonly Config Root = new Config(true);
        public static Config New { get { return new Config(true); } }
        private IConfiguration config = null;
        private bool isRoot = false;
        private Config(bool isRoot = false) { this.isRoot = isRoot; }
        private Config(IConfiguration config, bool isRoot = false) { this.config = config; this.isRoot = isRoot; }
        public IConfiguration GetConfig() { return config; }
        public Config Load(params string[] files)
        {
            if (!isRoot || config != null) throw new Exception("Config Build Error");
            var builder = new ConfigurationBuilder();
            foreach (var file in files)
            {
                var ext = new FileInfo(file).Extension;
                if (ext == ".xml")
                    builder.AddXmlFile(file, optional: true, reloadOnChange: true);
                else if (ext == ".json")
                    builder.AddJsonFile(file, optional: true, reloadOnChange: true);
            }
            config = builder.Build();
            return this;
        }
        public Config Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return this;
            else
            {
                var section = config?.GetChildren()?.FirstOrDefault(en => en.Key == key);
                return section == null ? new Config() : new Config(section);
            }
        }
        public Config this[string key] { get { return Get(key); } }
        public override string ToString() { return config == null ? null : ((IConfigurationSection)config).Value; }
        public static implicit operator string(Config config) { return config.ToString(); }
    }
}
