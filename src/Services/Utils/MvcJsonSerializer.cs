using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CallGate.Services.Utils
{
    public class MvcJsonSerializer : IJsonSerializer
    {
        private readonly IOptions<MvcJsonOptions> _mvcJsonOptions;

        public MvcJsonSerializer(IOptions<MvcJsonOptions> mvcJsonOptions)
        {
            _mvcJsonOptions = mvcJsonOptions;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _mvcJsonOptions.Value.SerializerSettings);
        }
    }
}