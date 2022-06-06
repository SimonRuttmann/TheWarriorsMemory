namespace Scripts.UI
{
    public interface IMenu
    {
        void NewGame();
        void OpenMainMenu();
        void OpenIngameMenu();
        void ContinueGame();
        void SaveGame();
        void BackToTitleScreen();
        void CloseGame();
        void LoadGame();
        void RestartGame();
    }
}