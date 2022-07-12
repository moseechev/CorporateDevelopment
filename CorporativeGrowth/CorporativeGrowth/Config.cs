using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorporativeGrowth
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int SpawnChance { get; set; } = 25;
        public List<ItemType> SpawnItems { get; set; } = new List<ItemType>() { ItemType.ArmorHeavy, ItemType.KeycardChaosInsurgency, ItemType.GunFSP9 };
        public int MaxHealth { get; set; } = 100;
        public string SpawnBroadcast { get; set; } = "You are <color=#000000>Corporate Development</color>!";
        public string SpawnCassie { get; set; } = "Unauthorized personnel HasEntered at gate a entrance";
        public string CustomInfo { get; set; } = "Corporate Development";
        public string SpawnCassieSubtitles { get; set; } = "Unauthorized personnel has entered the facility at Gate A entrance.";
        public bool ChiFriendlyFire { get; set; } = false;
        public string ChiOnCgFF { get; set; } = "<color=#FF0000>You can't damage the Corporate Development!</color>";
        public string СпOnChiFF { get; set; } = "<color=#FF0000>You can't damage the Insurgency!</color>";
        public int MaxSquad { get; set; } = 30;
        public bool DebugMode { get; set; } = true;
    }
}
