
using System.Collections.Generic;
using TMPro;

using UnityEngine;


public class Player : Inteligence
{



    // player attribues
    public int health;
    public int stuck_time;
    private float time;
    public GameObject goal_treasure;
    public Vector2 treasure_target;







    //

    [HideInInspector]
    public List<GameObject> enemies_items;
    [HideInInspector]
    public List<GameObject> health_items;
    [HideInInspector]
    public List<GameObject> treasure_items;

    [HideInInspector]
    public int step = 0;

    [HideInInspector]
    Vector2 stuck_position;




    // player states
    public enum States
    {

        to_health,
        to_treasure,



    }

    [HideInInspector]
    public States player_state;

    private Health health_to_grab = null;


    private Canvas canvas;
    private TMP_Text game_steps;

    private void Awake()
    {

        goal_treasure = null;
        treasure_target = new Vector2(1, 1);
        time = 0;

        rg = GetComponent<Rigidbody2D>();
        GameObject ph = GameObject.Find("path_player");
        canvas = FindObjectOfType<Canvas>();
        Transform step = canvas.transform.GetChild(3);
        game_steps = step.gameObject.GetComponent<TMP_Text>();

        player_state = States.to_treasure;

        if (ph != null)
        {
            path = ph.GetComponent<PathFinding>();
        }
        else
        {

        }


        health = Manager.Instance.player_health;
        speed = Manager.Instance.player_speed;

    }






    public void FixedUpdate()
    {

        base.FixedUpdate();

        if (play_phase)
        {


            CustomTags[] tags = GameObject.FindObjectsOfType<CustomTags>();

            if (enemies_items.Count > 0)
            {
                enemies_items.Clear();
            }
            if (health_items.Count > 0)
            {
                health_items.Clear();
            }
            if (treasure_items.Count > 0)
            {
                treasure_items.Clear();
            }



            foreach (var item in tags)
            {
                if (item.enemy)
                {
                    enemies_items.Add(item.gameObject);
                }
                if (item.health_item)
                {
                    health_items.Add(item.gameObject);
                }
                if (item.treasure)
                {
                    treasure_items.Add(item.gameObject);
                }

            }

            time = time + Time.deltaTime;

            if (time > stuck_time)
            {
                if (Vector3.Distance(stuck_position, gameObject.transform.position) < 1.5f)
                {

                    Destroy(gameObject);
                }
                else
                {
                    stuck_position = gameObject.transform.position;
                }

                time = 0;

            }


            update_aim();
            move_to_aim();


            if (health <= 0)
            {
                Destroy(gameObject);
            }



            step++;
        }

        game_steps.text = "play step:" + step.ToString();


    }








    public void update_aim()
    {

        if (path != null)
        {





            switch (player_state)
            {



                case States.to_treasure:



                    if (treasure_items.Count > 0 )
                    {
                        if(goal_treasure == null)
                        {
                            float min_distance = 1000000000;


                            foreach (var item in treasure_items)
                            {
                                if (Vector3.Distance(item.transform.position, gameObject.transform.position) < min_distance)
                                {
                                    min_distance = Vector3.Distance(item.transform.position, gameObject.transform.position);
                                    goal_treasure = item;
                                }
                            }

                            treasure_target = goal_treasure.transform.position;
                            path.target = treasure_target;
                        }
                        else
                        {
                            path.target = goal_treasure.transform.position;
                        }



                    }
                    else
                    {
                        path.target=new Vector2 (1, 1);
                    }










                    Collider2D[] detected_colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(2, 2), 0f);
                    List<Health> health = new List<Health>();
                    foreach (var item in detected_colliders)
                    {

                        Health h = item.GetComponent<Health>();
                        if (h != null)
                        {
                            health.Add(h);
                        }

                    }

                    if (health.Count > 0)
                    {



                        health_to_grab = health[0];

                        player_state = States.to_health;



                    }

                    break;








                case States.to_health:

                    if (health_to_grab != null)
                    {
                        path.target = health_to_grab.transform.position;
                    }
                    else
                    {
                        player_state = States.to_treasure;
                    }



                    break;



            }

        }
        else
        {
            throw new System.Exception("player path is empty.");
        }


    }




}
