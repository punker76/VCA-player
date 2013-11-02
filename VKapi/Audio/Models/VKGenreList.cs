using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKapi.Audio.Models
{
    public static class VKGenre
    {
        private static Dictionary<byte, string> _list = new Dictionary<byte, string>()
        {
            { 1,  "Rock" },
            { 2,  "Pop"},
            { 3,  "Rap & Hip-Hop"},
            { 4,  "Easy Listening"},
            { 5,  "Dance & House"},
            { 6,  "Instrumental"},
            { 7,  "Metal"},
            { 8,  "Dubstep"},
            { 9,  "Jazz & Blues"},
            { 10, "Drum & Bass"},
            { 11, "Trance"},
            { 12, "Chanson"},
            { 13, "Ethnic"},
            { 14, "Acoustic & Vocal"},
            { 15, "Reggae"},
            { 16, "Classical"},
            { 17, "Indie Pop"},
            { 18, "Other"},
            { 19, "Speech"},
            { 21, "Alternative"},
            { 22, "Electropop & Disco"},
        };
        private static ReadOnlyDictionary<byte, string> _readOnlyList = new ReadOnlyDictionary<byte, string>(_list);

        public static ReadOnlyDictionary<byte, string> List { get { return _readOnlyList; } }
    }
}
