using VkontakteAPI.Converters;

namespace VKapi.Models
{
    public enum VKGenreEnum : int
    {
        [VKDescription("Все жанры")]
        All = 0,
        [VKDescription("Rock")]
        Rock = 1,
        [VKDescription("Pop")]
        Pop = 2,
        [VKDescription("Rap & Hip-Hop")]
        RapAndHipHop = 3,
        [VKDescription("Easy Listening")]
        EasyListening = 4,
        [VKDescription("House & Dance")]
        DanceAndHouse = 5,
        [VKDescription("Instrumental")]
        Instrumental = 6,
        [VKDescription("Metal")]
        Metal = 7,
        [VKDescription("Dubstep")]
        Dubstep = 8,
        [VKDescription("Jazz & Blues")]
        JazzAndBlues = 9,
        [VKDescription("Drum & Bass")]
        DrumAndBass = 10,
        [VKDescription("Trance")]
        Trance = 11,
        [VKDescription("Chanson")]
        Chanson = 12,
        [VKDescription("Ethnic")]
        Ethnic = 13,
        [VKDescription("Acoustic & Vocal")]
        AcousticAndVocal = 14,
        [VKDescription("Reggae")]
        Reggae = 15,
        [VKDescription("Classical")]
        Classical = 16,
        [VKDescription("Indie Pop")]
        IndiePop = 17,
        [VKDescription("Other")]
        Other = 18,
        [VKDescription("Speech")]
        Speech = 19,
        [VKDescription("Alternative")]
        Alternative = 21,
        [VKDescription("Electropop & Disco")]
        ElectropopAndDisco = 22
    }
}