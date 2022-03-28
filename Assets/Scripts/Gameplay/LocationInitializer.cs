using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationInitializer : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField] private InputReader _inputReader = default;
    [SerializeField] private GameObject _testUI = default;
    
    [Header("Scene Ready Event")]
    [SerializeField] private VoidEventChannelSO _onSceneReady = default; //Raised by SceneLoader when the scene is set to active
    [SerializeField] private BoolEventChannelSO _onGamePaused = default; 
    
    private void OnEnable()
    {
        _onSceneReady.OnEventRaised += StartWhateverGameplay;
        _onGamePaused.OnEventRaised += ShowHideLocationUI;
    }

    private void OnDisable()
    {
        _onSceneReady.OnEventRaised -= StartWhateverGameplay;
        _onGamePaused.OnEventRaised -= ShowHideLocationUI;
    }

    private void StartWhateverGameplay()
    {
        _inputReader.EnableGameplayInput();
    }
    
    private void ShowHideLocationUI(bool open) => _testUI.gameObject.SetActive(!open);
}
