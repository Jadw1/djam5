using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class Enemy : Entity<EnemyStats>
{
    public AudioClip onHit;
    public AudioClip onDeath;
    
    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;
    private ParticleSystem _particleSystem;
    private Animator _animator;
    private AudioSource _audioSource;

    private Player _player;

    private float _cooldown;
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Start() 
    {
        Init();
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _particleSystem = GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _cooldown = Time.time;
    }

    private bool IsPlayerInRange()
    {
        // TODO: Make sense
        return Vector3.Distance(_player.transform.position, transform.position) <= _stats.size;
    }

    private bool CanAttack()
    {
        return _cooldown <= Time.time;
    }

    private void AttackPlayer()
    {
        // TODO: Make sense
        _cooldown = Time.time + _stats.dexterity;
        _animator.SetTrigger(Attack);
        _player.TakeDamage(_stats.damage);
    }

    private void Update()
    {
        if (!_player) {
            return;
        }

        if (IsPlayerInRange() && CanAttack())
        {
            AttackPlayer();
        }
        else
        {
            GoToPlayer();
        }
    }

    private void GoToPlayer() {
        _agent.SetDestination(_player.transform.position);
    }

    public override void TakeDamage(float amount)
    {
        if (_stats.health - amount > 0)
        {
            _audioSource.PlayOneShot(onHit);
        }
        
        base.TakeDamage(amount);
        _particleSystem.Play();
    }

    protected override void Die()
    {
        
        _audioSource.PlayOneShot(onDeath);
        base.Die();
    }

    public void PushBack(Vector3 origin, float force)
    {
        // fifty is a magic number
        _rigidbody.AddExplosionForce(force * 50, origin, 0);
    }
}
