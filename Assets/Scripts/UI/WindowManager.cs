using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private Button increaseResolution;
        [SerializeField] private Button decreaseResolution;
        [SerializeField] private Button fullscreenWindow;
        [SerializeField] private Button quitButton;

        private bool fullscreen;
        private Resolution[] resolutions;
        private int counter = 0;
        private void Awake()
        {
            resolutions = Screen.resolutions;
            increaseResolution.onClick.AddListener(Increase);
            decreaseResolution.onClick.AddListener(Decrease);
            fullscreenWindow.onClick.AddListener(() => { fullscreen = !fullscreen; Screen.fullScreen = fullscreen; });
            quitButton.onClick.AddListener(() => Application.Quit());

            Cursor.lockState = CursorLockMode.Locked;
        }

        void Increase()
        {
            counter++;
            if (counter > resolutions.Length - 1) counter = 0;
            Screen.SetResolution(resolutions[counter].width, resolutions[counter].height, fullscreen);
        }
        void Decrease()
        {
            counter--;
            if (counter < 0) counter = resolutions.Length - 1;
            Screen.SetResolution(resolutions[counter].width, resolutions[counter].height, fullscreen);
        }

        float lastEscape;
        private void Update()
        {
            if (Time.time - lastEscape > 1.0f && Input.GetKey(KeyCode.Escape))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.None;
                else
                    Cursor.lockState = CursorLockMode.Locked;

                lastEscape = Time.time;
            }
        }
    }
}
