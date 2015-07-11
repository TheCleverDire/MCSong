using System;

namespace MCSong
{
    public class CmdOpChat : Command
    {
        public override string name { get { return "opchat"; } }
        public override string[] aliases { get { return new string[] { "" }; } }
        public override CommandType type { get { return CommandType.Other; } }
        public override bool consoleUsable { get { return false; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdOpChat() { }

        public override void Use(Player p, string message)
        {
            p.opchat = !p.opchat;
            if (p.opchat) Player.SendMessage(p, "All messages will now be sent to OPs only");
            else Player.SendMessage(p, "OP chat turned off");
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/opchat - Makes support messages sent go to OPs by default");
        }
    }
}