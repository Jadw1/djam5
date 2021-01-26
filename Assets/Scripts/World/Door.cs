using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public TextMeshPro helperText;
    
    private Animator _animator;
    private bool _isOpen;
    private bool _canBeOpened;
    private PlayerStats _player;

    public List<StatsMutation> availableMutations;
    public List<WeaponMutation> weaponMutations;
    private List<Mutation> _mutations;
    private string _message;
    private bool _locked = true;
    
    public string afterOpenText = "No mutations left.";
    public string lockedText = "The door is locked.";

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _message = helperText.text;
        helperText.SetText(lockedText);
        PopulateMutations();
    }

    private void PopulateMutations()
    {
        // randomize
        var weaponMutation = weaponMutations
            .OrderBy(x => Guid.NewGuid())
            .FirstOrDefault();

        var mutations = new List<Mutation>();
        
        mutations.Add(weaponMutation);
        mutations.AddRange(availableMutations);
        
        _mutations = mutations
            .OrderBy(x => Guid.NewGuid())
            .Take(2)
            .ToList();
    }

    public void Unlock()
    {
        _locked = false;
        // display selected
        var builder = new StringBuilder(_message);
        foreach (var mutation in _mutations)
        {
            builder.AppendLine(mutation.name);
        }
        helperText.SetText(builder.ToString());
    }

    private void UpdateAnim()
    {
        _animator.SetBool("opened", _isOpen);
    }
    
    public delegate void OpenEvent(Door door);
    public event OpenEvent OnOpen;
    
    private void Open() {
        if (!LoadLevel())
        {
            helperText.SetText("Our dev team is preparing the next level, please wait!");
        }
        
        foreach (var mutation in _mutations)
        {
            _player.AddMutation(mutation);
        }
        
        _mutations.Clear();

        helperText.SetText(afterOpenText);
        _isOpen = true;
        UpdateAnim();
        OnOpen?.Invoke(this);
    }
    
    public delegate void CloseEvent(Door door);
    public event OpenEvent OnClose;

    private bool LoadLevel()
    {
        RoomCreator creator = GameObject.FindGameObjectWithTag("RoomCreator").GetComponent<RoomCreator>();
        return creator.LoadNextLevel(transform);
    }

    private void DestroyLevel()
    {
        RoomCreator creator = GameObject.FindGameObjectWithTag("RoomCreator").GetComponent<RoomCreator>();
        creator.DestroyOldLevel();
    }

    private void Close()
    {
        _isOpen = false;
        UpdateAnim();

        OnClose?.Invoke(this);
    }

    private void Update()
    {
        if (_locked && EnemySpawner.Instance.Count == 0)
        //if (_locked)
        {
            Unlock();
        }
        
        if (_canBeOpened && !_isOpen && Input.GetKeyDown(KeyCode.E))
        {
            Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var playerStats = other.GetComponent<PlayerStats>();
        
        if (playerStats != null)
        {
            var playerPos = playerStats.transform.position;
            var difference = playerPos - transform.position;
            difference = difference.normalized;
            var dot = Vector3.Dot(difference, transform.forward);

            if (dot > 0 && _isOpen)
            {
                DestroyLevel();
                Close();
            }
            
            Debug.Log(dot);
            
            _player = null;
            helperText.enabled = false;
            _canBeOpened = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerStats = other.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            _player = playerStats;
            helperText.enabled = true;
            _canBeOpened = !_locked;
        }
    }
}