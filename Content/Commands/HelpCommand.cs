using System.Diagnostics;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Commands
{
    internal class HelpCommand : ModCommand
    {
        // CommandType.World means that command can be used in Chat in SP and MP, but executes on the Server in MP
        public override CommandType Type
            => CommandType.World;

        public override string Command
            => "woomyWiki";

        public override string Usage
            => "/woomyWiki";

        public override string Description
            => "Opens a browser tab with the Woomy Mod Wiki";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Process.Start("explorer.exe", "https://superachi.github.io/WoomyModPages/");
        }
    }
}
