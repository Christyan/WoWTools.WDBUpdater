using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace WoWTools.WDBUpdater
{
    // from wowdev
    public enum WDBFIles
    {
        ArenaTeamCache =            0x5741544D, // ArenaTeamCache.wdb         WATM       Wrath ... Cata
        BattlePetNameCache =        0x5742504E, // BattlePetNameCache.wdb     WBPN       Mists ... WoD Not seen > 6.2.4
        CreatureCache =             0x574D4F42, // CreatureCache.wdb          WMOB
        DanceCache =                0x5744414E, // DanceCache.wdb             WDAN       Wrath ... Cata
        GameObjectCache =           0x57474F42, // GameObjectCache.wdb        WGOB
        GuildStatsCache =           0x57474C44, // GuildStatsCache.wdb        WGLD
        ItemCache =                 0x57494442, // ItemCache.wdb              WIDB       ≤ Wrath
        ItemNameCache =             0x574E4442, // ItemNameCache.wdb          WNDB       ≤ Wrath
        ItemTextCache =             0x57495458, // ItemTextCache.wdb          WITX
        NameCache =                 0x574E414D, // NameCache.wdb              WNAM
        NPCCache =                  0x574E5043, // NPCCache.wdb               WNPC
        PageTextCache =             0x57505458, // PageTextCache.wdb          WPTX
        PetitionCache =             0x5750544E, // PetitionCache.wdb          WPTN
        PetNameCache =              0x57504E4D, // PetNameCache.wdb           WPNM
        QuestCache =                0x57515354, // QuestCache.wdb             WQST
        RealmCache =                0x57524C4D, // RealmCache.wdb             WRLM       Mists ... WoD (6.0.1.18179) Not seen ≥ 6.2.3
        WOWCache =                  0x5752444E, // WOWCache.wdb               WRDN
    }

    public class WDBReader
    {
        private static int headerSize = 16;
        public string fileName {  get; set; }
        public Dictionary<Int32 /*entry*/, Byte[] /*data*/> dataTable = new Dictionary<int, byte[]>();
        public static Dictionary<WDBFIles /*magic*/, string /*tablename*/> WDBTables = new Dictionary<WDBFIles, string>
        {
            { WDBFIles.ArenaTeamCache, "" },
            { WDBFIles.BattlePetNameCache, "" },
            { WDBFIles.CreatureCache, "wdb_creatures" },
            { WDBFIles.DanceCache, "" },
            { WDBFIles.GameObjectCache, "wdb_gameobjects" },
            { WDBFIles.GuildStatsCache, "" },
            { WDBFIles.ItemCache, "" },
            { WDBFIles.ItemNameCache, "" },
            { WDBFIles.ItemTextCache, "" },
            { WDBFIles.NameCache, "" },
            { WDBFIles.NPCCache, "wdb_npctext" },
            { WDBFIles.PageTextCache, "wdb_pagetext" },
            { WDBFIles.PetitionCache, "" },
            { WDBFIles.PetNameCache, "" },
            { WDBFIles.QuestCache, "wdb_quests" },
            { WDBFIles.RealmCache, "" },
            { WDBFIles.WOWCache, "" },
        };

        // header
        public WDBFIles Signature { get; set; } = 0;
        public UInt32 Build { get; set; } = 0;
        public byte[] Locale { get; set; } = new byte[4];
        public UInt32 RecordSize { get; set; } = 0;
        public UInt32 RecordVersion { get; set; } = 0;
        public UInt32 CacheVersion { get; set; } = 0;


        public WDBReader(String _filename)
        {
            fileName = _filename;
        }

        public bool Read(HashSet<UInt32> acceptedBuilds)
        {
            using (Stream sr = File.Open(fileName, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(sr))
                {
                    if (reader.BaseStream.Length < headerSize)
                    {
                        Console.WriteLine(string.Format("File {0} is not a valid wdb file! Header size is wrong.", fileName));
                        return false;
                    }

                    Signature = (WDBFIles)reader.ReadUInt32();

                    if (!WDBTables.ContainsKey(Signature))
                    {
                        Console.WriteLine(string.Format("File {0} is not a valid wdb file!", fileName));
                        return false;
                    }

                    Build = reader.ReadUInt32();
                    if (acceptedBuilds != null && !acceptedBuilds.Contains(Build))
                    {
                        Console.WriteLine(string.Format("File {0} build ({1}) is not accepted!", fileName, Build));
                        return false;
                    }

                    if (Build >= 4500) // 1.6.0.4500
                        Locale = reader.ReadBytes(4);
                    RecordSize = reader.ReadUInt32();
                    RecordVersion = reader.ReadUInt32();
                    if (Build >= 9464) // 3.0.8.9464
                        CacheVersion = reader.ReadUInt32();

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        Int32 entry = reader.ReadInt32();
                        Int32 size = reader.ReadInt32();

                        if (size != 0)
                            dataTable.Add(entry, reader.ReadBytes(size));
                    }

                }
            }
            return true;
        }
    }
}
