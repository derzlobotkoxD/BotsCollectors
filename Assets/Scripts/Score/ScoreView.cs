using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private TMP_Text _scorePrefab;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _rotate;

    private TMP_Text _score;
    private RectTransform _positionScore;

    private void OnEnable() =>
        _scoreCounter.ScoreChanged += Change;

    private void OnDisable() =>
        _scoreCounter.ScoreChanged -= Change;

    public void StartShow() =>
        _score = Instantiate(_scorePrefab, transform.position + _offset, Quaternion.Euler(_rotate), _positionScore);

    public void SetParentForText(RectTransform position) =>
        _positionScore = position;

    private void Change(int value) =>
        _score.text = value.ToString();
}