using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FPS
{

    public enum PlayerWeapon
    {
        MACHINEGUN,
        MISSILELAUNCER,
        HOMINGMISSILE
    }

    public class Player : MonoBehaviour
    {
        [SerializeField] PlayerWeapon weaponState;

        [SerializeField] Camera FrontCam;

        [SerializeField] GameObject AimPoint;

        [SerializeField] Canvas mainCanvas;
        [SerializeField] LayerMask HitLayer;
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] Transform Wall;
        [SerializeField] Transform Container;
        RectTransform canvasRect;
        Vector3 aimPointPos;

        [SerializeField] float rotateSpeed;
        float zRot, xRot;

        [Header("Status")]
        public float Hp;
        public int GunDamageLevel = 0;
        public int GunSpreadLevel = 0;
        public int GunSpeedLevel = 0;

        [Header("UI")]
        [SerializeField] UIManager UImanager;

        [Header("MachineGun")]
        [SerializeField] float machinegunDelay;
        [SerializeField] Projectile bullet;
        [SerializeField] Transform[] guns;

        [SerializeField] GameObject FrontGun_1;
        [SerializeField] GameObject FrontGun_2;

        [SerializeField] Transform frontPos1;
        [SerializeField] Transform frontPos2;
        [SerializeField] float spread;

        [SerializeField] ParticleSystem gunEffect_1;
        [SerializeField] ParticleSystem gunEffect_2;

        [Header("Missile")]
        [SerializeField] float missileDelay;
        [SerializeField] Transform missilePos;
        [SerializeField] Projectile Missile;
        [SerializeField] int maxMissile; // alltime mag maximum
        [SerializeField] int loadedMissile; // currently in mag
        public int curMissile; // currently in inventory
        bool nowLoading;

        [Header("Homing Missile")]
        [SerializeField] float homingDelay;
        [SerializeField] float homingReloadDelay;
        [SerializeField] HomingMissile Homing;
        [SerializeField] RectTransform lockonUI;
        [SerializeField] Transform homingPos;
        List<GameObject> LockonEnemy = new List<GameObject>();
        List<RectTransform> LockonUIList = new List<RectTransform>();
        public int curHoming;
        [SerializeField] int maxHoming;
        [SerializeField] int loadedHoming;
        bool nowShooting;
        float homingCurDelay;

        float curDelay;
        Vector3 camOriginPos;
        bool bulletProof = false;
        void Start()
        {
            camOriginPos = FrontCam.transform.localPosition;
            canvasRect = mainCanvas.GetComponent<RectTransform>();
            loadedMissile = maxMissile;
            loadedHoming = maxHoming;
            UImanager.GetMissileInformation(curMissile, loadedMissile, maxMissile);
            Hp = 100f;
        }

        void Update()
        {
            SetCamera();
            FollowAimPoint();
            SwapWeapon();
            Move();
            FollowGun(FrontGun_1.transform);
            FollowGun(FrontGun_2.transform);
            FollowGun(missilePos);
            LockonUIFollow();

            HomingLoad();
            UImanager.GetMissileInformation(curMissile, loadedMissile, maxMissile);
            GameManager.Instance.PlayerHp = Hp;
            Hp = Mathf.Clamp(Hp, 0f, 100f);
            switch (weaponState)
            {
                case PlayerWeapon.MACHINEGUN:
                    MachinegunAttack();
                    break;
                case PlayerWeapon.MISSILELAUNCER:
                    MissileAttack();
                    break;
                case PlayerWeapon.HOMINGMISSILE:
                    HomingAttack();
                    break;
            }
        }

        void GunsRotate()
        {
            guns.ToList().ForEach(item => item.Rotate(Vector3.back * Time.deltaTime * 1000f));
        }

        private void FixedUpdate()
        {
        }

        void HomingLoad()
        {
            if (homingCurDelay >= homingReloadDelay && loadedHoming < maxHoming && LockonUIList.Count <= 0)
            {
                loadedHoming++;
                curHoming--;
                homingCurDelay = 0f;
            }
            else if (loadedHoming < maxHoming)
                homingCurDelay += Time.deltaTime;

            UImanager.GetHomingLoad(homingCurDelay, homingReloadDelay);
            UImanager.GetHomingInformation(curHoming, loadedHoming, maxHoming);
        }

        void HomingAttack()
        {
            if (Input.GetMouseButtonDown(1) && loadedHoming > 0)
            {
                RaycastHit hit;
                Ray ray = FrontCam.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit, 300f, HitLayer);

                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    loadedHoming--;
                    GameObject temp = hit.transform.gameObject;
                    RectTransform lockon = Instantiate(lockonUI, canvasRect);
                    LockonUIList.Add(lockon);
                    LockonEnemy.Add(temp);
                }
            }

            if (Input.GetMouseButtonDown(0) && LockonEnemy.Count > 0 && !nowShooting)
            {
                StartCoroutine(HomingShoot());
            }

        }

        IEnumerator HomingShoot()
        {
            nowShooting = true;
            for (int i = 0; i < LockonEnemy.Count; i++)
            {
                HomingMissile homing = Instantiate(Homing);
                homing.Init(homingPos, LockonEnemy[i].transform, 2f, 6f, 3f);
                Destroy(LockonUIList[i].gameObject);
                yield return new WaitForSeconds(homingDelay);
            }
            LockonUIList.Clear();
            LockonEnemy.Clear();
            nowShooting = false;
        }

        void LockonUIFollow()
        {
            if (LockonEnemy.Count > 0)
            {
                for (int i = 0; i < LockonEnemy.Count; i++)
                {
                    Vector2 viewportPos = FrontCam.WorldToViewportPoint(LockonEnemy[i].transform.position);
                    Vector2 worldObjPos = new Vector2(
                        ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                        ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
                        );
                    try
                    {
                        LockonUIList[i].anchoredPosition = worldObjPos;
                    }
                    catch (MissingReferenceException e)
                    {

                    }
                }
            }
        }

        void MissileAttack()
        {
            if (Input.GetMouseButton(0))
            {
                if (curDelay >= missileDelay && loadedMissile > 0 && !nowLoading)
                {
                    Instantiate(Missile, missilePos.position, missilePos.rotation * Quaternion.Euler(90, 0, 0));
                    curDelay = 0f;
                    loadedMissile--;
                }
                else if (loadedMissile <= 0 && !nowLoading)
                {
                    StartCoroutine(MagLoading(5f));
                }

                curDelay += Time.deltaTime;
            }
            else curDelay = missileDelay;

            if (Input.GetKeyDown(KeyCode.R) && !nowLoading) StartCoroutine(MagLoading(5f));
        }

        IEnumerator MagLoading(float duration)
        {
            float timer = duration;
            nowLoading = true;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                UImanager.GetMissileLoad(timer, duration);
                yield return null;
            }

            if (curMissile >= maxMissile)
            {
                curMissile += loadedMissile;
                curMissile -= maxMissile;

                loadedMissile = maxMissile;
            }
            else
            {
                loadedMissile = curMissile;
                curMissile = 0;
            }

            nowLoading = false;
        }

        void MachinegunAttack()
        {
            if (Input.GetMouseButton(0))
            {
                GunsRotate();
                if (curDelay >= machinegunDelay - (GunSpeedLevel * 0.05f))
                {
                    Projectile tp1 = Instantiate(bullet, frontPos1.position, frontPos1.rotation * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(Random.insideUnitSphere * (spread - GunSpreadLevel * 0.1f)));
                    Projectile tp2 = Instantiate(bullet, frontPos2.position, frontPos2.rotation * Quaternion.Euler(90, 0, 0) * Quaternion.Euler(Random.insideUnitSphere * (spread - GunSpreadLevel * 0.1f)));
                    tp1.damage += GunDamageLevel;
                    tp2.damage += GunDamageLevel;
                    gunEffect_1.Play();
                    gunEffect_2.Play();
                    curDelay = 0f;
                }
                curDelay += Time.deltaTime;
            }
            else
                curDelay = machinegunDelay;
        }

        void SwapWeapon()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                weaponState = PlayerWeapon.MACHINEGUN;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                weaponState = PlayerWeapon.MISSILELAUNCER;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                weaponState = PlayerWeapon.HOMINGMISSILE;

            UImanager.GetCurWeapon((int)weaponState);
        }

        void Move()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            xRot = Mathf.Lerp(xRot, vertical * -15f, Time.deltaTime * rotateSpeed);
            zRot = Mathf.Lerp(zRot, horizontal * -30f, Time.deltaTime * rotateSpeed);

            transform.Translate(new Vector3(horizontal, vertical, 0) * Time.deltaTime * moveSpeed);

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, -150f, 150f);
            pos.y = Mathf.Clamp(pos.y, 25f, 50f);
            transform.position = pos;

            Container.localRotation = Quaternion.Euler(new Vector3(xRot, 0, zRot));

        }

        void SetCamera()
        {
            Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }

        void FollowGun(Transform trans)
        {
            Ray ray = FrontCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitResult;

            if (Physics.Raycast(ray, out hitResult, 300f, HitLayer))
            {
                Vector3 mouseDir = new Vector3(hitResult.point.x - trans.position.x, hitResult.point.y - trans.position.y, hitResult.point.z) - trans.position;
                trans.LookAt(hitResult.point/* - new Vector3(0, transform.position.y, 0)*/);
                aimPointPos = hitResult.point/* - new Vector3(0, transform.position.y, 0)*/;
            }
        }

        void FollowAimPoint()
        {
            Vector2 viewportPos = Camera.main.WorldToViewportPoint(aimPointPos);
            Vector2 worldObject_ScreenPos = new Vector2(
                ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            AimPoint.GetComponent<RectTransform>().anchoredPosition = worldObject_ScreenPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") && !bulletProof)
            {
                OnDamage(other.GetComponentInParent<EnemyBase>().damage);
            }

            if (other.CompareTag("EnemyBullet") && !bulletProof)
            {
                OnDamage(other.GetComponent<EnemyBullet>().damage);
            }
        }

        public void OnDamage(float damage)
        {
            Hp -= damage;
            StartCoroutine(CameraShake(0.3f, 2f));
            StartCoroutine(BulletProofCoroutine(1.5f));
        }

        IEnumerator BulletProofCoroutine(float delay)
        {
            bulletProof = true;
            yield return new WaitForSeconds(delay);
            bulletProof = false;
        }

        IEnumerator CameraShake(float _time, float _amount)
        {
            float curTime = _time;

            while (curTime > 0f)
            {
                Vector3 randPos = Random.insideUnitSphere * _amount + camOriginPos;
                FrontCam.transform.localPosition = randPos;
                curTime -= Time.deltaTime;
                yield return null;
            }

            FrontCam.transform.localPosition = camOriginPos;
        }
    }

}
