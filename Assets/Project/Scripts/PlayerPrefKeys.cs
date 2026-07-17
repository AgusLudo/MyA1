using JetBrains.Annotations;

namespace Project.Scripts
{
    /// <summary>
    /// Las keys para ver los valores de <see cref="UnityEngine.PlayerPrefs"/>.
    /// </summary>
    internal static class PlayerPrefKeys
    {
        

        [NotNull]
        public static readonly string SoundVolume = "sfxVolume";

        [NotNull]
        public static readonly string MusicVolume = "musicVolume";

        [NotNull]
        public static readonly string Lives = "lives";

        [NotNull]
        public static readonly string Score = "score";

        [NotNull]
        public static readonly string LastScore = "lastScore";

        [NotNull]
        public static readonly string Highscore = "highscore";
    }
}
