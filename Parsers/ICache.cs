using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWTools.WDBUpdater.Parsers
{
    public interface ICache<T>
    {
        public uint GetID();
        public string GetName();
        public static abstract WDBFIles GetCacheType();
        //public String GetJSON();
        public static abstract T GetData(UInt32 entry);
        public static abstract List<UInt32> GetEntries();
        public static abstract bool Parse(WDBReader reader);
        public static abstract T ReadEntry(ByteBuffer buffer, UInt32 build);
    }
}
