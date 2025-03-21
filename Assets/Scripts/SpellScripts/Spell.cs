using UnityEngine;

public class Spell : MonoBehaviour
{
    public Player player;
    public int manaCost;
    public int damage;
    public int speed;
    public enum Elements
    {
        Fire,
        Ice,
        Rock,
        Wind,
        Electric,
        Null
    }
    public Elements element = Elements.Fire;
    

    public virtual void Start()
    {
        player = FindFirstObjectByType<Player>();
        ConsumeMana();
    }
    public void ConsumeMana()
    {
        player.mana -= manaCost;
    }
}
