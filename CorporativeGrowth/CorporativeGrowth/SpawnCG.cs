using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorporativeGrowth
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    class SpawnCG : ParentCommand
    {
        public override string Command { get; } = "spawncg";
        public override string[] Aliases { get; } = { "scg" };
        public override string Description { get; } = "Spawns Corporative Growth member";

        public override void LoadGeneratedCommands()
        {

        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("corpgrowth.spawn"))
            {
                response = "You don't have permission to do that! You need corpgrowth.spawn permission to commit this command. If you must have this permission but you don't for some reason, please report this to local server's admins.";
                return false;
            }

            if (arguments.Count == 0)
            {
                Player sender1 = Player.Get(sender);

                if (sender1.CustomInfo == Plugin.plugin.Config.CustomInfo)
                {
                    response = $"Player {sender1.Nickname} is already a CG member!";
                    return false;
                }

                Plugin.plugin.SpawnPlayer(sender1);
                Log.Debug($"Player {Player.Get(sender).Nickname} with ID of {Player.Get(sender).CustomUserId} spawned themselves as a CG member.", Plugin.plugin.Config.DebugMode);
                response = $"Player {sender1.Nickname} became a CG member.";
                return true;
            }

            Player player = Player.Get(arguments.At(0));
            if (player == null)
            {
                response = "This player doesn't exist. Player's ID or their nickname is required!";
                return false;
            }

            if (player.CustomInfo == Plugin.plugin.Config.CustomInfo)
            {
                response = $"Player {player.Nickname} is already a CG member!";
                return false;
            }
            else
            {
                Plugin.plugin.SpawnPlayer(player);
                Log.Debug($"Player {Player.Get(sender).Nickname} with ID of {Player.Get(sender).CustomUserId} spawned {player.Nickname} as a CG member.", Plugin.plugin.Config.DebugMode);
                response = $"Became {player.Nickname} became a CG member!";
                return true;
            }
        }
    }
}
