using UnityEngine;
using TMPro;

namespace Platformer.Mechanics
{
    public class Timer : MonoBehaviour
    {
        public bool isDone = false;
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI highscoreTimerText;

        private float timeElapsed = 0f;
        public float HighScore => PlayerPrefs.GetFloat("HighScore");

        void Start()
        {
            highscoreTimerText.text = FormatTime(HighScore);
        }

        void Update()
        {
            // If the timer is not done, increment the time elapsed
            if (!isDone)
            {
                timeElapsed += Time.deltaTime;

                // Update the timer display
                UpdateTimerDisplay();
            }
            else
            {
                SaveHighScore();
            }

        }

        // Method to format the time in HH:mm:ss format
        private void UpdateTimerDisplay()
        {
            if (timerText != null)
            {
                timerText.text = FormatTime(timeElapsed);
                ColorizeText();
            }
        }

        string FormatTime(float timeElapsed)
        {
            int hours = (int)(timeElapsed / 3600f);
            int minutes = (int)(timeElapsed % 3600 / 60f);
            int seconds = (int)(timeElapsed % 60f);

            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }

        #region HighScore logic
        public void SaveHighScore()
        {
            float currentHighScore = HighScore;

            if (timeElapsed > currentHighScore)
            {
                PlayerPrefs.SetFloat("HighScore", timeElapsed);
                PlayerPrefs.Save();
            }
        }

        void ColorizeText()
        {
            // Make the text red, if its greater than the curr. highscore
            // Make the text white if its less than or highscore is 0
            timerText.color = HighScore == 0 || timeElapsed < HighScore ? Color.white : Color.red;
        }

        // Method to reset the high score (called from a context menu in the Unity editor)
        public void ResetHighScore()
        {
            PlayerPrefs.DeleteKey("HighScore");
            highscoreTimerText.text = FormatTime(0f); // Update UI to show 00:00:00
        }
        #endregion
    }
}