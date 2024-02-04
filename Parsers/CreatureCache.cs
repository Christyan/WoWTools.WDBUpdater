using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WoWTools.WDBUpdater.Parsers
{
    public class CreatureDisplayData
    {
        public UInt32 CreatureDisplayInfoID { get; set; }
        public Single CreatureScale { get; set; }
        public float CreatureProbability { get; set; }
    }
    public class CreatureCache : ICache<CreatureCache>
    {
        // public UInt32 TitleLen { get; set; }
        // public UInt32 TitleAltLen { get; set; }
        // public UInt32 CursorNameLen { get; set; }
        public bool Leader { get; set; }
        // public UInt32[] NameLen { get; set; } = new UInt32[4];
        public string[] Name { get; set; } = new string[4] {"", "", "", "" };
        // public UInt32[] NameAltLen { get; set; } = new UInt32[4];
        public string[] NameAlt { get; set; } = new string[4] { "", "", "", "" };
        public UInt32[] Flags { get; set; } = new UInt32[2];
        public Int32 CreatureType { get; set; }
        public Int32 CreatureFamily { get; set; }
        public Int32 Classification { get; set; }
        public UInt32[] ProxyCreatureID { get; set; } = new UInt32[2];
        public Int32 NumCreatureDisplays { get; set; }
        public Single TotalProbability { get; set; }
        public CreatureDisplayData[] DisplayData { get; set; } = new CreatureDisplayData[0];
        public Single HPMultiplier { get; set; }
        public Single EnergyMultiplier { get; set; }
        public Int32 NumQuestItems { get; set; }
        public Int32 NumQuestCurrencies { get; set; }
        public Int32 CreatureMovementInfoID { get; set; }
        public UInt32 HealthScalingExpansion { get; set; }
        public UInt32 RequiredExpansion { get; set; }
        public UInt32 VignetteID { get; set; }
        public UInt32 Class { get; set; }
        public UInt32 CreatureDifficultyID { get; set; }
        public UInt32 WidgetSetID { get; set; }
        public UInt32 WidgetSetUnitConditionID { get; set; }
        public String Title { get; set; } = "";
        public String TitleAlt { get; set; } = "";
        public String CursorName { get; set; } = "";
        public UInt32[] QuestItems { get; set; } = new uint[0];
        public UInt32[] QuestCurrencies { get; set; } = new uint[0];

        public UInt32 ID { get; private set; }
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
            return WDBFIles.CreatureCache;
        }

        public static Dictionary<UInt32, CreatureCache> Entries = new Dictionary<UInt32, CreatureCache>();
        public static CreatureCache GetData(UInt32 entry)
        {
            return Entries[entry];
        }
        public static List<UInt32> GetEntries()
        {
            return Entries.Keys.ToList();
        }
        public static bool Parse(WDBReader reader)
        {
            if (reader.Signature != WDBFIles.CreatureCache)
                return false;

            foreach (var element in reader.dataTable)
            {
                ByteBuffer _buffer = new ByteBuffer(element.Value);
                CreatureCache entry = ReadEntry(_buffer, reader.Build);
                entry.ID = Convert.ToUInt32(element.Key);
                Entries.Add(entry.ID, entry);
                Debug.Assert(_buffer.Rpos == _buffer.Size());
            }
            return true;
        }

        public static CreatureCache ReadEntry(ByteBuffer buffer, UInt32 build)
        {
            UInt32 TitleLen = buffer.ReadBits(11);
            UInt32 TitleAltLen = buffer.ReadBits(11);
            UInt32 CursorNameLen = buffer.ReadBits(6);
            UInt32[] NameLen = new UInt32[4];
            UInt32[] NameAltLen = new UInt32[4];

            CreatureCache _cache = new CreatureCache();
            _cache.Leader = buffer.ReadBit();

            for (int i = 0; i < 4; ++i)
            {
                NameLen[i] = buffer.ReadBits(11);
                NameAltLen[i] = buffer.ReadBits(11);
            }
            for (int i = 0; i < 4; ++i)
            {
                if (NameLen[i] > 1)
                    _cache.Name[i] = buffer.ReadString(NameLen[i]);
                if (NameAltLen[i] > 1)
                    _cache.NameAlt[i] = buffer.ReadString(NameAltLen[i]);
            }
            for (int i = 0; i < 2; ++i)
                _cache.Flags[i] = buffer.ReadUInt32();
            _cache.CreatureType = buffer.ReadInt32();
            _cache.CreatureFamily = buffer.ReadInt32();
            _cache.Classification = buffer.ReadInt32();
            for (int i = 0; i < 2; ++i)
                _cache.ProxyCreatureID[i] = buffer.ReadUInt32();

            if (build < 27101) // 8.0.1.27101
            {
                _cache.NumCreatureDisplays = 4;
                _cache.DisplayData = new CreatureDisplayData[4];
                for (int i = 0; i < 4; ++i)
                    _cache.DisplayData[i] = new CreatureDisplayData { CreatureDisplayInfoID = buffer.ReadUInt32() };
            }
            else
            {
                _cache.NumCreatureDisplays = buffer.ReadInt32();
                _cache.TotalProbability = buffer.ReadSingle();

                _cache.DisplayData = new CreatureDisplayData[_cache.NumCreatureDisplays];
                for (int i = 0; i < _cache.NumCreatureDisplays; ++i)
                    _cache.DisplayData[i] = new CreatureDisplayData { CreatureDisplayInfoID = buffer.ReadUInt32(), CreatureScale = buffer.ReadSingle(), CreatureProbability = buffer.ReadSingle() };
            }

            _cache.HPMultiplier = buffer.ReadSingle();
            _cache.EnergyMultiplier = buffer.ReadSingle();
            _cache.NumQuestItems = buffer.ReadInt32();
            if (_cache.NumQuestItems != 0)
                _cache.QuestItems = new UInt32[_cache.NumQuestItems];
            
            if (build >= 52902) // 10.2.5.52902
                _cache.NumQuestCurrencies = buffer.ReadInt32();

            _cache.CreatureMovementInfoID = buffer.ReadInt32();
            _cache.HealthScalingExpansion = buffer.ReadUInt32();
            _cache.RequiredExpansion = buffer.ReadUInt32();
            if (build >= 22248) // >= 7.0.3.22248
                _cache.VignetteID = buffer.ReadUInt32();

            if (build >= 27101) // >= 8.0.1.27101
                _cache.Class = buffer.ReadUInt32();

            if (build >= 28724 && build < 36216) // >= 8.1.0.28724 && < 9.0.1.36216
                buffer.ReadSingle(); // FadeRegionRadius

            if (build >= 39185) // 9.1.0.39185
                _cache.CreatureDifficultyID = buffer.ReadUInt32();

            if (build >= 29683) // >= 8.1.5.29683
            {
                _cache.WidgetSetID = buffer.ReadUInt32();
                _cache.WidgetSetUnitConditionID = buffer.ReadUInt32();
            }

            if (TitleLen > 1)
                _cache.Title = buffer.ReadString(TitleLen);
            if (TitleAltLen > 1)
                _cache.TitleAlt = buffer.ReadString(TitleAltLen);
            if (CursorNameLen > 1)
                _cache.CursorName = buffer.ReadString(CursorNameLen);
            for (int i = 0; i < _cache.NumQuestItems; ++i)
                _cache.QuestItems[i] = buffer.ReadUInt32();

            for (int i = 0; i < _cache.NumQuestCurrencies; ++i)
                _cache.QuestItems[i] = buffer.ReadUInt32();

            return _cache;
        }
    }
}
