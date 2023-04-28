namespace KazatanGames.Framework
{
    using UnityEngine;

    public abstract class AppConfigSO : ScriptableObject
    {
        public int version = 1;
        public string versionCode = "0.0.1";
        public string releaseBranch = "dev";
        public Utilities.SceneField initialScene;
        public bool autoLoadInitialScene = true;
        public bool debugMode = true;
        public bool skipIntros = false;
    }
}