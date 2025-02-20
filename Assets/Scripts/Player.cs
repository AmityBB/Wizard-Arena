using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    private Vector2 moveDir;
    public float speed;
    private bool locked;


    public float health;
    public float maxHealth;
    public float mana;
    public float maxMana;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        locked = true;
    }
    public void MovePlayer(InputAction.CallbackContext context)
    {
        if(context.performed || context.canceled)
        {
            moveDir = context.ReadValue<Vector2>().normalized;
        }
    }
    private void Update()
    {
        transform.Translate(new Vector3(moveDir.x, 0, moveDir.y) * speed);
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                locked = false;
            }
            else if (!locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                locked = true;
            }
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            health = 1;
            mana = 1;
            StartCoroutine(HealthRegen());
            StartCoroutine(ManaRegen());

        }
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        if (mana > maxMana)
        {
            mana = maxMana;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(health < maxHealth)
        {
            StartCoroutine(HealthRegen());
        }
    }

    private IEnumerator HealthRegen()
    {
        yield return new WaitForSeconds(1);
        health += 50;
        if (health < maxHealth)
        {
            StartCoroutine(HealthRegen());
        }
    }
    private IEnumerator ManaRegen()
    {
        yield return new WaitForSeconds(1);
        mana += 100;
        if (mana < maxMana)
        {
            StartCoroutine(ManaRegen());
        }
    }
}
