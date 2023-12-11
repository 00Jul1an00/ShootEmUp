using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HelpersButtons : MonoBehaviour
{
    [SerializeField] private Button _start;
    [SerializeField] private Button _pause;
    [SerializeField] private Button _resume;

    public void DeactivateStartButton()
    {
        _start.gameObject.SetActive(false);
    }

    public void AddStartButtonListiner(UnityAction action)
    {
        _start.onClick.AddListener(action);
    }

    public void AddPauseButtonListiner(UnityAction action)
    {
        _pause.onClick.AddListener(action);
    }

    public void AddResumeButtonListiner(UnityAction action)
    {
        _resume.onClick.AddListener(action);
    }

    private void OnDestroy()
    {
        _start.onClick.RemoveAllListeners();
        _pause.onClick.RemoveAllListeners();
        _resume.onClick.RemoveAllListeners();
    }
}
