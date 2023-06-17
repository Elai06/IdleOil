using UnityEngine;

public class TargetFrameRate : MonoBehaviour
{
    [SerializeField] private int _targetFrameRate = 60;

    void Awake()
    {
        if (Application.targetFrameRate != _targetFrameRate)
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}