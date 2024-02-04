using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWTools.WDBUpdater.Parsers
{
    public class PageTextCache : ICache<PageTextCache>
    {
        public UInt32 PageID { get; set; }
        public UInt32 NextPageID { get; set; }
        public UInt32 PageInfo { get; set; }
        public Byte Flags { get; set; }
        public UInt32 TextLen { get; set; }
        public String Text { get; set; } = "";

        public uint GetID()
        {
            return PageID;
        }
        public string GetName()
        {
            return "";
        }
        public static WDBFIles GetCacheType()
        {
            return WDBFIles.PageTextCache;
        }

        public static Dictionary<UInt32, PageTextCache> Entries = new Dictionary<UInt32, PageTextCache>();
        public static PageTextCache GetData(UInt32 entry)
        {
            return Entries[entry];
        }
        public static List<UInt32> GetEntries()
        {
            return Entries.Keys.ToList();
        }
        public static bool Parse(WDBReader reader)
        {
            if (reader.Signature != WDBFIles.PageTextCache)
                return false;

            Dictionary<UInt32, PageTextCache> entries = new Dictionary<uint, PageTextCache>();
            foreach (var element in reader.dataTable)
            {
                ByteBuffer _buffer = new ByteBuffer(element.Value);

                if (reader.Build >= 22248) // >= 7.0.3.22248
                {
                    UInt32 pageCount = _buffer.ReadUInt32();
                    for (int i = 0; i < pageCount; i++)
                    {
                        PageTextCache entry = ReadEntry(_buffer, reader.Build);
                        Entries.Add(entry.PageID, entry);
                    }
                }
                else
                {
                    PageTextCache entry = ReadEntry(_buffer, reader.Build);
                    Entries.Add(entry.PageID, entry);
                }
                Debug.Assert(_buffer.Rpos == _buffer.Size());
            }
            return true;
        }

        public static PageTextCache ReadEntry(ByteBuffer buffer, UInt32 build)
        {
            PageTextCache _cache = new PageTextCache();
            _cache.PageID = buffer.ReadUInt32();
            _cache.NextPageID = buffer.ReadUInt32();
            if (build >= 22248) // >= 7.0.3.22248
            {
                _cache.PageInfo = buffer.ReadUInt32();
                _cache.Flags = buffer.ReadByte();
            }
            _cache.TextLen = buffer.ReadBits(12);
            _cache.Text = buffer.ReadString(_cache.TextLen);
            return _cache;
        }
    }
}
