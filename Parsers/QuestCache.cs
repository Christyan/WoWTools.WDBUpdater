using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WoWTools.WDBUpdater.Parsers
{
    class QuestRewardSpell
    {
        public UInt32 SpellID { get; set; }
        public UInt32 PlayerConditionID { get; set; }
        public UInt32 DisplayType { get; set; }
    }

    class QuestObjective
    {
        public UInt32 ID { get; set; }
        public byte Type { get; set; }
        public sbyte StorageIndex { get; set; }
        public Int32 ObjectID { get; set; }
        public Int32 Amount { get; set; }
        public UInt32 Flags { get; set; }
        public UInt32 Flags2 { get; set; }
        public Single PercentAmount { get; set; }
        public UInt32 NumVisualEffects { get; set; }
        public List<UInt32> VisualEffect { get; set; } = new List<uint>();
        //public UInt32 DescriptionLen { get; set; }
        public String Description { get; set; } = "";
    }

    class QuestConditionalQuestText
    {
        public UInt32 PlayerConditionID { get; set; }
        public UInt32 QuestGiverCreatureID { get; set; }
        public UInt32 TextLen { get; set; }
        public String Text { get; set; } = "";
    }

    class QuestCache : ICache<QuestCache>
    {
        public UInt32 QuestID { get; set; }
        public UInt32 QuestType { get; set; }
        public UInt32 QuestPackageID { get; set; }
        public UInt32 ContentTuningID { get; set; }
        public Int32 AreaTableID { get; set; }
        public UInt32 QuestInfoID { get; set; }
        public UInt32 SuggestedGroupNum { get; set; }
        public UInt32 RewardNextQuest { get; set; }
        public UInt32 RewardXPDifficulty { get; set; }
        public float RewardXPMultiplier { get; set; }
        public Int32 RewardMoney { get; set; }
        public UInt32 RewardMoneyDifficulty { get; set; }
        public float RewardMoneyMultiplier { get; set; }
        public UInt32 RewardBonusMoney { get; set; }
        public UInt32 RewardDisplaySpellCount { get; set; }
        public UInt32 RewardSpell { get; set; }
        public UInt32 RewardHonorAddition { get; set; }
        public UInt32 RewardHonorMultiplier { get; set; }
        public UInt32 RewardArtifactXPDifficulty { get; set; }
        public float RewardArtifactXPMultiplier { get; set; }
        public UInt32 RewardArtifactCategoryID { get; set; }
        public UInt32 ProvidedItem { get; set; }
        public UInt32[] Flags { get; set; } = new uint[3];
        // for 4
        public UInt32[] RewardFixedItemID { get; set; } = new uint[4];
        public UInt32[] RewardFixedItemQuantity { get; set; } = new uint[4];
        public UInt32[] ItemDropItemID { get; set; } = new uint[4];
        public UInt32[] ItemDropItemQuantity { get; set; } = new uint[4];
        //for end

        // for 6
        public UInt32[] RewardChoiceItemItemID { get; set; } = new uint[6];
        public UInt32[] RewardChoiceItemItemQuantity { get; set; } = new uint[6];
        public UInt32[] RewardChoiceItemItemDisplayID { get; set; } = new uint[6];
        // for end

        public UInt32 POIContinent { get; set; }
        public float POIx { get; set; }
        public float POIy { get; set; }
        public UInt32 POIPriority { get; set; }
        public UInt32 RewardTitle { get; set; }
        public UInt32 RewardArenaPoints { get; set; }
        public UInt32 RewardSkillLineID { get; set; }
        public UInt32 RewardNumSkillUps { get; set; }
        public UInt32 PortraitGiverDisplayID { get; set; }
        public UInt32 PortraitGiverMountDisplayID { get; set; }
        public UInt32 PortraitModelSceneID { get; set; }
        public UInt32 PortraitTurnInDisplayID { get; set; }

        // for 5
        public UInt32[] FactionID { get; set; } = new uint[5];
        public Int32[] FactionValue { get; set; } = new int[5];
        public UInt32[] FactionOverride { get; set; } = new uint[5];
        public Int32[] FactionGainMaxRank { get; set; } = new int[5];
        // for end

        public UInt32 RewardFactionFlags { get; set; }

        // for 4
        public UInt32[] RewardCurrencyID { get; set; } = new uint[4];
        public UInt32[] RewardCurrencyQuantity { get; set; } = new uint[4];
        // for end

        public UInt32 AcceptedSoundKitID { get; set; }
        public UInt32 CompleteSoundKitID { get; set; }
        public UInt32 AreaGroupID { get; set; }
        public UInt64 TimeAllowed { get; set; }
        public UInt32 NumObjectives { get; set; }
        public UInt64 RaceFlags { get; set; }
        public UInt32 QuestRewardID { get; set; }
        public UInt32 ExpansionID { get; set; }
        public Int32 ManagedWorldStateID { get; set; }
        public UInt32 QuestSessionBonus { get; set; }
        public UInt32 QuestGiverCreatureID { get; set; }
        public UInt32 NumConditionalQuestDescription { get; set; }
        public UInt32 NumConditionalQuestCompletionLog { get; set; }

        public List<QuestRewardSpell> RewardDisplay = new List<QuestRewardSpell>();

        [Newtonsoft.Json.JsonIgnore]
        public UInt32 LogTitleLen { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public UInt32 LogDescriptionLen { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public UInt32 QuestDescriptionLen { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public UInt32 AreaDescriptionLen { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public UInt32 PortraitGiverTextLen { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public UInt32 PortraitGiverNameLen { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public UInt32 PortraitTurnInTextLen { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public UInt32 PortraitTurnInNameLen { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public UInt32 QuestCompletionLogLen { get; set; }
        public bool ReadyForTranslation { get; set; }

        public List<QuestObjective> Objective { get; set; } = new List<QuestObjective>();

        public string LogTitle { get; set; } = "";
        public string LogDescription { get; set; } = "";
        public string QuestDescription { get; set; } = "";
        public string AreaDescription { get; set; } = "";
        public string PortraitGiverText { get; set; } = "";
        public string PortraitGiverName { get; set; } = "";
        public string PortraitTurnInText { get; set; } = "";
        public string PortraitTurnInName { get; set; } = "";
        public string QuestCompletionLog { get; set; } = "";

        // ConditionalQuestDescriptionSize
        public List<QuestConditionalQuestText> ConditionalQuestDescription { get; set; } = new List<QuestConditionalQuestText>();

        // ConditionalQuestCompletionLogSize
        public List<QuestConditionalQuestText> ConditionalQuestCompletionLog { get; set; } = new List<QuestConditionalQuestText>();

        public uint GetID()
        {
            return unchecked((uint)QuestID);
        }
        public string GetName()
        {
            return LogTitle;
        }
        public static WDBFIles GetCacheType()
        {
            return WDBFIles.QuestCache;
        }

        public static Dictionary<UInt32, QuestCache> Entries = new Dictionary<UInt32, QuestCache>();
        public static QuestCache GetData(UInt32 entry)
        {
            return Entries[entry];
        }
        public static List<UInt32> GetEntries()
        {
            return Entries.Keys.ToList();
        }
        public static bool Parse(WDBReader reader)
        {
            if (reader.Signature != WDBFIles.QuestCache)
                return false;

            Dictionary<UInt32, QuestCache> entries = new Dictionary<uint, QuestCache>();
            foreach (var element in reader.dataTable)
            {
                ByteBuffer _buffer = new ByteBuffer(element.Value);
                QuestCache entry = ReadEntry(_buffer, reader.Build);
                Entries.Add(entry.QuestID, entry);
                Debug.Assert(_buffer.Rpos == _buffer.Size());
            }
            return true;
        }

        public static QuestCache ReadEntry(ByteBuffer buffer, UInt32 build)
        {
            QuestCache _cache = new QuestCache();

            _cache.QuestID = buffer.ReadUInt32();
            _cache.QuestType = buffer.ReadUInt32();

            if (build < 36216) // < 9.0.1.36216
            {
                buffer.ReadUInt32(); // questlevel
                if (build >= 27101) // 8.0.1.27101
                    buffer.ReadUInt32(); // QuestScalingFactionGroup
                if (build >= 25860) // < 7.3.5.25860
                    buffer.ReadUInt32(); // scalemaxlevel
            }

            buffer.ReadUInt32(); // QuestPackageID
            if (build < 36216)
                buffer.ReadUInt32(); // Minlevel
            else
                _cache.ContentTuningID = buffer.ReadUInt32();
            _cache.AreaTableID = buffer.ReadInt32();
            _cache.QuestInfoID = buffer.ReadUInt32();
            _cache.SuggestedGroupNum = buffer.ReadUInt32();
            _cache.RewardNextQuest = buffer.ReadUInt32();
            _cache.RewardXPDifficulty = buffer.ReadUInt32();
            _cache.RewardXPMultiplier = buffer.ReadSingle();
            _cache.RewardMoney = buffer.ReadInt32();
            _cache.RewardMoneyDifficulty = buffer.ReadUInt32();
            _cache.RewardMoneyMultiplier = buffer.ReadSingle();
            _cache.RewardBonusMoney = buffer.ReadUInt32();

            if (build < 36216) // < 9.0.1.36216
            {
                if (build < 22248) // < 7.0.3.22248
                {
                    _cache.RewardDisplaySpellCount = 1;
                    _cache.RewardDisplay.Add(new QuestRewardSpell { SpellID = buffer.ReadUInt32() });
                }
                else
                {
                    _cache.RewardDisplaySpellCount = 3;
                    for (int i = 0; i < 3; ++i)
                        _cache.RewardDisplay.Add(new QuestRewardSpell { SpellID = buffer.ReadUInt32() });
                }
            }
            else
                _cache.RewardDisplaySpellCount = buffer.ReadUInt32();

            _cache.RewardSpell = buffer.ReadUInt32();
            _cache.RewardHonorAddition = buffer.ReadUInt32();
            _cache.RewardHonorMultiplier = buffer.ReadUInt32();
            if (build >= 22248) // >= 7.0.3.22248
            {
                _cache.RewardArtifactXPDifficulty = buffer.ReadUInt32();
                _cache.RewardArtifactXPMultiplier = buffer.ReadSingle();
                _cache.RewardArtifactCategoryID = buffer.ReadUInt32();
            }
            _cache.ProvidedItem = buffer.ReadUInt32();
            if (build < 27101) // < 8.0.1.27101
            {
                for (int i = 0; i < 2; ++i)
                    _cache.Flags[i] = buffer.ReadUInt32();
            }
            else
            {
                for (int i = 0; i < 3; ++i)
                    _cache.Flags[i] = buffer.ReadUInt32();
            }

            for (int i = 0; i < 4; ++i)
            {
                _cache.RewardFixedItemID[i] = buffer.ReadUInt32();
                _cache.RewardFixedItemQuantity[i] = buffer.ReadUInt32();
                _cache.ItemDropItemID[i] = buffer.ReadUInt32();
                _cache.ItemDropItemQuantity[i] = buffer.ReadUInt32();
            }

            for (int i = 0; i < 6; ++i)
            {
                _cache.RewardChoiceItemItemID[i] = buffer.ReadUInt32();
                _cache.RewardChoiceItemItemQuantity[i] = buffer.ReadUInt32();
                _cache.RewardChoiceItemItemDisplayID[i] = buffer.ReadUInt32();
            }

            _cache.POIContinent = buffer.ReadUInt32();
            _cache.POIx = buffer.ReadSingle();
            _cache.POIy = buffer.ReadSingle();
            _cache.POIPriority = buffer.ReadUInt32();


            _cache.RewardTitle = buffer.ReadUInt32();
            if (build < 22248) // < 7.0.3.22248
                buffer.ReadUInt32(); // talents
            _cache.RewardArenaPoints = buffer.ReadUInt32();
            _cache.RewardSkillLineID = buffer.ReadUInt32();
            _cache.RewardNumSkillUps = buffer.ReadUInt32();

            _cache.PortraitGiverDisplayID = buffer.ReadUInt32();
            if (build >= 27101) // 8.0.1.27101
                _cache.PortraitGiverMountDisplayID = buffer.ReadUInt32();
            if (build >= 39185) // 9.1.0.39185
                _cache.PortraitModelSceneID = buffer.ReadUInt32();
            _cache.PortraitTurnInDisplayID = buffer.ReadUInt32();

            for (int i = 0; i < 5; ++i)
            {
                _cache.FactionID[i] = buffer.ReadUInt32();
                _cache.FactionValue[i] = buffer.ReadInt32();
                _cache.FactionOverride[i] = buffer.ReadUInt32();
                if (build >= 22248) // >= 7.0.3.22248
                    _cache.FactionGainMaxRank[i] = buffer.ReadInt32();
            }

            _cache.RewardFactionFlags = buffer.ReadUInt32();
            for (int i = 0; i < 4; ++i)
            {
                _cache.RewardCurrencyID[i] = buffer.ReadUInt32();
                _cache.RewardCurrencyQuantity[i] = buffer.ReadUInt32();
            }

            _cache.AcceptedSoundKitID = buffer.ReadUInt32();
            _cache.CompleteSoundKitID = buffer.ReadUInt32();
            _cache.AreaGroupID = buffer.ReadUInt32();
            if (build >= 50401) // 10.1.5.50401
                _cache.TimeAllowed = buffer.ReadUInt64();
            else
                _cache.TimeAllowed = buffer.ReadUInt32();
            _cache.NumObjectives = buffer.ReadUInt32();
            if (build < 25860) // < 7.3.5.25860
                _cache.RaceFlags = buffer.ReadUInt32();
            else
                _cache.RaceFlags = buffer.ReadUInt64();

            if (build >= 22248) // >= 7.0.3.22248
            {
                _cache.QuestRewardID = buffer.ReadUInt32();
                if (build >= 23835) // 7.2.0.23835
                    _cache.ExpansionID = buffer.ReadUInt32();

                if (build >= 29683) // 8.1.5.29683
                    _cache.ManagedWorldStateID = buffer.ReadInt32();
                if (build >= 31921) // 8.2.5.31921
                    _cache.QuestSessionBonus = buffer.ReadUInt32();

                if (build >= 46293) // 10.0.0.46293
                {
                    _cache.QuestGiverCreatureID = buffer.ReadUInt32();
                    _cache.NumConditionalQuestDescription = buffer.ReadUInt32();
                    _cache.NumConditionalQuestCompletionLog = buffer.ReadUInt32();
                }

                if (build >= 36216) // < 9.0.1.36216
                {
                    for (int i = 0; i < _cache.RewardDisplaySpellCount; ++i)
                    {
                        if (build >= 49407) // >= 10.1.0.49407
                            _cache.RewardDisplay.Add(new QuestRewardSpell { SpellID = buffer.ReadUInt32(), PlayerConditionID = buffer.ReadUInt32(), DisplayType = buffer.ReadUInt32() });
                        else
                            _cache.RewardDisplay.Add(new QuestRewardSpell { SpellID = buffer.ReadUInt32(), PlayerConditionID = buffer.ReadUInt32() });
                    }
                }

                ReadQuestStringLengths(_cache, buffer, build);
            }

            if (build >= 46293) // 10.0.0.46293
                _cache.ReadyForTranslation = buffer.ReadBit();

            for (uint i = 0; i < _cache.NumObjectives; ++i)
            {
                QuestObjective objective = new QuestObjective();
                objective.ID = buffer.ReadUInt32();
                objective.Type = buffer.ReadByte();
                objective.StorageIndex = buffer.ReadSByte();
                objective.ObjectID = buffer.ReadInt32();
                objective.Amount = buffer.ReadInt32();
                objective.Flags = buffer.ReadUInt32();
                if (build >= 22900) //  >= 7.1.0.22900
                    objective.Flags2 = buffer.ReadUInt32();
                objective.PercentAmount = buffer.ReadSingle();
                objective.NumVisualEffects = buffer.ReadUInt32();
                for (int j = 0; j < objective.NumVisualEffects; ++j)
                    objective.VisualEffect.Add(buffer.ReadUInt32());

                UInt32 DescriptionLen = buffer.ReadBits(8);
                objective.Description = buffer.ReadString(DescriptionLen);
                _cache.Objective.Add(objective);
            }

            if (build < 22248)
                ReadQuestStringLengths(_cache, buffer, build);

            _cache.LogTitle = buffer.ReadString(_cache.LogTitleLen);
            _cache.LogDescription = buffer.ReadString(_cache.LogDescriptionLen);
            _cache.QuestDescription = buffer.ReadString(_cache.QuestDescriptionLen);
            _cache.AreaDescription = buffer.ReadString(_cache.AreaDescriptionLen);
            _cache.PortraitGiverText = buffer.ReadString(_cache.PortraitGiverTextLen);
            _cache.PortraitGiverName = buffer.ReadString(_cache.PortraitGiverNameLen);
            _cache.PortraitTurnInText = buffer.ReadString(_cache.PortraitTurnInTextLen);
            _cache.PortraitTurnInName = buffer.ReadString(_cache.PortraitTurnInNameLen);
            _cache.QuestCompletionLog = buffer.ReadString(_cache.QuestCompletionLogLen);


            if (build >= 46293) // 10.0.0.46293
            {
                for (int i = 0; i < _cache.NumConditionalQuestDescription; i++)
                {
                    QuestConditionalQuestText _condText = new QuestConditionalQuestText();
                    _condText.PlayerConditionID = buffer.ReadUInt32();
                    _condText.QuestGiverCreatureID = buffer.ReadUInt32();
                    _condText.TextLen = buffer.ReadBits(12);
                    _condText.Text = buffer.ReadString(_condText.TextLen);
                    _cache.ConditionalQuestDescription.Add(_condText);
                }

                for (int i = 0; i < _cache.NumConditionalQuestCompletionLog; i++)
                {
                    QuestConditionalQuestText _condText = new QuestConditionalQuestText();
                    _condText.PlayerConditionID = buffer.ReadUInt32();
                    _condText.QuestGiverCreatureID = buffer.ReadUInt32();
                    _condText.TextLen = buffer.ReadBits(12);
                    _condText.Text = buffer.ReadString(_condText.TextLen);
                    _cache.ConditionalQuestCompletionLog.Add(_condText);
                }
            }

            return _cache;
        }

        public static void ReadQuestStringLengths(QuestCache _cache, ByteBuffer buffer, UInt32 build)
        {
            if (build >= 28724 && build < 29683) // >= 8.1.0.2874 && < 8.1.5.29683
            {
                ReadQuestStringLengths810(_cache, buffer);
                return;
            }
            _cache.LogTitleLen = buffer.ReadBits(9);
            _cache.LogDescriptionLen = buffer.ReadBits(12);
            _cache.QuestDescriptionLen = buffer.ReadBits(12);
            _cache.AreaDescriptionLen = buffer.ReadBits(9);
            _cache.PortraitGiverTextLen = buffer.ReadBits(10);
            _cache.PortraitGiverNameLen = buffer.ReadBits(8);
            _cache.PortraitTurnInTextLen = buffer.ReadBits(10);
            _cache.PortraitTurnInNameLen = buffer.ReadBits(8);
            _cache.QuestCompletionLogLen = buffer.ReadBits(11);
        }
        public static void ReadQuestStringLengths810(QuestCache _cache, ByteBuffer buffer)
        {
            _cache.LogTitleLen = buffer.ReadBits(10);
            _cache.LogDescriptionLen = buffer.ReadBits(12);
            _cache.QuestDescriptionLen = buffer.ReadBits(12);
            _cache.AreaDescriptionLen = buffer.ReadBits(9);
            _cache.PortraitGiverTextLen = buffer.ReadBits(11);
            _cache.PortraitGiverNameLen = buffer.ReadBits(9);
            _cache.PortraitTurnInTextLen = buffer.ReadBits(11);
            _cache.PortraitTurnInNameLen = buffer.ReadBits(9);
            _cache.QuestCompletionLogLen = buffer.ReadBits(12);
        }
    }
}
