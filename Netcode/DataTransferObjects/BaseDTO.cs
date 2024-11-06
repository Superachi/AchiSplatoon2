using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers;
using Newtonsoft.Json;
using Terraria.ModLoader;

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

    public virtual void ApplyToModPlayer(ModPlayer modPlayer)
    {
        DebugHelper.PrintWarning("Tried to apply ModPlayer DTO, but the function to do so is not implemented.");
    }

    public virtual string Serialize()
    {
        return JsonConvert.SerializeObject(
            this,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }
        );
    }
}
