using AchiSplatoon2.Content.AsepriteAnimationData.Models;
using AchiSplatoon2.Helpers;
using Newtonsoft.Json;
using System.IO;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.EnumsAndConstants
{
    internal static class AnimationDataPaths
    {
        private static readonly string _animationJsonRootPaths = $"Content\\AsepriteAnimationData\\Json";

        public static AnimationFrames? ToAnimationFrames(this string jsonPath)
        {
            using (Stream data = ModContent.GetInstance<AchiSplatoon2>().GetFileStream(jsonPath))
            {
                StreamReader reader = new StreamReader(data);
                string json = reader.ReadToEnd();
                reader.Close();

                var frameData = JsonConvert.DeserializeObject<AnimationFrames>(json);
                if (frameData == null)
                {
                    DebugHelper.PrintWarning($"Failed to retrieve animation data for {jsonPath}");
                }

                return frameData;
            }
        }

        private static string FormatPath(string filename, string directory = "")
        {
            var path = Path.Combine(_animationJsonRootPaths, directory, filename);
            path += ".json";

            return path;
        }

        public static string Tacticooler => FormatPath(nameof(Tacticooler));
    }
}
