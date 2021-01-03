using System.Collections.Generic;
using UnityEngine;

public class MutationsList : MonoBehaviour
{
    public GameObject mutationInfo;

    private List<GameObject> _mutations = new List<GameObject>();

    public void AddMutation(Mutation mutation)
    {
        var gameObject = Instantiate(mutationInfo, transform);

        var info = gameObject.GetComponent<MutationInfo>();
        info.SetMutationInfo(mutation);
        
        _mutations.Add(gameObject);
    }

    public void Clear()
    {
        foreach (var mutation in _mutations)
        {
            Destroy(mutation);
        }
        
        _mutations.Clear();
    }
}
