using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWTools.WDBUpdater.Parsers
{
    public class NpcCache : ICache<NpcCache>
    {
        public Single[] Probality = new Single[8];
        public UInt32[] BroadcastTextID = new UInt32[8];

        public UInt32 ID { get; set; }
        public uint GetID()
        {
            return ID;
        }
        public string GetName()
        {
            return ID.ToString();
        }

        public static WDBFIles GetCacheType()
        {
            return WDBFIles.NPCCache;
        }

        public static Dictionary<UInt32, NpcCache> Entries = new Dictionary<UInt32, NpcCache>();

        public static NpcCache GetData(uint entry)
        {
            return Entries[entry];
        }

        public static List<uint> GetEntries()
        {
            return Entries.Keys.ToList();
        }

        public static bool Parse(WDBReader reader)
        {
            if (reader.Signature != WDBFIles.NPCCache)
                return false;

            foreach (var element in reader.dataTable)
            {
                ByteBuffer _buffer = new ByteBuffer(element.Value);
                NpcCache entry = ReadEntry(_buffer, reader.Build);
                entry.ID = unchecked((uint)element.Key);
                Entries.Add(entry.ID, entry);
                Debug.Assert(_buffer.Rpos == _buffer.Size());
            }
            return true;
        }

        public static NpcCache ReadEntry(ByteBuffer buffer, uint build)
        {
            NpcCache _cache = new NpcCache();
            for (int i = 0; i < 8; ++i)
                _cache.Probality[i] = buffer.ReadSingle();

            for (int i = 0; i < 8; ++i)
                _cache.BroadcastTextID[i] = buffer.ReadUInt32();
            return _cache;
        }
    }
}
