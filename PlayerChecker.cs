/* 更新履歴
 *  1.0.0
 *   初期版
 */


using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Player Checker", "Babu77", "1.0.0")]
    public class PlayerChecker : RustPlugin
    {
        #region Declarations
        private const string HammerShortname = "hammer";
        private const BUTTON UseKey = BUTTON.RELOAD;
        private const string PermUse = "playerchecker.use";
        #endregion
        
        #region OxideHooks
        private void Loaded()
        {
            permission.RegisterPermission(PermUse, this);
        }
        
        private void OnPlayerInput(BasePlayer player, InputState input)
        {
            if (!permission.UserHasPermission(player.UserIDString, PermUse))
            {
                return;
            }
            
            if (player == null || !player.IsAlive() || player.IsSleeping())
                return;
            
            if (!IsHoldingHammer(player))
                return;
            
            if (input.WasJustPressed(UseKey))
            {
                Ray ray = new Ray(player.eyes.position, player.eyes.BodyForward());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    var targetPlayer = hit.GetEntity() as BasePlayer;
                    if (targetPlayer != null)
                    {
                        ChatToPlayerInfo(player, targetPlayer);
                        LogPlayerInfo(player, targetPlayer);
                    }
                }
            }
        }
        #endregion

        #region Helpers
        private static bool IsHoldingHammer(BasePlayer player)
        {
            var heldItem = player.GetActiveItem();
            if (heldItem == null)
                return false;
            
            return heldItem.info.shortname == HammerShortname;
        }

        private void ChatToPlayerInfo(BasePlayer player, BasePlayer target)
        {
            var msg = $"<color=yellow>[Player Information]</color>\nPlayer Name: {target.displayName}\nPlayer ID: {target.UserIDString}";
            PrintToChat(player, msg);
        }

        private void LogPlayerInfo(BasePlayer player, BasePlayer target)
        {
            Puts($"Player {player.displayName} is looking at {target.displayName}");
            Puts($"Target Info: ID={target.UserIDString}, Health={target.Health()}, Position={target.transform.position}");
        }
        #endregion
    }
}