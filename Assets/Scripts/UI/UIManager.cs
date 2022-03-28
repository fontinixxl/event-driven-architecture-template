using UnityEngine;

public class UIManager : MonoBehaviour
{
	[Header("Scene UI")]
	[SerializeField] private MenuSelectionHandler _selectionHandler = default;
	[SerializeField] private UIPopup _popupPanel = default;
	[SerializeField] private UIPause _pauseScreen = default;

	[Header("Gameplay")]
	[SerializeField] private MenuSO _mainMenu = default;
	[SerializeField] private InputReader _inputReader = default;

	[Header("Listening on")]
	[SerializeField] private VoidEventChannelSO _onSceneReady = default;

	[Header("Broadcasting on ")]
	[SerializeField] private LoadEventChannelSO _loadMenuEvent = default;

	private void OnEnable()
	{
		_onSceneReady.OnEventRaised += ResetUI;
		_inputReader.MenuPauseEvent += OpenUIPause; // subscription to open Pause UI event happens in OnEnabled, but the close Event is only subscribed to when the popup is open
	}

	private void OnDisable()
	{
		_onSceneReady.OnEventRaised -= ResetUI;
		_inputReader.MenuPauseEvent -= OpenUIPause;
	}

	void ResetUI()
	{
		_pauseScreen.gameObject.SetActive(false);

		Time.timeScale = 1;
	}

	void OpenUIPause()
	{
		_inputReader.MenuPauseEvent -= OpenUIPause; // you can open UI pause menu again, if it's closed

		Time.timeScale = 0; // Pause time

		_pauseScreen.BackToMainRequested += ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
		_pauseScreen.Resumed += CloseUIPause;//once the UI Pause popup is open, listen to unpause event

		_pauseScreen.gameObject.SetActive(true);

		_inputReader.EnableMenuInput();
		
		// TODO: If GameStateManager; Update State to Pause 
	}

	void CloseUIPause()
	{
		Time.timeScale = 1; // unpause time

		_inputReader.MenuPauseEvent += OpenUIPause; // you can open UI pause menu again, if it's closed

		// once the popup is closed, you can't listen to the following events 
		_pauseScreen.BackToMainRequested -= ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
		_pauseScreen.Resumed -= CloseUIPause;//once the UI Pause popup is open, listen to unpause event

		_pauseScreen.gameObject.SetActive(false);

		_inputReader.EnableGameplayInput();
		
		_selectionHandler.Unselect();
	}

	void ShowBackToMenuConfirmationPopup()
	{
		_pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive

		_popupPanel.ClosePopupAction += HideBackToMenuConfirmationPopup;

		_popupPanel.ConfirmationResponseAction += BackToMainMenu;

		_inputReader.EnableMenuInput();
		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.BackToMenu);
	}

	void BackToMainMenu(bool confirm)
	{
		HideBackToMenuConfirmationPopup();// hide confirmation screen, show close UI pause, 

		if (confirm)
		{
			CloseUIPause();//close ui pause to unsub from all events 
			_loadMenuEvent.RaiseEvent(_mainMenu, false); //load main menu
		}
	}
	
	void HideBackToMenuConfirmationPopup()
	{
		_popupPanel.ClosePopupAction -= HideBackToMenuConfirmationPopup;
		_popupPanel.ConfirmationResponseAction -= BackToMainMenu;

		_popupPanel.gameObject.SetActive(false);
		_selectionHandler.Unselect();
		_pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive

		// time is still set to 0 and Input is still set to menuInput 
		//going out from confirmaiton popup screen gets us back to the pause screen
	}
}
