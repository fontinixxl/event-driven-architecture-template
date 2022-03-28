using System.Collections;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
	[SerializeField] private UIPopup _popupPanel = default;
	[SerializeField] private UIMainMenu _mainMenuPanel = default;
	[SerializeField] private SaveSystem _saveSystem = default;
	[SerializeField] private InputReader _inputReader = default;
	
	[Header("Broadcasting on")]
	[SerializeField]
	private VoidEventChannelSO _startNewGameEvent = default;
	[SerializeField]
	private VoidEventChannelSO _continueGameEvent = default;

	private bool _hasSaveData;

	private IEnumerator Start()
	{
		_inputReader.EnableMenuInput();
		yield return new WaitForSeconds(0.4f); //waiting time for all scenes to be loaded 
		SetMenuScreen();
	}
	void SetMenuScreen()
	{
		_hasSaveData = _saveSystem.LoadSaveDataFromDisk();
		_mainMenuPanel.SetMenuScreen(_hasSaveData);
		_mainMenuPanel.ContinueButtonAction += _continueGameEvent.RaiseEvent;
		_mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
		_mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
		_mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;

	}

	void ButtonStartNewGameClicked()
	{
		if (!_hasSaveData)
		{
			ConfirmStartNewGame();

		}
		else
		{
			ShowStartNewGameConfirmationPopup();
		}
	}

	void ConfirmStartNewGame()
	{
		_startNewGameEvent.RaiseEvent();
	}

	void ShowStartNewGameConfirmationPopup()
	{
		_popupPanel.ConfirmationResponseAction += StartNewGamePopupResponse;
		_popupPanel.ClosePopupAction += HidePopup;

		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.NewGame);

	}

	void StartNewGamePopupResponse(bool startNewGameConfirmed)
	{

		_popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
		_popupPanel.ClosePopupAction -= HidePopup;

		_popupPanel.gameObject.SetActive(false);

		if (startNewGameConfirmed)
		{
			ConfirmStartNewGame();
		}
		else
		{
			_continueGameEvent.RaiseEvent();
		}

		_mainMenuPanel.SetMenuScreen(_hasSaveData);

	}

	void HidePopup()
	{
		_popupPanel.ClosePopupAction -= HidePopup;
		_popupPanel.gameObject.SetActive(false);
		_mainMenuPanel.SetMenuScreen(_hasSaveData);

	}

	public void OpenSettingsScreen()
	{

	}
	public void CloseSettingsScreen()
	{
		
	}

	public void ShowExitConfirmationPopup()
	{
		_popupPanel.ConfirmationResponseAction += HideExitConfirmationPopup;
		_popupPanel.gameObject.SetActive(true);
		_popupPanel.SetPopup(PopupType.Quit);
	}
	
	void HideExitConfirmationPopup(bool quitConfirmed)
	{
		_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
		_popupPanel.gameObject.SetActive(false);
		if (quitConfirmed)
		{
			Application.Quit();
		}
		_mainMenuPanel.SetMenuScreen(_hasSaveData);
		
	}
	private void OnDestroy()
	{
		_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
		_popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
	}
}
