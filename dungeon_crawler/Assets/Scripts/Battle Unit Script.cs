using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [Header("Unit Stats")]
    public string unitName = "Unit";
    public int maxHealth = 100;
    public int currentHealth;
    public int attackPower = 20;
    public int defense = 5;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;

    private bool isDefending = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public int Attack()
    {
        int damage = attackPower + Random.Range(-5, 6);
        return Mathf.Max(damage, 1);
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = damage;

        if (isDefending)
        {
            actualDamage = Mathf.Max(damage - defense * 2, 0);
            Debug.Log($"{unitName} blocked some damage!");
        }
        else
        {
            actualDamage = Mathf.Max(damage - defense, 0);
        }

        currentHealth -= actualDamage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        Debug.Log($"{unitName} took {actualDamage} damage! HP: {currentHealth}/{maxHealth}");
    }

    public void Defend()
    {
        isDefending = true;
    }

    public void ResetDefense()
    {
        isDefending = false;
    }

    // ADD THE HEAL METHOD HERE ⬇️
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);  // Don't exceed max

        Debug.Log($"{unitName} healed {amount} HP! HP: {currentHealth}/{maxHealth}");
    }
    // ⬆️ HEAL METHOD ENDS HERE

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    System.Collections.IEnumerator FlashRed()
    {
        Color original = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = original;
    }
}