namespace Host.Infrastructure.Interfaces
{
    public interface INavigationService
    {
        public IApplicationView GetCurrentActiveView();
        public void OpenView(IApplicationView view);
        public void CloseView();
        public void LoadMainMenu();
        public void ExitGameplayScene();
        public void LoadGameplayScene();
        public void ExitGame();
    }
}