namespace Host.Infrastructure.ApplicationEvents
{
    public static class ApplicationEventNames
    {
        public static readonly string INIT_DONE = "InitializationDone";
        public static readonly string PRESS_START = "PressedStart";
        public static readonly string LOAD_MENU = "LoadMenu";
        public static readonly string LOAD_GAMEPLAY = "LoadGameplay";
        public static readonly string EXIT_GAMEPLAY = "ExitGameplay";
        public static readonly string SETTINGS_UPDATED = "SettingsUpdated";
        public static readonly string LANGUAGE_CHANGED = "LanguageChanged";
        public static readonly string AUDIO_SETTINGS_CHANGED = "AudioSettingsChanged";
        public static string GAME_SAVED = "GameSaved";
        public static string SAVED_GAME_DELETED = "SavedGameDeleted";
    }
}