namespace Scripts.UI
{
    public interface IMenu
    {
        void NewGame();
        void OpenMainMenu();
        void OpenIngameMenu();
        void ContinueGame();
        void CloseGame();
        void RestartGame();
        void OpenEndScreen();
    }
}