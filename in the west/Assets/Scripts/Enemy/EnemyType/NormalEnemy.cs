using System.Collections;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    enum State
    {
        Move,
        Attack,
    }

    State CurrentState = State.Move;

    private EnemySystem _enemySystem;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private PlayerSystem _playerSystem;
    private GameObject _player_gb;

    public float MoveSpeed;
    public int Damage;
    public float KnunkBack;

    public float RayLenth;
    public LayerMask Player;
    public Vector2 AttackBoxSize;

    private int _dir;

    private bool _bAttack;
    private bool _bSwing;
    private float _attackTime;

    private void Awake()
    {
        _enemySystem = GetComponent<EnemySystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _player_gb = GameObject.Find("Player");
        _playerSystem = _player_gb.GetComponent<PlayerSystem>();
    }

    private void Update()
    {
        if (!GameInstance.instance.bPlaying) return;

        if (_enemySystem.Hp > 0)
        {
            UpdateRaycaset();
            UpdateAttack();
        }          
    }

    private void FixedUpdate()
    {
        if (!GameInstance.instance.bPlaying) return;

        if (_enemySystem.Hp > 0 && !_bAttack)
            UpdateMove();
    }

    private void UpdateMove()
    {
        if(CurrentState == State.Move)
        {
            transform.position += new Vector3(_enemySystem.Player_dir, 0, 0) * MoveSpeed * Time.deltaTime;

            if (_enemySystem.Player_dir == 1)
            {
                _dir = 1;
                _spriteRenderer.flipX = true;
            }              
            else
            {
                _dir = -1;
                _spriteRenderer.flipX = false;
            }           
        }     
    }

    private void UpdateAttack()
    {
        if (!_bAttack)
            return;

        _attackTime += Time.deltaTime;

        _animator.SetBool("bAttack", true);

        if (_attackTime >= 0.4 && !_bSwing)
        {
            Collider2D attackBox = Physics2D.OverlapBox(transform.position + new Vector3(_dir / 1.5f, 0, 0), AttackBoxSize, 0, Player);

            if (attackBox != null)
            {
                _playerSystem.Hit(Damage, KnunkBack, transform.position.x);
            }

            SoundManager.soundManager.PlaySfx(SoundManager.Sfx.Swing);
            _bSwing = true;
        }
        if (_attackTime >= 0.8)
        {
            _animator.SetBool("bAttack", false);
            _bAttack = false;
        }
    }

    private void UpdateRaycaset()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right * _dir, RayLenth, Player);
        Debug.DrawRay(transform.position, Vector3.right * _dir * RayLenth, Color.red);

        if (!_bAttack && hit.collider != null)
        {
            _attackTime = 0;
            _bSwing = false;
            _bAttack = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (_enemySystem != null)
            Gizmos.DrawWireCube(transform.position + new Vector3(_dir / 1.5f, 0, 0), AttackBoxSize);
    }
}
