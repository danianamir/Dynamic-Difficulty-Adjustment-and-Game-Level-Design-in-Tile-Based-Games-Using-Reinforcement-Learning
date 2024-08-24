

using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;

using UnityEngine;

public class Manager : MonoBehaviour
{


    public float w_difficulty;
    private static Manager _instance;

    public static Manager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Check if an instance already exists in the scene
                _instance = FindObjectOfType<Manager>();

                // If no instance exists, create a new GameObject with the Singleton component
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("Singleton");
                    _instance = singletonObject.AddComponent<Manager>();
                }
            }
            return _instance;
        }
    }

    [HideInInspector]

    public bool in_level_desing;

    [HideInInspector]
    public int max_tile_changed;
    public int grid_size;
    public int max_game_step;


    public int player_speed;

    public int player_health;

    [System.Serializable]
    public struct Initial_results
    {
        public Player player;
        public int total_enemy;
        public int total_health;
        public int total_treasure;
        public int total_wall;

        public int max_game_step;

    }


    public Initial_results initial_Results;
    // end result
    [System.Serializable]
    public struct EndResults
    {
        public int total_treasure;
        public int gain;
        public int not_gain;
        public float difficulty;

        public int game_steps;
        public int max_game_steps;



        public int healt_count;
        public int consumed_health;

        public int enemy_count;
        public int killed_enemy;

        public int wall_count;





    }

    [HideInInspector]
    public EndResults endResults;


    private LearnerInstantiator ln;



    [System.Serializable]




    public enum Phases
    {
        None,
        end_not_playable,
        playable,
        end_playable,

    }

    public Phases phases;


    private Canvas canvas;
    private TMPro.TMP_Text phase_shower;
    private TMPro.TMP_Text expected_difficulty_shower;
    private TMPro.TMP_Text round_shower;

    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }




    private void Start()
    {



        ln = FindObjectOfType<LearnerInstantiator>();

        canvas = FindObjectOfType<Canvas>();
        Transform phase = canvas.transform.GetChild(0);
        phase_shower = phase.gameObject.GetComponent<TMP_Text>();

        Transform expectd = canvas.transform.GetChild(4);
        expected_difficulty_shower = expectd.gameObject.GetComponent<TMP_Text>();

        Transform round = canvas.transform.GetChild(5);
        round_shower = round.gameObject.GetComponent<TMP_Text>();
    }


    private void FixedUpdate()
    {

        //   Debug.Log("manager");
        if (in_level_desing)
        {

            phase_shower.text = "phase:" + "level designing";


            if (check_end_of_level_design(ln.step))
            {

                ln.step = 0;

                // check plability 
                if (check_game_playability())
                {




                    initial_Results = assign_game_inital_results();
                    in_level_desing = false;
                    phases = Phases.playable;

                }
                else
                {
                    in_level_desing = false;
                    phases = Phases.end_not_playable;

                }
            }

        }
        else
        {

            phase_shower.text = "phase:" + "game_playing";

            if (check_game_end(initial_Results.player.step, max_game_step))
            {


                endResults = assign_game_end_results(initial_Results, initial_Results.player.step);
                initial_Results.player.step = 0;
                phases = Phases.end_playable;





            }

        }





    }



    public bool check_end_of_level_design(int step)
    {



        if (


                   step >= grid_size * grid_size
        // || ln.changed_tiles_percentages >= max_tile_percrntage


        )
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    public bool check_game_playability()
    {
        CustomTags[] cts = FindObjectsOfType<CustomTags>();

        int number_of_players = 0;
        int number_of_treasure = 0;

        foreach (CustomTags tag in cts)
        {
            if (tag.player)
            {
                number_of_players++;
            }
            if (tag.treasure)
            {
                number_of_treasure++;
            }
        }




        if (number_of_players == 1 && number_of_treasure >= 2)
        {


            return true;
        }
        else
        {

            return false;
        }



    }
    public Initial_results assign_game_inital_results()
    {
        Initial_results initial = new Initial_results();

        CustomTags[] cts = FindObjectsOfType<CustomTags>();

        int number_of_enemies = 0;
        int number_of_health = 0;
        int number_of_treasure = 0;
        int number_of_wall = 0;


        foreach (CustomTags tag in cts)
        {
            if (tag.treasure)
            {
                number_of_treasure++;
            }
            if (tag.health_item)
            {
                number_of_health++;
            }
            if (tag.wall)
            {
                number_of_wall++;
            }
            if (tag.enemy)
            {
                number_of_enemies++;
            }
        }

        int total_enemy = number_of_enemies;
        Player player = FindObjectOfType<Player>();








        initial.player = player;
        initial.total_enemy = total_enemy;
        initial.total_health = number_of_health;
        initial.total_treasure = number_of_treasure;
        initial.total_wall = number_of_wall;
        initial.max_game_step = max_game_step;
        return initial;


    }



    public bool check_game_end(int step, int max_step)
    {
        CustomTags[] cts = FindObjectsOfType<CustomTags>();

        int player_count = 0;
        int treasure_count = 0;

        foreach (var item in cts)
        {
            if (item.player)
            {
                player_count++;
            }
            if (item.treasure)
            {
                treasure_count++;
            }
        }

        if (player_count == 1)
        {
            if (step >= max_step)
            {
                return true;
            }
            else
            {
                if (treasure_count > 0)
                {

                    return false;
                }
                else
                {
                    return true;
                }
            }


        }
        else
        {
            return true;
        }
    }
    public EndResults assign_game_end_results(Initial_results initial, int game_steps)
    {







        CustomTags[] cts = FindObjectsOfType<CustomTags>();

        int player_count = 0;
        int enemies_remain = 0;
        int health_remain = 0;
        int treausre_remain = 0;

        foreach (var item in cts)
        {
            if (item.player)
            {
                player_count++;
            }
            if (item.enemy)
            {
                enemies_remain++;
            }
            if (item.health_item)
            {
                health_remain++;
            }
            if (item.treasure)
            {
                treausre_remain++;
            }

        }










        EndResults results = new EndResults();

        results.total_treasure = initial.total_treasure;
        results.gain = initial.total_treasure - treausre_remain;
        results.not_gain = treausre_remain;
        results.difficulty = ((float)results.gain / (float)results.total_treasure) - 0.5f;


        results.game_steps = game_steps;

        results.healt_count = initial.total_health;
        results.consumed_health = initial.total_health - health_remain;

        results.enemy_count = initial.total_enemy;
        results.killed_enemy = initial.total_enemy - enemies_remain;

        results.wall_count = initial.total_wall;





        // Debug.Log("killed enemy= " + killed_enemy + ",,,," + "steps=" + game_steps);

        return results;


    }






    public void Restart_episode()
    {





        ln.buildable = new List<bool>();
        for (int i = 0; i < (grid_size * grid_size); i++)
        {
            ln.buildable.Add(true);
        }


        int s = UnityEngine.Random.Range(0, 5);


        if (s == 0)
        {
            max_tile_changed = 0;
        }
        if (s == 1)
        {
            max_tile_changed = 2;
        }
        if (s == 2)
        {
            max_tile_changed = 4;
        }
        if (s == 3)
        {
            max_tile_changed = 6;
        }
        if (s == 4)
        {
            max_tile_changed = 8;
        }





        int j = 0;
        while (j < max_tile_changed)
        {

            int ind = UnityEngine.Random.Range(0, grid_size * grid_size);

            if (ln.buildable[ind] == true)
            {
                ln.buildable[ind] = false;
                j++;
            }



        }









        //delete all gameobjects
        CustomTags[] tags = FindObjectsOfType<CustomTags>();

        foreach (CustomTags tag in tags)
        {
            if (tag.destroyable)
            {
                Destroy(tag.gameObject);
            }

        }











        ln.index_position = UnityEngine.Random.Range(0, grid_size * grid_size);
        ln.huristic_initate_player = true;
        ln.step = 0;
        ln.time = 0;
        ln.timeee = 0;
        ln.initial_distance = ln.distance_to_goal_calculator();
        ln.constraint = ln.constraint_calculator();
        ln.obs = ln.observation_update();
        ln.satisfied = -1000;
        ln.w_l = -1000;




        in_level_desing = true;
        phases = Phases.None;
        endResults = new EndResults();
        initial_Results = new Initial_results();


    }








    public (float, float) reward_difficulty(float whight, EndResults results)
    {



        float gain;
        float total;
        float difficulty;
        float reward;


        gain = results.gain;
        total = results.total_treasure;



        difficulty = ((float)gain / (float)total) - 0.5f;



        reward = 1/(1+  math.abs(difficulty));

        reward = reward * whight;
        // Debug.Log("gain" + gain + "total" + total + "difficulty" + difficulty + "reward " + reward);



        return (reward, difficulty);


    }



    public float reward_level()
    {
        float reward = 0;

        reward = 1/ (1+ math.abs(ln.distance_to_goal_calculator()));

        //  Debug.Log("distance " + ln.distance_to_goal_calculator() + "reward " + reward);

        return reward;
    }
}
