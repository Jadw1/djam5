using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public Mutation[] mutations;

    private bool opened = false;
    private Transform door;
    private Animator animator;

    private void Start() {
        door = transform.GetChild(0);
        animator = GetComponent<Animator>();
        
        animator.SetBool("opened", opened);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log($"Available mutations: ${mutations.Length}");
    }

    private void OnTriggerStay(Collider other) {
        if (!opened && Input.GetKeyDown(KeyCode.E)) {
            opened = true;
            animator.SetBool("opened", opened);
        }
    }

    private void OnTriggerExit(Collider other) {
        opened = false;
        animator.SetBool("opened", opened);
    }
}
