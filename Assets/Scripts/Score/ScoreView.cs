using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private TMP_Text _scorePrefab;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _rotate;

    private TMP_Text _score;

    private void OnEnable() =>
        _scoreCounter.Changed += OnChange;

    private void OnDisable() =>
        _scoreCounter.Changed -= OnChange;

    public void CreateText(RectTransform parent)
    {
        Vector3 position = transform.position + _offset;
        _score = Instantiate(_scorePrefab, position, Quaternion.Euler(_rotate), parent);
    }

    private void OnChange(int value) =>
        _score.text = value.ToString();
}