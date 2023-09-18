using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object update) => update is null ? default : JsonConvert.SerializeObject(update);
    }
}
