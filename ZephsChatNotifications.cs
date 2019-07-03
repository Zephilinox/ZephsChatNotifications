using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ZephsChatNotifications
{
	class ZephsChatNotifications : Mod
	{
		public static ZephsChatNotifications instance;
		internal ZephsChatNotificationsGlobalItem zephsChatNotificationsGlobalItem;

		public ZephsChatNotifications()
		{
            Properties = new ModProperties()
            {
                Autoload = true,
            };
		}
		
		public override void Load()
		{
			instance = this;
			zephsChatNotificationsGlobalItem = (ZephsChatNotificationsGlobalItem)GetGlobalItem("ZephsChatNotificationsGlobalItem");
        }
		
		public override void Unload()
		{
			instance = null;
		}

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case 1: //Pickup
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        Main.NewText("[n:" + Main.player[reader.ReadInt32()].name + "] Picked Up " + "[i/p" + reader.ReadInt32() + ":" + reader.ReadInt32() + "]");
                    }
                    else if (Main.netMode == NetmodeID.Server) //would singleplayer ever get here anyway?
                    {
                        //Recreate the packet we received
                        ModPacket p = GetPacket();
                        p.Write((byte)1);
                        p.Write(reader.ReadInt32());
                        p.Write(reader.ReadInt32());
                        p.Write(reader.ReadInt32());
                        //Send it to everyone, including the client who sent it.
                        p.Send(-1);
                    }

                    break;
                case 2: //Crafted item with prefix
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        Main.NewText("[n:" + Main.player[reader.ReadInt32()].name + "] Crafted " + "[i/p" + reader.ReadInt32() + ":" + reader.ReadInt32() + "]");
                    }
                    else if (Main.netMode == NetmodeID.Server) //would singleplayer ever get here anyway?
                    {
                        //Recreate the packet we received
                        ModPacket p = GetPacket();
                        p.Write((byte)2);
                        p.Write(reader.ReadInt32());
                        p.Write(reader.ReadInt32());
                        p.Write(reader.ReadInt32());
                        //Send it to everyone, including the client who sent it.
                        p.Send(-1);
                    }
                    break;
                case 3: //Crafted item with stacks
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        Main.NewText("[n:" + Main.player[reader.ReadInt32()].name + "] Crafted " + "[i/s" + reader.ReadInt32() + ":" + reader.ReadInt32() + "]");
                    }
                    else if (Main.netMode == NetmodeID.Server) //would singleplayer ever get here anyway?
                    {
                        //Recreate the packet we received
                        ModPacket p = GetPacket();
                        p.Write((byte)3);
                        p.Write(reader.ReadInt32());
                        p.Write(reader.ReadInt32());
                        p.Write(reader.ReadInt32());
                        //Send it to everyone, including the client who sent it.
                        p.Send(-1);
                    }
                    break;
            }
        }
    }
}
