using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    TheLab _inputs; // Section 001
    // PlayerControl _inputs; // Section 401. 
    [SerializeField] private Button _turnCameraLeft;
    [SerializeField] private Button _turnCameraRight;
    [SerializeField] private int _index = 0;
    [SerializeField] private CinemachineVirtualCamera _currentCamera;
    [SerializeField]
    private List<CinemachineVirtualCamera> _virtualCameras =
            new List<CinemachineVirtualCamera>();

    void Awake()
    {
        InitCameraPriorities();
        _inputs = new TheLab(); // Section 001;
        // _inputs = new PlayerControl(); // Section 401;
        _inputs.Player.Camera.performed += context => MoveCamera(context.ReadValue<float>());
        _turnCameraLeft.onClick.AddListener(() => MoveCamera(-1));
        _turnCameraRight.onClick.AddListener(() => MoveCamera(1));
    }

    void InitCameraPriorities()
    {
        foreach (var vCamera in _virtualCameras)
        {
            vCamera.Priority = 0;
        }
        _currentCamera = _virtualCameras[0];
        _currentCamera.Priority = 10;
    }

    void OnEnable() => _inputs.Enable();
    void OnDisable() => _inputs.Disable();

    void MoveCamera(float value)
    {
        Debug.Log($"Camera change value {value}");
        _index += (int)value;
        if (_index < 0) _index = _virtualCameras.Count - 1;
        if (_index > _virtualCameras.Count - 1) _index = 0;
        ChangeCamera();
    }

    void ChangeCamera()
    {
        _currentCamera.Priority = 0;
        _currentCamera = _virtualCameras[_index];
        _currentCamera.Priority = 10;
    }
}