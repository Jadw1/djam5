using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class DisplayPoints : MonoBehaviour
    {
        private Text _text;
        private int _lastPoints = 0;

        private void Start()
        {
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            var points = PointsManager.Instance.Points;
            
            if (points != _lastPoints)
            {
                _lastPoints = points;
                _text.text = points.ToString();
            }
        }
    }
}