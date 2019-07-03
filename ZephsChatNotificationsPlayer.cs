using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace ZephsChatNotifications
{
	class ZephsChatNotificationsPlayer : ModPlayer
    {
        public bool startedCrafting = false;

        //todo: use these instead of resetting global item vars
        public int startedCraftingItemType = 0;
        public int startedCraftingItemPrefix = 0;
        public int startedCraftingItemStack = 0;

        public Item lastMouseItem;

        public ZephsChatNotificationsPlayer()
		{

		}

        public override void PostUpdate()
        {
            base.PostUpdate();
            Player p = Main.player[Main.myPlayer];

            //inventory is closed, so we can't be crafting
            if (!Main.playerInventory)
            {
                startedCrafting = false;
            }

            //Inventory open, not crafting, mouse matches last crafted item
            //We must have started crafting, as last crafted item is reset when we finish crafting
            // so it won't match our mouse unless we craft something new (i.e. can't pick up the thing you last crafted to fool it)
            if (Main.playerInventory &&
                !startedCrafting &&
                Main.mouseItem.type == ZephsChatNotificationsGlobalItem.lastCraftedItemType)
            {
                startedCrafting = true;
                //Main.NewText("Started crafting " + "[i/p" + ZephsChatNotificationsGlobalItem.lastCraftedItemPrefix + ":" + ZephsChatNotificationsGlobalItem.lastCraftedItemType + "]");
            }

            //Inventory open, crafting, mouse item changes
            //If our mouse item changes then it means we've put down whatever we were crafting, so we're done.
            if (Main.playerInventory &&
                startedCrafting &&
                lastMouseItem.type != Main.mouseItem.type &&
                lastMouseItem.type != 0 && //Disregard empty mouse, since if it's empty it could mean that we've now started crafting
                ZephsChatNotificationsGlobalItem.lastCraftedItemType >= 0) //If it's -1 for some reason then don't print anything to the chat
            {
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)3);
                packet.Write((int)Main.LocalPlayer.whoAmI);
                packet.Write((int)ZephsChatNotificationsGlobalItem.lastCraftedItemStack);
                packet.Write((int)ZephsChatNotificationsGlobalItem.lastCraftedItemType);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    //Send to server since we're the client
                    packet.Send(-1);
                }

                startedCrafting = false;

                //Reset so that "Main.mouseItem.type == ZephsChatNotificationsGlobalItem.lastCraftedItemType" isn't true above
                ZephsChatNotificationsGlobalItem.lastCraftedItemType = -1;
                ZephsChatNotificationsGlobalItem.lastCraftedItemStack = -1;
                ZephsChatNotificationsGlobalItem.lastCraftedItemPrefix = -1;
            }

            lastMouseItem = Main.mouseItem;
        }
    }
}
