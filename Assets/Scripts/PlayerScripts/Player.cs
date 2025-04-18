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
    public bool grounded;
    private bool regeningHP;
    private bool regeningMana;
    public bool poisoned;
    public bool isDead;
    private bool CastingActive;
    [SerializeField] int selectedSpell;
    public List<Texture> spellSprites;


    public float speed;
    public float jumpForce;
    public float health;
    public float maxHealth;
    public float mana;
    public float maxMana;
    [SerializeField] private float iFrames;
    [SerializeField] private float gravityScale;


    public GameObject healthBar;
    public GameObject healthBarColor;
    public GameObject manaBar;
    public GameObject deathScreen;
    public GameObject selectedSlot;
    public GameObject prevSlot;
    public GameObject nextSlot;
    public List<GameObject> spells;
    public List<GameObject> zombieSpawns;
    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI manaTxt;
    private Coroutine HPRegen;
    private Coroutine MRegen;
    public Rigidbody rb;
    public Camera cam;
    private GameManager gameManager;
    private Vector3 maxSpeed;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody>();
        gameManager = FindFirstObjectByType<GameManager>();
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
        if(context.performed && gameManager.locked && !isDead)
        {
            switch (selectedSpell)
            {
                case 0:
                    if (mana >= spells[0].GetComponent<Spell>().manaCost && !CastingActive)
                    {
                       Instantiate(spells[0], cam.gameObject.transform.position +(cam.gameObject.transform.forward * 2), cam.gameObject.transform.rotation);
                    }
                break;
                case 1:
                    if(mana >= spells[1].GetComponent<Spell>().manaCost && !CastingActive)
                    {
                        Instantiate(spells[1], cam.gameObject.transform.position + (cam.gameObject.transform.forward * 2), cam.gameObject.transform.rotation);
                    }
                    break;
                case 2:
                    if (mana >= spells[2].GetComponent<Spell>().manaCost && !CastingActive)
                    {
                        spells[2].GetComponent<WindSpell>().Cast();
                    }
                    break;
                case 3:
                    if (mana >= spells[3].GetComponent<Spell>().manaCost && !CastingActive)
                    {
                        Instantiate(spells[3], Vector3.zero, Quaternion.identity);
                    }
                    break;
                case 4:
                    if (mana >= spells[4].GetComponent<Spell>().manaCost && !CastingActive)
                    {
                        Instantiate(spells[4], Vector3.zero, Quaternion.identity);
                    }
                    break;
                case 5:
                    if(mana >= spells[5].GetComponent<Spell>().manaCost && !CastingActive)
                    {
                        CastingActive = true;
                        mana -= spells[5].GetComponent<Spell>().manaCost;
                        StartCoroutine(PebbleSpell(20));
                    }
                    break;
                case 6:
                    if (mana >= spells[6].GetComponent<Spell>().manaCost && !CastingActive)
                    {
                        mana -= spells[6].GetComponent<Spell>().manaCost;
                        for(int i = 0; i < zombieSpawns.Count; i++)
                        {
                            Instantiate(spells[6], new Vector3(zombieSpawns[i].transform.position.x, 0.5f, zombieSpawns[i].transform.position.z), gameObject.transform.rotation);
                        }
                    }
                    break;
                
            }
        }
    }
    private void Update()
    {
        if(iFrames < 0)
        {
            iFrames = 0;
        }
        if(iFrames > 0)
        {
            iFrames--;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.Pause();
        }
        if (poisoned)
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
        if(health <= 0)
        {
            health = 0;
            Die();
        }
        if(mana < 0)
        {
            mana = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        Vector3 direction = transform.right * moveDir.x + transform.forward * moveDir.y;
        rb.AddForce(direction * speed);
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
        if (iFrames <= 0 || poisoned)
        {
            health -= dmg;
            iFrames = 50;
        }
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

    private IEnumerator PebbleSpell(int count)
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(spells[5], cam.gameObject.transform.position + (cam.gameObject.transform.forward * 2), cam.gameObject.transform.rotation);
        count--;
        if (count > 0)
        {
            StartCoroutine(PebbleSpell(count));
        }
        else
        {
            CastingActive = false;
        }
    }

   
    public void Die()
    {
        Time.timeScale = 0;
        gameObject.GetComponent<Player>().enabled = false;
        cam.GetComponent<PlayerCamera>().enabled = false;
        deathScreen.GetComponent<Canvas>().enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isDead = true;
    }
}
