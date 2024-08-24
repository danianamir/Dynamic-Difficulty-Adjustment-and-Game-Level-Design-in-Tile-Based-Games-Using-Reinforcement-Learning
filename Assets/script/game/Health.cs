
using UnityEngine;

public class Health : Items
{
    public int count;

    Player player;


    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();

        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        else
        {

        }

    }

 


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (play_phase)
        {
            if (collision.gameObject.GetComponent<CustomTags>() != null)
            {
                CustomTags ct = collision.gameObject.GetComponent<CustomTags>();
                if (ct.player)
                {
                    player.health = player.health + count;
                    Destroy(gameObject);
                }

            }

        }

    }
}
