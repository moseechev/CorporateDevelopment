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
    class SpawnCGSquad : ParentCommand
    {
        public override string Command { get; } = "spawncgsquad";
        public override string[] Aliases { get; } = { "scgs" };
        public override string Description { get; } = "Spawns Corporative Growth Squad";

        public override void LoadGeneratedCommands()
        {

        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("corpgrowth.spawnsquad"))
            {
                response = "You don't have permission to do that! You need corpgrowth.spawnsquad permission to execute this command. If you should have this permission but you don't for some reason, please report this to this server's owner.";
                return false;
            }

            try
            {
                List<Player> Spectators = Player.List.Where(x=>x.IsDead).ToList();
                if (Spectators.Count < 0)
                {
                    response = "There's not enough dead players on the map!";
                    return false;
                }
                List<Player> playersToSpawn = new List<Player>();
                for (int i = 0; i < Plugin.plugin.Config.MaxSquad && i < Spectators.Count; i++)
                {
                    playersToSpawn.Add(Spectators.RandomItem());
                }
                Plugin.plugin.RespawningTeam(playersToSpawn);
                response = $"Successfully spawned a squad of {playersToSpawn.Count} players!";
                return true;
            }
            catch (Exception e)
            {
                Log.Debug(e, true);
                response = e.ToString();
                return false;
            }
        }
    }
}
