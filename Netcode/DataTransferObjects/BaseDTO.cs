using Newtonsoft.Json;

namespace AchiSplatoon2.Netcode.DataTransferObjects;

internal class BaseDTO
{
    public virtual string CreateDTOSummary()
    {
        string output = "\n";
        output += $"Type: {GetType().Name}\n";
        output += JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        return output;
    }
}
