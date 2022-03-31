using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] Image BG;
    Animator anim;

    void Start()
    {
        Cursor.visible = true;
        anim = GetComponent<Animator>();
        anim.SetTrigger("Trigger_1");
        BG.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void GameStart()
    {
        anim.SetTrigger("Trigger_2");
    }

    IEnumerator FadeOut()
    {
        BG.gameObject.SetActive(true);
        float duration = 2f;
        Color color = BG.color;
        while (BG.color.a < 1)
        {
            color.a += duration * Time.deltaTime;
            BG.color = color;
            yield return null;
        }

        SceneManager.LoadScene(2);
    }

    public void SceneMove()
    {
        StartCoroutine(FadeOut());
    }
}
