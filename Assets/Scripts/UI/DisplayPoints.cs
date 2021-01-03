using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DisplayPoints : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private int _lastPoints = 0;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void FixedUpdate()
        {
            var points = PointsManager.Instance.Points;
            
            if (points != _lastPoints)
            {
                _lastPoints = points;
                _text.SetText(points.ToString());
            }
        }
    }
}