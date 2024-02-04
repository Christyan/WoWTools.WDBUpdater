using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWTools.WDBUpdater.Parsers
{
    public class GameObejctCache : ICache<GameObejctCache>
    {
        public Int32 GameObjectType { get; set; }
        public Int32 GameObjectDisplayID { get; set; }
        public String[] Name { get; set; } = new string[4];
        public String IconName { get; set; } = "";
        public String CastBarCaption { get; set; } = "";
        public String UnkString { get; set; } = "";
        public Int32[] Data = new Int32[35];
        public Single Scale { get; set; }
        public UInt32 NumQuestItems { get; set; }
        public UInt32[] QuestItems { get; set; } = new uint[0];
        public UInt32 ExpansionID { get; set; }
        public UInt32 ContentTuningID { get; set; }


        public UInt32 ID { get; set; }
        public uint GetID()
        {
            return ID;
        }
        public string GetName()
        {
            return Name[0];
        }
        public static WDBFIles GetCacheType()
        {
            return WDBFIles.GameObjectCache;
        }

        public static Dictionary<UInt32, GameObejctCache> Entries = new Dictionary<UInt32, GameObejctCache>();
        public static GameObejctCache GetData(UInt32 entry)
        {
            return Entries[entry];
        }
        public static List<UInt32> GetEntries()
        {
            return Entries.Keys.ToList();
        }
        public static bool Parse(WDBReader reader)
        {
            if (reader.Signature != WDBFIles.GameObjectCache)
                return false;

            foreach (var element in reader.dataTable)
            {
                ByteBuffer _buffer = new ByteBuffer(element.Value);
                GameObejctCache entry = ReadEntry(_buffer, reader.Build);
                entry.ID = unchecked((UInt32)element.Key);
                Entries.Add(entry.ID, entry);
                Debug.Assert(_buffer.Rpos == _buffer.Size());
            }
            return true;
        }

        public static GameObejctCache ReadEntry(ByteBuffer buffer, UInt32 build)
        {
            uint datacount = 33;
            if (build >= 40871) // 9.1.5.40871
                datacount = 35;
            else if (build >= 27101) // 8.0.1.27101
                datacount = 34;

            GameObejctCache _cache = new GameObejctCache();
            _cache.GameObjectType = buffer.ReadInt32();
            _cache.GameObjectDisplayID = buffer.ReadInt32();
            for (int i = 0; i < 4; ++i)
                _cache.Name[i] = buffer.ReadString();
            _cache.IconName = buffer.ReadString();
            _cache.CastBarCaption = buffer.ReadString();
            _cache.UnkString = buffer.ReadString();
            for (int i = 0; i < datacount; ++i)
                _cache.Data[i] = buffer.ReadInt32();

            _cache.Scale = buffer.ReadSingle();
            _cache.NumQuestItems = buffer.ReadByte();
            if (_cache.NumQuestItems != 0)
            {
                _cache.QuestItems = new UInt32[_cache.NumQuestItems];
                for (int i = 0; i < _cache.NumQuestItems; ++i)
                    _cache.QuestItems[i] = buffer.ReadUInt32();
            }

            if (build < 36216) // 9.0.1.36216
                _cache.ExpansionID = buffer.ReadUInt32();
            else
                _cache.ContentTuningID = buffer.ReadUInt32();

            return _cache;
        }
    }
}
