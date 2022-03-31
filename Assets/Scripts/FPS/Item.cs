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
                    message = "�̻��� ź�� 15�� ȹ��";
                    break;
                case ItemType.HomingAmmo:
                    player.curHoming += 15;
                    message = "����ź ź�� 15�� ȹ��";
                    break;
                case ItemType.HP_Increase:
                    player.Hp += 10f;
                    message = "ü�� 10 ȸ��";
                    break;
                case ItemType.Pain_Decrease:
                    GameManager.Instance.PainGauge -= 10f;
                    message = "���� 10 ����";
                    break;
                case ItemType.WeaponUpgrade:
                    int rand = Random.Range(0, 3);

                    switch (rand)
                    {
                        case 0:
                            player.GunDamageLevel++;
                            message = "�Ѿ� ������ ���׷��̵�";
                            break;
                        case 1:
                            player.GunSpeedLevel++;
                            message = "�Ѿ� ������ ����";
                            break;
                        case 2:
                            player.GunSpreadLevel++;
                            message = "�Ѿ� ź���� ����";
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
