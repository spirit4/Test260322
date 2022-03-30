using UnityEngine;

namespace Assets.Scripts
{
    /** <summary>Since we have very a few data and calls I use PlayerPrefs between scenes and for local save</summary> */
    public static class Saver
    {
        public static void Save(int level, int score)
        {
            PlayerPrefs.SetInt("Level", level);
            PlayerPrefs.SetInt("Score", score);

            int maxScore = PlayerPrefs.GetInt("MaxScore", 0);
            maxScore = score > maxScore ? score : maxScore;
            PlayerPrefs.SetInt("MaxScore", maxScore);

            //Debug.Log($"Save level, score {level} {score}");
        }

        public static void Load(out int level, out int score, out int maxScore)
        {
            //PlayerPrefs.DeleteAll();
            level = PlayerPrefs.GetInt("Level", 1);
            score = PlayerPrefs.GetInt("Score", 0);
            maxScore = PlayerPrefs.GetInt("MaxScore", 0);
           
            //Debug.Log($"Load level, score {level} {score}");
        }
    }
}
