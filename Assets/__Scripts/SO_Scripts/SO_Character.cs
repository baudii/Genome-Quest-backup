using UnityEngine;

[CreateAssetMenu(menuName = "SO/Character")]
public class SO_Character : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public string RelationshipWithPlayer;
    [SerializeField] public string Description;
    [SerializeField] public string Family;
    [SerializeField] public string Age;
    [SerializeField] public string Gender;
}
