using UnityEngine;

[CreateAssetMenu(fileName = "Name", menuName = "Scriptable Objects/Character Info")]
public class SO_CharacterInfo : ScriptableObject
{
    [SerializeField] public string _name;
    [SerializeField] public string _relationToPlayer;
    [SerializeField, TextArea(10, 20)] public string _description;
}
