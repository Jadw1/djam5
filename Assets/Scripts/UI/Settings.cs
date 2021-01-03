using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Settings : MonoBehaviour
    {
        public Button mute;
        public Button close;
        private bool _muted = false;
    
        void Start()
        {
            mute.onClick.AddListener(Mute);
            close.onClick.AddListener(Close);
        }

        private void Mute()
        {
            if (_muted)
            {
                mute.GetComponent<Image>().color = Color.green;
                mute.GetComponentInChildren<Text>().text = "ON";
                _muted = false;
                AudioListener.pause = _muted;
            }
            else
            {
                mute.GetComponent<Image>().color = Color.red;
                mute.GetComponentInChildren<Text>().text = "OFF";
                _muted = true;
                AudioListener.pause = _muted;
            }
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
