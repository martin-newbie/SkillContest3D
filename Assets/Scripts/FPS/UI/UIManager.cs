using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Objects")]
        [SerializeField] List<Image> WeaponImages = new List<Image>();
        [SerializeField] Image HPGauge;
        [SerializeField] Image PainGauge;
        public GameClear gameClear;
        [SerializeField] Image Bleeding;

        [Header("Missile")]
        [SerializeField] Text CurMissile;
        [SerializeField] Text LoadedMissile;
        [SerializeField] Text MaxMissile;
        [SerializeField] Image missileLoad;

        [Header("Homing")]
        [SerializeField] Text CurHoming;
        [SerializeField] Text LoadedHoming;
        [SerializeField] Text MaxHoming;
        [SerializeField] Image homingLoad;

        void Start()
        {

        }

        void Update()
        {
            HPGauge.fillAmount = GameManager.Instance.PlayerHp / 100f;
            PainGauge.fillAmount = GameManager.Instance.PainGauge / 100f;
            Bleeding.color = new Color(1, 1, 1, 1 - (GameManager.Instance.PlayerHp / 100f));
        }

        public void GameClearUI(float score, float hp, float pg)
        {
            gameClear.gameObject.SetActive(true);
            gameClear.ResultPrint(score, hp, pg);
        }

        public void GetHomingInformation(int curCount, int loaded, int max)
        {
            CurHoming.text = curCount.ToString();
            LoadedHoming.text = loaded.ToString();
            MaxHoming.text = max.ToString();
        }

        public void GetHomingLoad(float cur, float max)
        {
            homingLoad.fillAmount = cur / max;
        }

        public void GetCurWeapon(int index)
        {
            WeaponImages.ForEach(item => item.color = new Color(1, 1, 1, 0.2f));
            WeaponImages[index].color = new Color(1, 1, 1, 0.5f);
        }

        public void GetMissileLoad(float cur, float max)
        {
            missileLoad.fillAmount = cur / max;
        }

        public void GetMissileInformation(int curCount, int loaded, int max)
        {
            CurMissile.text = curCount.ToString();
            LoadedMissile.text = loaded.ToString();
            MaxMissile.text = max.ToString();
        }
    }

}
