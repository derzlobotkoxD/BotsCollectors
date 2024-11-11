using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] ScoreCounter _scoreCounter;
    [SerializeField] TMP_Text _scorePrefab;
    [SerializeField] Transform _parentForText;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _rotate;

    private TMP_Text _score;

    private void Awake() =>
        _score = Instantiate(_scorePrefab, transform.position + _offset, Quaternion.Euler(_rotate), _parentForText);

    private void OnEnable() =>
        _scoreCounter.ScoreChanged += Change;

    private void OnDisable() =>
        _scoreCounter.ScoreChanged -= Change;

    private void Change(int value) =>
        _score.text = value.ToString();
}