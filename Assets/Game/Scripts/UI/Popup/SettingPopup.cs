public class SettingPopup : BasePopup
{

   public void OnCloseButton()
   {
      GameManager.Instance.isInGameRunning = true;
      gameObject.SetActive(false);
   }

   public void OnGoMainMenuButton()
   {
      UIManager.Instance.currentScreen.ChangeScreen(Screen.MainScreen);
      UIManager.Instance.ShowWaitBg(); 
      gameObject.SetActive(false);
   }
}
