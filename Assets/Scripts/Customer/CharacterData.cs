using UnityEngine;

[CreateAssetMenu(
    fileName = "Character",
    menuName = "Game/Character"
)]
public class CharacterData : ScriptableObject
{
    public string characterName;

    public Sprite neutral;

    public Sprite happy;

    public Sprite angry;

    public Sprite shocked;

    public Sprite confused;
}