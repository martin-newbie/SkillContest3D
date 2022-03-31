using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{

    public enum ItemType
    {
        MissileAmmo,
        HomingAmmo,
        HP_Increase,
        Pain_Decrease,
        WeaponUpgrade,
        BulletProof
    }

    public class Item : MonoBehaviour
    {
        Player player;
        ItemType type;

        void Start()
        {
            player = FindObjectOfType(typeof(Player)) as Player;
        }

        void Update()
        {

        }

        public void RandomItem()
        {
            type = (ItemType)Random.Range(0, (int)ItemType.BulletProof);
            string message = "";
            switch (type)
            {
                case ItemType.MissileAmmo:
                    player.curMissile += 15;
                    message = "¹Ì»çÀÏ Åº¾à 15°³ È¹µæ";
                    break;
                case ItemType.HomingAmmo:
                    player.curHoming += 15;
                    message = "À¯µµÅº Åº¾à 15°³ È¹µæ";
                    break;
                case ItemType.HP_Increase:
                    player.Hp += 10f;
                    message = "Ã¼·Â 10 È¸º¹";
                    break;
                case ItemType.Pain_Decrease:
                    GameManager.Instance.PainGauge -= 10f;
                    message = "°íÅë 10 °¨¼Ò";
                    break;
                case ItemType.WeaponUpgrade:
                    int rand = Random.Range(0, 3);

                    switch (rand)
                    {
                        case 0:
                            player.GunDamageLevel++;
                            message = "ÃÑ¾Ë µ¥¹ÌÁö ¾÷±×·¹ÀÌµå";
                            break;
                        case 1:
                            player.GunSpeedLevel++;
                            message = "ÃÑ¾Ë µô·¹ÀÌ °¨¼Ò";
                            break;
                        case 2:
                            player.GunSpreadLevel++;
                            message = "ÃÑ¾Ë ÅºÆÛÁü °¨¼Ò";
                            break;
                    }

                    break;
                case ItemType.BulletProof:
                    break;
                default:
                    break;
            }

            PrintMessage(message);
        }

        void PrintMessage(string message)
        {

        }
    }

}
