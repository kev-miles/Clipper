namespace Host.Infrastructure.FileManagement
{
    public static class SaveDataKeys
    {
        public static readonly string UserName = "UserName";
        public static readonly string SlotId = "SaveSlotId";

        public static readonly string CurrentLevel = "CurrentLevel";
        public static readonly string LevelScores = "LevelScores";

        public static readonly string[] AllKeys = new[]
        {
            UserName,
            SlotId,
            CurrentLevel,
            LevelScores
        };
    }
}