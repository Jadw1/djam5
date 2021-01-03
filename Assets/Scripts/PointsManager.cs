using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PointsManager : MonoBehaviour
    {
        private static PointsManager _instance;
        public static PointsManager Instance => _instance;

        private int _points = 0;
        public int Points => _points;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void OnDestroy()
        {
            if (this == _instance)
            {
                _instance = null;
            }
        }

        public void AddPoint()
        {
            _points++;
        }
    }
}