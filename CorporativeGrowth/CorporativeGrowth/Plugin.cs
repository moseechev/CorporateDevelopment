using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Random = UnityEngine.Random;

namespace CorporativeGrowth
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "corpgrowth";
        public override string Name => "CorporativeGrowth";
        public override string Author => "moseechev";
        public override System.Version Version { get; } = new System.Version(1, 0, 0);
        public static Plugin plugin;
        public override void OnEnabled()
        {
            plugin = this;
            Exiled.Events.Handlers.Server.RespawningTeam += OnTeamRespawn;
            Exiled.Events.Handlers.Player.Hurting += OnPlayerHurt;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
            Exiled.Events.Handlers.Player.ChangingRole += OnPlayerChaningRole;
            Exiled.Events.Handlers.Player.Died += OnPlayerDied;
            base.OnEnabled();
        }

        private void OnPlayerDied(DiedEventArgs ev)
        {
            ev.Target.CustomInfo = "";
        }

        private void OnPlayerChaningRole(ChangingRoleEventArgs ev)
        {
            ev.Player.CustomInfo = "";
        }

        private void OnPlayerLeft(LeftEventArgs ev)
        {
            ev.Player.CustomInfo = "";
        }

        private void OnPlayerHurt(HurtingEventArgs ev)
        {
            if (ev.Attacker != null)
            {
                if (ev.Attacker != ev.Target)
                {
                    if (ev.Target.CustomInfo == Config.CustomInfo && (ev.Attacker.Role.Team == Team.CHI || ev.Attacker.Role.Team == Team.CDP) && !Config.ChiFriendlyFire && !Server.FriendlyFire)
                    {
                        ev.Attacker.ShowHint(Config.ChiOnCgFF, 3);
                        ev.Amount = 0;
                        ev.IsAllowed = false;
                    }
                    else if (ev.Attacker.CustomInfo == Config.CustomInfo && (ev.Target.Role.Team == Team.CHI || ev.Target.Role.Team == Team.CDP) && !Config.ChiFriendlyFire && !Server.FriendlyFire)
                    {
                        ev.Attacker.ShowHint(Config.СпOnChiFF, 3);
                        ev.Amount = 0;
                        ev.IsAllowed = false;
                    }
                }
            }
        }

        private void OnTeamRespawn(RespawningTeamEventArgs ev)
        {
            if (Random.Range(0,101) <= Config.SpawnChance)
            {
                ev.IsAllowed = false;
                Timing.CallDelayed(3f, () => RespawningTeam(ev.Players));
            }
        }
        public void RespawningTeam(List<Player> players)
        {
            Cassie.MessageTranslated(Config.SpawnCassie, Config.SpawnCassieSubtitles, false, true, true);
            foreach (Player player in players)
            {
                if (player.CustomInfo != "ГОК")
                {
                    SpawnPlayer(player);
                }
            }
            CheckingWrongSpawn();
        }

        public void SpawnPlayer(Player player)
        {
            player.SetRole(RoleType.Tutorial);
            player.EnableEffect(Exiled.API.Enums.EffectType.Visual173Blink, 2.3f, false);
            Timing.CallDelayed(1f, () => player.ChangeAppearance(RoleType.Scientist));
            Timing.CallDelayed(3f, () => player.ChangeAppearance(RoleType.Scientist));
            Timing.CallDelayed(1f, () => player.Position = new Vector3(-55 + Random.Range(-1, 1), 990, -50 + Random.Range(-1, 1)));
            Timing.CallDelayed(2f, () =>
            {
                player.ClearInventory();
                foreach (ItemType item in Config.SpawnItems)
                {
                    player.AddItem(item);
                }
            });
            Timing.CallDelayed(4f, () => player.MaxHealth = Config.MaxHealth);
            Timing.CallDelayed(4.1f, () => player.Health = Config.MaxHealth);
            player.CustomInfo = Config.CustomInfo;
            player.Broadcast(15, Config.SpawnBroadcast, Broadcast.BroadcastFlags.Normal, true);
        }

        void CheckingWrongSpawn()
        {
            if (Round.IsStarted)
            {
                foreach (Player player in Player.List)
                {
                    if (player.CustomInfo == Config.CustomInfo &&
                        player.Position.z > -48 && player.Position.z < -37 &&
                        player.Position.x > 48 && player.Position.x < 59 &&
                        player.Position.y > 1014 && player.Position.y < 1029)
                    {
                        Timing.CallDelayed(0.1f, () => player.Position = new Vector3(-55 + Random.Range(-1, 1), 990, -50 + Random.Range(-1, 1)));
                    }
                }
                Timing.CallDelayed(5f, () => CheckingWrongSpawn());
            }
        }

        public override void OnDisabled()
        {
            plugin = this;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnTeamRespawn;
            Exiled.Events.Handlers.Player.Hurting -= OnPlayerHurt;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeft;
            Exiled.Events.Handlers.Player.ChangingRole -= OnPlayerChaningRole;
            Exiled.Events.Handlers.Player.Died -= OnPlayerDied;
            base.OnDisabled();
        }
    }
}
