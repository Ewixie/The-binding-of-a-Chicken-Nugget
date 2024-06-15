
using Data;
using Managers;
using TMPro;
using UnityEngine;

public class StageTitle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private float fadeTime;
    private float _timer;
    private Color _startColor;
    private Color _endColor;
    void Start()
    {
        stageText.text = "Stage: " + ProgressManager.Instance.GetCurrentRunData().stage;
        _startColor = stageText.color;
        _endColor = _startColor;
        _endColor.a = 0;
    }

    void Update()
    {
        if (_timer <= fadeTime)
        {
            stageText.color = Color.Lerp(_startColor, _endColor, _timer / fadeTime);
            _timer += TimeManager.UnpausedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
