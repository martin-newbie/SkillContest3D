using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FPS
{
    public class GameClear : MonoBehaviour
    {

        [Header("UI objects")]
        public Image Background;
        public Text KillScore;
        public Text HpScore;
        public Text PgScore; //pain gauge
        public Text TotalScore;
        public Button Next;

        void Start()
        {
            Next.gameObject.SetActive(false);
        }

        void Update()
        {

        }

        public void NextStage()
        {
            if (GameManager.Instance.Stage == 1)
            {
                StatusManager.Instance.stage = 1;
                SceneManager.LoadScene(2);
            }
            else SceneManager.LoadScene(1);

        }

        public void ResultPrint(float score, float hp, float pg)
        {
            StartCoroutine(ResultPrintCoroutine(score, hp, pg));
        }

        IEnumerator ResultPrintCoroutine(float score, float hp, float pg)
        {
            yield return new WaitForSeconds(1f);
            Background.gameObject.SetActive(true);
            yield return StartCoroutine(TextCounting(KillScore, 0, score));
            yield return StartCoroutine(TextCounting(HpScore, 0, hp));
            yield return StartCoroutine(TextCounting(PgScore, 0, pg));
            yield return StartCoroutine(TextCounting(TotalScore, 0, score + hp + pg));
            Cursor.visible = true;
            Next.gameObject.SetActive(true);
        }

        IEnumerator TextCounting(Text text, float current, float target)
        {
            float duration = 0.5f;
            float offset = (target - current) / duration;

            while (current < target)
            {
                current += offset * Time.deltaTime;
                text.text = current.ToString();
                yield return null;
            }

            current = target;
            text.text = target.ToString();

            yield return new WaitForSeconds(1f);


        }
    }

}
