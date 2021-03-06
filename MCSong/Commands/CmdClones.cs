using System;
using System.Collections.Generic;
using System.Text;
//using MySql.Data.MySqlClient;
//using MySql.Data.Types;
using System.Data;

namespace MCSong
{
    class CmdClones : Command
    {

        public override string name { get { return "clones"; } }
        public override string[] aliases { get { return new string[] { "" }; } }
        public override CommandType type { get { return CommandType.Information; } }
        public override bool consoleUsable { get { return true; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdClones() { }

        public override void Use(Player p, string message)
        {
            if (message == "")
            {
                if (p == null)
                {
                    Help(p);
                    return;
                }
                message = p.name;
            }

            string originalName = message.ToLower();

            Player who = Player.Find(message);
            if (who == null)
            {
                Player.SendMessage(p, "Could not find player. Searching PlayerDB.");

                /*DataTable FindIP = MySQL.fillData("SELECT IP FROM Players WHERE Name='" + message + "'");

                if (FindIP.Rows.Count == 0) { Player.SendMessage(p, "Could not find any player by the name entered."); FindIP.Dispose(); return; }

                message = FindIP.Rows[0]["IP"].ToString();
                FindIP.Dispose();

                OfflinePlayer o = OfflinePlayer.Find(message);
                if (o == null) { Player.SendMessage(p, "Could not find any player by the name entered."); return; }
                message = o.ip;*/
                try
                {
                    message = Server.s.database.GetTable("Players").GetValue(Server.s.database.GetTable("Players").Rows.IndexOf(Server.s.database.GetTable("Players").GetRow(new string[] { "Name" }, new string[] { message })), "IP");
                }
                catch
                {
                    Player.SendMessage(p, "Could not find any player by the name entered.");
                    return;
                }
            }
            else
            {
                message = who.ip;
            }

            /*DataTable Clones = MySQL.fillData("SELECT Name FROM Players WHERE IP='" + message + "'");

            if (Clones.Rows.Count == 0) { Player.SendMessage(p, "Could not find any record of the player entered."); return; }

            List<string> foundPeople = new List<string>();
            for (int i = 0; i < Clones.Rows.Count; ++i)
            {
                if (!foundPeople.Contains(Clones.Rows[i]["Name"].ToString().ToLower()))
                    foundPeople.Add(Clones.Rows[i]["Name"].ToString().ToLower());
            }

            Clones.Dispose();*/
            List<string> foundPeople = new List<string>();
            /*Player.players.ForEach(delegate (Player pl)
            {
                if (pl.ip == message)
                    if (!foundPeople.Contains(pl.name))
                        foundPeople.Add(pl.name);
            });
            PlayerDB.allOffline.ForEach(delegate (OfflinePlayer op)
            {
                if (op.ip == message)
                    if (!foundPeople.Contains(op.name))
                        foundPeople.Add(op.name);
            });*/
            jDatabase.Table players = Server.s.database.GetTable("Players");
            List<List<string>> foundRows = players.GetRows("IP", message);
            foreach (List<string> row in foundRows)
                foundPeople.Add(players.GetValue(players.Rows.IndexOf(row), "Name"));
            if (foundPeople.Count <= 1) { Player.SendMessage(p, originalName + " has no clones."); return; }

            Player.SendMessage(p, "These people have the same IP address:");
            Player.SendMessage(p, string.Join(", ", foundPeople.ToArray()));
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/clones <name> - Finds everyone with the same IP as <name>");
        }
    }
}
