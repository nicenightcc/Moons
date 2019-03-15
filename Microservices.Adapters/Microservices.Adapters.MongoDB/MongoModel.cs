using Microservices.Base;
using Newtonsoft.Json;

namespace Microservices.Adapters.IDatabase
{
    public abstract class MongoModel : ICache
    {
        [JsonIgnore]
        public virtual object _id { get; set; }
        public override bool Equals(object obj)
        {
            return this._id == ((MongoModel)obj)._id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
