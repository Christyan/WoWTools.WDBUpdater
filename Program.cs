using MySqlConnector;
using WoWTools.WDBUpdater.Parsers;

namespace WoWTools.WDBUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(String.Format("Usage: {0} wdbfile mysql/txt", System.AppDomain.CurrentDomain.FriendlyName));
                return;
            }

            HashSet<UInt32> acceptedBuild = null;

            if (args.Length >= 3)
            {
                if (args[2] == "onlyretail")
                {
                    acceptedBuild = new HashSet<UInt32>();
                    using (var connection = new MySqlConnection(SettingsManager.connectionString))
                    {
                        connection.Open();
                        using (var command = new MySqlCommand("SELECT build FROM `wowtools`.`wow_builds` WHERE `branch` = \"Retail\"", connection))
                        {
                            using (MySqlDataReader mreader = command.ExecuteReader())
                            {
                                while (mreader.Read())
                                {
                                    acceptedBuild.Add(mreader.GetUInt32(0));
                                }
                            }
                        }
                    }
                }
            }

            WDBReader reader = new WDBReader(args[0]);
            if (!reader.Read(acceptedBuild))
                return;

            QuestCache.Parse(reader);
            CreatureCache.Parse(reader);
            GameObejctCache.Parse(reader);
            PageTextCache.Parse(reader);

            switch (args[1])
            {
                case "txt":
                    Utils.Dumper<QuestCache>.DumpWDBText();
                    Utils.Dumper<CreatureCache>.DumpWDBText();
                    Utils.Dumper<GameObejctCache>.DumpWDBText();
                    Utils.Dumper<PageTextCache>.DumpWDBText();
                    Utils.Dumper<NpcCache>.DumpWDBText();
                    break;
                case "mysql":

                    using (var connection = new MySqlConnection(SettingsManager.connectionString))
                    {
                        connection.Open();
                        Utils.Dumper<QuestCache>.DumpToSql(connection, reader);
                        Utils.Dumper<CreatureCache>.DumpToSql(connection, reader);
                        Utils.Dumper<GameObejctCache>.DumpToSql(connection, reader);
                        Utils.Dumper<PageTextCache>.DumpToSql(connection, reader);
                        Utils.Dumper<NpcCache>.DumpToSql(connection, reader);
                    }
                    break;
            }


            Console.WriteLine(reader.dataTable.Count);
        }
    }
}
