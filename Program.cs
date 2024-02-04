using MySqlConnector;
using WoWTools.WDBUpdater.Parsers;

namespace WoWTools.WDBUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(String.Format("Usage: {0} wdbfile mysql/txt", System.AppDomain.CurrentDomain.FriendlyName));
                return;
            }

            WDBReader reader = new WDBReader(args[0]);
            reader.Read();

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
                    break;
                case "mysql":

                    using (var connection = new MySqlConnection(SettingsManager.connectionString))
                    {
                        connection.Open();
                        Utils.Dumper<QuestCache>.DumpToSql(connection, reader);
                        Utils.Dumper<CreatureCache>.DumpToSql(connection, reader);
                        Utils.Dumper<GameObejctCache>.DumpToSql(connection, reader);
                        Utils.Dumper<PageTextCache>.DumpToSql(connection, reader);
                    }
                    break;
            }


            Console.WriteLine(reader.dataTable.Count);
        }
    }
}
