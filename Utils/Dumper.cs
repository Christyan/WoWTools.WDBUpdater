using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWTools.WDBUpdater.Parsers;

namespace WoWTools.WDBUpdater.Utils
{
    public static class Dumper<T> where T : ICache<T>
    {
        public static void DumpWDBText()
        {
            List<UInt32> entries = T.GetEntries();
            if (entries.Count == 0)
                return;

            Console.WriteLine("-- ");
            Console.WriteLine(String.Format("-- Dumping {0} {1}...", entries.Count, T.GetCacheType()));
            Console.WriteLine("-- ");

            foreach (var element in entries)
            {
                var str = Utils.WowToolsJsonSerializer.Serialize(T.GetData(element));
                Console.WriteLine(str);
                //break;
            }
        }

        public static void DumpToSql(MySqlConnection connection, WDBReader reader)
        {
            List<UInt32> entries = T.GetEntries();
            if (entries.Count == 0)
                return;

            String tablename = WDBReader.WDBTables[T.GetCacheType()];

            Dictionary<UInt32, Tuple<UInt32/*firstseenbuild*/, UInt32 /*lastupdatedbuild*/>> inTable = new Dictionary<UInt32, Tuple<UInt32, UInt32>>();

            // load builds from table
            using (var command = new MySqlCommand("SELECT id, firstseenbuild, lastupdatedbuild FROM " + tablename + " WHERE `id` IN (" + String.Join(",", entries) + ")", connection))
            {
                using (MySqlDataReader mreader = command.ExecuteReader())
                {
                    while (mreader.Read())
                    {
                        UInt32 id = mreader.GetUInt32(0);
                        Tuple<UInt32, UInt32> builds = Tuple.Create(mreader.GetUInt32(1), mreader.GetUInt32(2));
                        inTable.Add(id, builds);
                    }
                }
            }



            foreach (var element in entries)
            {
                T data = T.GetData(element);
                
                var str = Utils.WowToolsJsonSerializer.Serialize(data);

                if (inTable.ContainsKey(data.GetID()))
                {
                    Tuple<UInt32, UInt32> builds = inTable[data.GetID()];
                    if (builds.Item2 >= reader.Build) // if lastupdatedbuild is newwst
                        continue;

                    if (builds.Item1 > reader.Build) // if firstseen is higher
                    {
                        using (var command = new MySqlCommand("UPDATE `" + tablename + "` SET `firstseenbuild`=@build WHERE `id`=@id", connection))
                        {
                            command.Parameters.AddWithValue("id", data.GetID());
                            command.Parameters.AddWithValue("build", reader.Build);
                            using (MySqlDataReader mreader = command.ExecuteReader())
                            {

                            }
                        }
                        continue;
                    }
                }

                using (var command = new MySqlCommand("INSERT INTO `" + tablename + "` (`id`, `name`, `json`, `firstseenbuild`, `lastupdatedbuild`) VALUES (@id, @name, @json, @build, @build) ON DUPLICATE KEY UPDATE `lastupdatedbuild` = @build, json=@json ", connection))
                {
                    command.Parameters.AddWithValue("id", data.GetID());
                    command.Parameters.AddWithValue("json", str);
                    command.Parameters.AddWithValue("name", data.GetName());
                    command.Parameters.AddWithValue("build", reader.Build);
                    using (MySqlDataReader mreader = command.ExecuteReader())
                    {

                    }
                }
            }
        }
    }
}
