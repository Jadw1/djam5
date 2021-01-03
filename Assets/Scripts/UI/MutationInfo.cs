using TMPro;
using UnityEngine;

public class MutationInfo : MonoBehaviour
{
    public TextMeshProUGUI mutationName;
    public TextMeshProUGUI mutationDescription;

    public void SetMutationInfo(Mutation mutation)
    {
        mutationName.SetText(mutation.name);
        mutationDescription.SetText(mutation.description);
    }
}
