using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    public int Hp;
    public float DeadTime;
    public bool bKunckBack;

    private float _knuckBackTiem;
    private float _knuckBack;
    private int _directoin;
    private bool _bdead;

    [HideInInspector]
    public int Player_dir;
    private GameObject _player_gb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.manager.CurrentEnemyCount++;

        _player_gb = GameObject.Find("Player");
    }

    private void Update()
    {
        UpdateBorder();
        UpdatePlayerPos();

        if (_bdead)
            UpdateDead();
    }

    private void FixedUpdate()
    {
        UpdateKnuckBack();
    }

    private void UpdateBorder()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0.0325f) pos.x = 0.0325f;
        if (pos.x > 0.9675f) pos.x = 0.9675f;

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void UpdatePlayerPos()
    {
        if (transform.position.x < _player_gb.transform.position.x)
            Player_dir = 1;
        else
            Player_dir = -1;
    }

    private void UpdateKnuckBack()
    {
        if (bKunckBack && Hp > 0 && _knuckBackTiem > 0)
        {
            transform.position += Vector3.right * _knuckBack * _directoin * Time.deltaTime;
            _knuckBackTiem -= Time.deltaTime;
        }
    }

    private void UpdateDead()
    {
        gameObject.layer = 6;

        _animator.SetBool("bDead", true);

        _spriteRenderer.color -= new Color(0, 0, 0, DeadTime) * Time.deltaTime;

        if (_spriteRenderer.color.a <= 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (gameObject.name == "Boss1(Clone)")
        {
            GameManager.manager.GameClear();
            return;
        }

        if (!GameInstance.instance.bPlaying)
            return;

        if (Random.Range(0, 2) == 0)
        {
            int item = 0;

            if (!GameInstance.instance.bHatItem)
            {
                item = Random.Range(0, 5);

                if (item == 4)
                    GameInstance.instance.bHatItem = true;
                else
                    GameInstance.instance.ItemInventroy[item]++;
            }
            else
            {
                item = Random.Range(0, 4);

                GameInstance.instance.ItemInventroy[item]++;
            }

            SoundManager.soundManager.PlaySfx(SoundManager.Sfx.GetItem);
        }

        GameManager.manager.CurrentEnemyCount--;
    }

    public void Hit(string weapon, float direction, int additionalDamage)
    {
        int damage = 0;

        if (weapon == "Pistol")
        {
            damage = 1;
            _knuckBack = 5;
        }
        else
        {
            damage = 3;
            _knuckBack = 10;
        }

        Hp -= damage + additionalDamage;

        if (direction - transform.position.x > 0)
            _directoin = -1;
        else
            _directoin = 1;

        _knuckBackTiem = 0.1f;

        if (Hp > 0)
            StartCoroutine(Blink());
        else
            _bdead = true;
    }

    private IEnumerator Blink()
    {
        _spriteRenderer.color = new Color(1, 0.75f, 0.75f, 1);

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = Color.white;
    }
}
