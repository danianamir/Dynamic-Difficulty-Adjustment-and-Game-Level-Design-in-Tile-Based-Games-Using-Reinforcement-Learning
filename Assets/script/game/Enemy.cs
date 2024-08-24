
using System.Collections.Generic;

using UnityEngine;




public class Enemy : Items
{


    public enum Direction
    {
        vertical,
        hrizontal,

    }

    public Direction direction;
    public float speed;
    public int damage;



    bool first_time = true;
    Rigidbody2D rg;

    // Start is called before the first frame update



    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        base.FixedUpdate();



        if (play_phase)
        {
            move(direction);

        }




    }



    public void move(Direction d)
    {



        LayerMask layerMask = Physics2D.AllLayers & ~((1 << 11) | (1 << 7));
        Collider2D[] cls = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(1, 1), 0f, layerMask);
        List<Collider2D> detected_colliders = new List<Collider2D>();
        foreach (var item in cls)
        {
            if (item.gameObject != gameObject)
            {


                detected_colliders.Add(item);

            }
        }




        if (direction == Direction.vertical)
        {

            if (first_time)
            {
                rg.velocity = new Vector2(0, speed);
                first_time = false;
            }
            else
            {

                if (detected_colliders.Count > 0)
                {

                    rg.velocity = new Vector2(0, -rg.velocity.y);
                }
                else
                {
                    rg.velocity = new Vector2(rg.velocity.x, rg.velocity.y);
                }

            }
        }




        if (direction == Direction.hrizontal)
        {

            if (first_time)
            {
                rg.velocity = new Vector2(speed, 0);
                first_time = false;

            }
            else
            {


                if (detected_colliders.Count > 0)
                {


                    rg.velocity = new Vector2(-rg.velocity.x, 0);
                }
                else
                {

                    rg.velocity = new Vector2(rg.velocity.x, rg.velocity.y);
                }

            }


        }



    }






    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (play_phase)
        {


            if (collision.gameObject.GetComponent<CustomTags>() != null)
            {
                CustomTags ct = collision.gameObject.GetComponent<CustomTags>();
                Player p;
                if (ct.player)
                {
                    p = ct.GetComponent<Player>();
                    p.health = p.health - damage;
                    Destroy(gameObject);
                }




            }

        }



    }






}
