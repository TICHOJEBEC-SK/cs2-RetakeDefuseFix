using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace RetakeDefuse
{
    public partial class RetakeDefuse : BasePlugin
    {
        public override string ModuleAuthor => "TICHOJEBEC";
        public override string ModuleName => "Defuser Fix";
        public override string ModuleVersion => "v1.0";

        public override void Load(bool hotReload)
        {
            RegisterEventHandler<EventBombPlanted>((@event, info) =>
            {
                GiveAllCtDefuseKit();
                return HookResult.Continue;
            });
        }

        private static List<CCSPlayerController> GetValidPlayers()
        {
            return Utilities.GetPlayers()
                .Where(player => player.IsValid && player.PlayerPawn.Value != null)
                .ToList();
        }

        public static bool HasWeapon(CCSPlayerController player, string weaponName)
        {
            if (!player.IsValid || !player.PawnIsAlive) return false;

            var pawn = player.PlayerPawn.Value;
            return pawn?.WeaponServices?.MyWeapons
                .Any(weapon => weapon?.Value?.IsValid == true && weapon.Value.DesignerName?.Contains(weaponName) == true) ?? false;
        }

        private HookResult GiveAllCtDefuseKit()
        { 
            var players = GetValidPlayers();  
            foreach (var player in players)
            {
                if (player.TeamNum == 3 && !HasWeapon(player, "item_defuser"))
                {
                    var playerPawn = player.PlayerPawn.Value;
                    if (playerPawn?.ItemServices != null)
                    {
                        var itemServices = new CCSPlayer_ItemServices(playerPawn.ItemServices.Handle);
                        itemServices.HasDefuser = true;
                    }
                }
            }
            return HookResult.Continue;
        }
    }
}
