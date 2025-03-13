using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    private Vector2 moveDir;
    public float speed;
    public float jumpForce;
    private bool locked;
    public bool grounded;
    private bool regeningHP;
    private bool regeningMana;
    public bool poisoned;
    [SerializeField] int selectedSpell;
    public GameObject selectedSlot;
    public GameObject prevSlot;
    public GameObject nextSlot;
    public List<Texture> spellSprites;


    public float health;
    public float maxHealth;
    public float mana;
    public float maxMana;

    [SerializeField]
    private float gravityScale;

    public GameObject healthBar;
    public GameObject healthBarColor;
    public TextMeshProUGUI healthTxt;
    public GameObject manaBar;
    public TextMeshProUGUI manaTxt;
    private Coroutine HPRegen;
    private Coroutine MRegen;
    public Rigidbody rb;
    public Camera cam;
    public List<GameObject> spells;

    private void Start()
    {
        cam = FindFirstObjectByType<Camera>();
        rb = GetComponent<Rigidbody>();
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
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && grounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Sqrt(jumpForce), rb.linearVelocity.z);
        }
    }

    public void SwapSpell(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int numkeyvalue;
            int.TryParse(context.control.name, out numkeyvalue);
            numkeyvalue--;
            selectedSpell = numkeyvalue;
        }
    }
    public void QuickSwap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int value = (int)context.ReadValue<float>();
            selectedSpell += value;
        }
    }

    public void CastSpell(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            switch (selectedSpell)
            {
                case 0:
                    if (mana >= spells[0].GetComponent<Spell>().manaCost)
                    {
                       Instantiate(spells[0], cam.gameObject.transform.position +(cam.gameObject.transform.forward * 2), cam.gameObject.transform.rotation);
                    }
                break;
                case 1:
                    if(mana >= spells[1].GetComponent<Spell>().manaCost)
                    {
                        Instantiate(spells[1], cam.gameObject.transform.position + (cam.gameObject.transform.forward * 2), cam.gameObject.transform.rotation);
                    }
                    break;
                case 2:
                    if(mana >= spells[2].GetComponent<Spell>().manaCost)
                    {
                        spells[2].GetComponent<WindSpell>().Cast();
                    }
                    break;
                case 3:
                    Debug.Log("Rockspike");
                    break;
                case 4:
                    if (mana >= spells[3].GetComponent<Spell>().manaCost)
                    {

                        Instantiate(spells[3], Vector3.zero, Quaternion.identity);
                    }
                        break;
                case 5:
                    Debug.Log("Pebbles");
                    break;
                case 6:
                    Debug.Log("Zombiesummon");
                    break;
                
            }
        }
    }
    private void Update()
    {
        if(poisoned)
        {
            healthBarColor.GetComponent<Image>().color = new Color(255, 0, 255, 255);
        }
        else
        {
            healthBarColor.GetComponent<Image>().color = Color.green;
        }
        if (selectedSpell > 6)
        {
            selectedSpell = 0;
        }
        if (selectedSpell < 0)
        {
            selectedSpell = 6;
        }
        selectedSlot.GetComponent<RawImage>().texture = spellSprites[selectedSpell];
        if(selectedSpell == 0)
        {
            prevSlot.GetComponent<RawImage>().texture = spellSprites[6];
            
        }
        else
        {
            prevSlot.GetComponent<RawImage>().texture = spellSprites[selectedSpell - 1];
        }
        if(selectedSpell == 6) 
        {
            nextSlot.GetComponent<RawImage>().texture = spellSprites[0];
        }
        else
        {
            nextSlot.GetComponent<RawImage>().texture = spellSprites[selectedSpell + 1];
        }

        healthBar.GetComponent<Slider>().value = health;
        manaBar.GetComponent<Slider>().value = mana;
        healthTxt.text = "Health:" + health.ToString("0") +"/"+ maxHealth;
        manaTxt.text = "Mana:" + mana.ToString("0") + "/" + maxMana;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                locked = false;
                cam.GetComponent<PlayerCamera>().enabled = false;
            }
            else if (!locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                locked = true;
                cam.GetComponent<PlayerCamera>().enabled = true;
            }
        }
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        if (mana > maxMana)
        {
            mana = maxMana;
        }

        if (health < maxHealth && !regeningHP)
        {
            HPRegen = StartCoroutine(HealthRegen());
            regeningHP = true;
        }
        if (mana < maxMana && !regeningMana)
        {
            MRegen = StartCoroutine(ManaRegen());
            regeningMana = true;
        }
        if(health < 0)
        {
            health = 0;
            Die();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        transform.Translate(new Vector3(moveDir.x, 0, moveDir.y) * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(health < maxHealth && HPRegen == null)
        {
            HPRegen = StartCoroutine(HealthRegen());
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.layer == 3 || collision.gameObject.layer == 8)
        {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 8)
        {
            grounded = false;
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

    private IEnumerator HealthRegen()
    {
        yield return new WaitForSeconds(1);
        health += 50;
        if (health < maxHealth)
        {
            HPRegen = StartCoroutine(HealthRegen());
        }
        else
        {
            regeningHP = false;
            HPRegen = null;
        }
    }
    private IEnumerator ManaRegen()
    {
        yield return new WaitForSeconds(1);
        mana += 100;
        if (mana < maxMana)
        {
            MRegen = StartCoroutine(ManaRegen());
        }
        else 
        {
            regeningMana = false;
            MRegen = null;
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
