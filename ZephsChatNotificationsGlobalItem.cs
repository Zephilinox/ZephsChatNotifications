using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;

namespace ZephsChatNotifications
{
	class ZephsChatNotificationsGlobalItem : GlobalItem
	{
        public static int lastCraftedItemType = -1;
        public static int lastCraftedItemPrefix = -1;
        public static int lastCraftedItemStack = -1;

        public ZephsChatNotificationsGlobalItem()
		{
			
		}
		
		public override bool OnPickup(Item item, Player player)
		{
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                return true;
            }

			var zephsChatNotificationsPlayer = player.GetModPlayer<ZephsChatNotificationsPlayer>(mod);
            if (item.maxStack == 1 && item.value > 0) //Exclude hearts & mana stars, also wood armor, so maybe figure out a different solution
            {
                //Don't need to check client vs server, only client matters here
                ModPacket p = mod.GetPacket();
                p.Write((byte)1);
                p.Write((int)player.whoAmI);
                p.Write((int)item.prefix);
                p.Write((int)item.type);

                //Send to server since we're the client
                p.Send(-1);
            }

            return true;
        }

        public override void OnCraft(Item item, Recipe recipe)
        {
            //Fix single-player error message
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                return;
            }

            if (item.maxStack == 1)
            {
                ModPacket p = mod.GetPacket();
                p.Write((byte)2);
                p.Write((int)Main.LocalPlayer.whoAmI);
                p.Write((int)item.prefix);
                p.Write((int)item.type);

                //Send to server since we're the client
                p.Send(-1);
            }

            lastCraftedItemType = item.type;
            lastCraftedItemPrefix = item.prefix;
            lastCraftedItemStack = item.stack;
        }
    }
}
