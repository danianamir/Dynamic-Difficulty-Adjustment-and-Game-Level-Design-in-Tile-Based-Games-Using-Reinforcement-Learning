
using System.Collections.Generic;

using UnityEngine;



public class PathFinding : MonoBehaviour
{



    public struct AdjacentIndexes
    {
        public int[] adjecent_indexes;
        public bool[] adjacent_existence;




    }

    public enum PathType
    {
        existence,
 
        player,
    }

    public PathType pathType;


    [HideInInspector]
    public int grid_size;
    public float cell_size;
    [HideInInspector]
    public int cell_count;



    public Vector2 target;


    public GameObject[] directions = new GameObject[8];





    [System.Serializable]
    public struct Grid
    {
        public int index;
        public bool empty;
        public float distance_to_goal;
        public float x;
        public float y;
        public Directions directions;
        public bool target;

    }

    public List<Grid> grids = new List<Grid>();


    public enum Directions
    {
        None,
        up,
        down,
        left,
        right,
        up_right,
        up_left,
        down_right,
        down_left,
    }






    private void Start()
    {



        LearnerInstantiator ln = FindObjectOfType<LearnerInstantiator>();

        grid_size = Manager.Instance.grid_size;


        float count = (grid_size / cell_size);
        if (count % 1 == 0)
        {
            cell_count = (int)count;

            //Debug.Log(cell_count);
        }
        else
        {
            Debug.LogError(" cell count not integer");
        }

    }

    private void FixedUpdate()
    {



        update_directions(pathType);

        if (pathType == PathType.existence)
        {
            // update_visiuasl();
        }


    }






    public virtual void initial_grid_cells(PathType pathType)
    {

        grids.Clear();
        for (float i = cell_size / 2; i <= grid_size - cell_size / 2; i = i + cell_size)
        {
            for (float j = cell_size / 2; j <= grid_size - cell_size / 2; j = j + cell_size)
            {



                Grid grid = new Grid();
                grid.x = j;
                grid.y = i;

                Vector2 cell_posiotn = new Vector2(j, i);
                grid.distance_to_goal = Vector3.Distance(new Vector3(cell_posiotn.x, cell_posiotn.y, 0), new Vector3(target.x, target.y, 0));

                grid.empty = true;

                Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(j, i), new Vector2(cell_size / 2, cell_size / 2), 0f);

                if (colliders.Length > 0)
                {

                    for (int k = 0; k < colliders.Length; k++)
                    {


                        Items item = colliders[k].GetComponent<Items>();
                        if (item != null)
                        {




                            if (pathType == PathType.player)
                            {
                                if (item.type == 1 || item.type == 2|| item.type == 5)
                                {
                                    grid.empty = false;
                                }

                            }









                            if (pathType == PathType.existence)
                            {
                                if (item.type == 5)
                                {
                                    grid.empty = false;
                                }

                            }

                        }

                    }
                }


                grid.directions = Directions.None;
                grid.target = false;



                int index_x = (int)((grid.x + cell_size / 2) / cell_size) - 1;
                int index_y = (int)((grid.y + cell_size / 2) / cell_size) - 1;
                grid.index = (int)(index_x + index_y * cell_count);
                grids.Add(grid);
            }
        }
    }


    public void update_directions(PathType pathType)
    {



        initial_grid_cells(pathType);



        List<int> grid_to_search = new List<int>();





        // target cell initalization
        AdjacentIndexes aj_main = indexer_grid(target.x, target.y, grid_size);


        Grid target_cell = new Grid();
        target_cell = grids[aj_main.adjecent_indexes[0]];
        target_cell.directions = Directions.up;
        target_cell.empty = true;
        target_cell.target = true;
        grids[aj_main.adjecent_indexes[0]] = target_cell;


        // add adjacent to search
        for (int i = 1; i < 9; i++)
        {
            if (aj_main.adjacent_existence[i])
            {

                grid_to_search.Add(aj_main.adjecent_indexes[i]);
            }
        }




        int index = 0;
        while (index < grids.Count * 2)

        {
            if (grid_to_search.Count == 0)
            {

                break;
            }



            index++;
            // set main cell from search list
            // Debug.Log(grid_to_search[0]);
            float x = grids[grid_to_search[0]].x;
            float y = grids[grid_to_search[0]].y;
            AdjacentIndexes aj = indexer_grid(x, y, grid_size);


            // set adjacent to main cell
            int self = aj.adjecent_indexes[0];
            int up = aj.adjecent_indexes[1];
            int down = aj.adjecent_indexes[2];
            int left = aj.adjecent_indexes[3];
            int right = aj.adjecent_indexes[4];
            int up_right = aj.adjecent_indexes[5];
            int up_left = aj.adjecent_indexes[6];
            int down_right = aj.adjecent_indexes[7];
            int down_left = aj.adjecent_indexes[8];








            float min_distance = 1000000000000;



            // up 

            if (aj.adjacent_existence[1])
            {


                if (grids[up].distance_to_goal < min_distance
                    && grids[up].empty
                    && grids[up].directions != Directions.None)
                {

                    min_distance = grids[up].distance_to_goal;
                    Grid g = grids[self];
                    g.directions = Directions.up;
                    grids[self] = g;

                }
                if (grids[up].directions == Directions.None
                   && grids[up].empty
                   && !grid_to_search.Contains(grids[up].index))
                {
                    grid_to_search.Add(grids[up].index);

                }
            }



            // down

            if (aj.adjacent_existence[2])
            {

                if (grids[down].distance_to_goal < min_distance
                    && grids[down].empty
                    && grids[down].directions != Directions.None)
                {

                    min_distance = grids[down].distance_to_goal;

                    Grid g = grids[self];
                    g.directions = Directions.down;
                    grids[self] = g;

                }
                if (grids[down].directions == Directions.None
                   && grids[down].empty
                   && !grid_to_search.Contains(grids[down].index))
                {
                    grid_to_search.Add(grids[down].index);

                }
            }


            // left

            if (aj.adjacent_existence[3])
            {
                if (grids[left].distance_to_goal < min_distance
                    && grids[left].empty
                    && grids[left].directions != Directions.None
                    )
                {
                    min_distance = grids[left].distance_to_goal;

                    Grid g = grids[self];
                    g.directions = Directions.left;
                    grids[self] = g;

                }
                if (grids[left].directions == Directions.None
                   && grids[left].empty
                   && !grid_to_search.Contains(grids[left].index))
                {
                    grid_to_search.Add(grids[left].index);

                }

            }


            // right



            if (aj.adjacent_existence[4])
            {

                if (grids[right].distance_to_goal < min_distance
                    && grids[right].empty
                    && grids[right].directions != Directions.None)
                {
                    min_distance = grids[right].distance_to_goal;

                    Grid g = grids[self];
                    g.directions = Directions.right;
                    grids[self] = g;

                }
                if (grids[right].directions == Directions.None
                   && grids[right].empty
                   && !grid_to_search.Contains(grids[right].index))
                {
                    grid_to_search.Add(grids[right].index);

                }
            }









            //up_right



            if (aj.adjacent_existence[5])
            {



                if (grids[up_right].distance_to_goal < min_distance
                    && grids[up_right].empty
                    && grids[up_right].directions != Directions.None

                )
                {


                    if (grids[up].empty && grids[right].empty)
                    {
                        min_distance = grids[up_right].distance_to_goal;

                        Grid g = grids[self];
                        g.directions = Directions.up_right;
                        grids[self] = g;
                    }



                }





                if (grids[up_right].directions == Directions.None
                   && grids[up_right].empty
                   && !grid_to_search.Contains(grids[up_right].index))
                {
                    grid_to_search.Add(grids[up_right].index);

                }
            }


            // up_left



            if (aj.adjacent_existence[6])
            {






                if (grids[up_left].distance_to_goal < min_distance
                    && grids[up_left].empty
                    && grids[up_left].directions != Directions.None


                    )
                {

                    if (grids[up].empty && grids[left].empty)
                    {
                        min_distance = grids[up_left].distance_to_goal;

                        Grid g = grids[self];
                        g.directions = Directions.up_left;
                        grids[self] = g;
                    }

                }

                if (grids[up_left].directions == Directions.None
                   && grids[up_left].empty
                   && !grid_to_search.Contains(grids[up_left].index))
                {
                    grid_to_search.Add(grids[up_left].index);

                }
            }


            // down_right


            if (aj.adjacent_existence[7])
            {




                if (grids[down_right].distance_to_goal < min_distance
                    && grids[down_right].empty
                    && grids[down_right].directions != Directions.None

                )


                {

                    if (grids[down].empty && grids[right].empty)
                    {
                        min_distance = grids[down_right].distance_to_goal;

                        Grid g = grids[self];
                        g.directions = Directions.down_right;
                        grids[self] = g;
                    }

                }


                if (grids[down_right].directions == Directions.None
                    && grids[down_right].empty
                    && !grid_to_search.Contains(grids[down_right].index))
                {
                    grid_to_search.Add(grids[down_right].index);

                }
            }




            //down_left




            if (aj.adjacent_existence[8])
            {





                if (grids[down_left].distance_to_goal < min_distance
                    && grids[down_left].empty
                    && grids[down_left].directions != Directions.None

                 )
                {


                    if (grids[down].empty && grids[left].empty)
                    {
                        min_distance = grids[down_left].distance_to_goal;

                        Grid g = grids[self];
                        g.directions = Directions.down_left;
                        grids[self] = g;
                    }

                }



                if (grids[down_left].directions == Directions.None
                     && grids[down_left].empty
                     && !grid_to_search.Contains(grids[down_left].index))
                {
                    grid_to_search.Add(grids[down_left].index);

                }
            }




            // delte the main cell from serch list
            grid_to_search.RemoveAt(0);





        }












    }


    public virtual void update_visiuasl()
    {
        CustomTags[] cts = FindObjectsOfType<CustomTags>();


        foreach (CustomTags ct in cts)
        {
            if (ct.arrow)
            {
                Destroy(ct.gameObject);
            }


        }



        foreach (var item in grids)
        {
            if (item.directions == Directions.None)
            {
                // Instantiate(directions[8], new Vector3(item.x, item.y, 0), directions[0].transform.rotation);
            }
            if (item.directions == Directions.up)
            {
                Instantiate(directions[0], new Vector3(item.x, item.y, 0), directions[0].transform.rotation);
            }
            if (item.directions == Directions.down)
            {
                Instantiate(directions[1], new Vector3(item.x, item.y, 0), directions[1].transform.rotation);
            }
            if (item.directions == Directions.left)
            {
                Instantiate(directions[2], new Vector3(item.x, item.y, 0), directions[2].transform.rotation);
            }
            if (item.directions == Directions.right)
            {
                Instantiate(directions[3], new Vector3(item.x, item.y, 0), directions[3].transform.rotation);
            }
            if (item.directions == Directions.up_right)
            {
                Instantiate(directions[4], new Vector3(item.x, item.y, 0), directions[4].transform.rotation);
            }
            if (item.directions == Directions.up_left)
            {
                Instantiate(directions[5], new Vector3(item.x, item.y, 0), directions[5].transform.rotation);
            }
            if (item.directions == Directions.down_right)
            {
                Instantiate(directions[6], new Vector3(item.x, item.y, 0), directions[6].transform.rotation);
            }
            if (item.directions == Directions.down_left)
            {
                Instantiate(directions[7], new Vector3(item.x, item.y, 0), directions[7].transform.rotation);
            }
        }
    }
    public AdjacentIndexes indexer_grid(float x_input, float y_input, int grid_size)
    {






        AdjacentIndexes aj = new AdjacentIndexes();
        aj.adjecent_indexes = new int[9];
        aj.adjacent_existence = new bool[9];






        if (x_input > 0 && x_input < grid_size && y_input > 0 && y_input < grid_size)
        {

            // self
            float x_x = (Mathf.Ceil(x_input * (1 / cell_size)) / (1 / cell_size)) - cell_size / 2;
            float y_y = (Mathf.Ceil(y_input * (1 / cell_size)) / (1 / cell_size)) - cell_size / 2;
            int x = (int)((x_x + cell_size / 2) / cell_size) - 1;
            int y = (int)((y_y + cell_size / 2) / cell_size) - 1;



            aj.adjecent_indexes[0] = x + y * cell_count;
            aj.adjacent_existence[0] = true;








            // up
            if (x >= 0 && x < cell_count && y + 1 >= 0 && y + 1 < cell_count)
            {
                int new_x = x;
                int new_y = y + 1;

                aj.adjecent_indexes[1] = new_x + new_y * cell_count;
                aj.adjacent_existence[1] = true;
            }
            else
            {
                aj.adjecent_indexes[1] = -1;
            }




            // down
            if (x >= 0 && x < cell_count && y - 1 >= 0 && y - 1 < cell_count)
            {
                int new_x = x;
                int new_y = y - 1;
                aj.adjecent_indexes[2] = new_x + new_y * cell_count;
                aj.adjacent_existence[2] = true;
            }
            else
            {
                aj.adjecent_indexes[2] = -1;
            }




            // left
            if (x - 1 >= 0 && x - 1 < cell_count && y >= 0 && y < cell_count)
            {
                int new_x = x - 1;
                int new_y = y;
                aj.adjecent_indexes[3] = new_x + new_y * cell_count;
                aj.adjacent_existence[3] = true;
            }
            else
            {
                aj.adjecent_indexes[3] = -1;
            }


            // right

            if (x + 1 >= 0 && x + 1 < cell_count && y >= 0 && y < cell_count)
            {
                int new_x = x + 1;
                int new_y = y;
                aj.adjecent_indexes[4] = new_x + new_y * cell_count;
                aj.adjacent_existence[4] = true;
            }
            else
            {
                aj.adjecent_indexes[4] = -1;
            }



            // up_right

            if (x + 1 >= 0 && x + 1 < cell_count && y + 1 >= 0 && y + 1 < cell_count)
            {
                int new_x = x + 1;
                int new_y = y + 1;
                aj.adjecent_indexes[5] = new_x + new_y * cell_count;
                aj.adjacent_existence[5] = true;
            }
            else
            {
                aj.adjecent_indexes[5] = -1;
            }


            // up_left
            if (x - 1 >= 0 && x - 1 < cell_count && y + 1 >= 0 && y + 1 < cell_count)
            {
                int new_x = x - 1;
                int new_y = y + 1;
                aj.adjecent_indexes[6] = new_x + new_y * cell_count;
                aj.adjacent_existence[6] = true;
            }
            else
            {
                aj.adjecent_indexes[6] = -1;
            }



            // down_right
            if (x + 1 >= 0 && x + 1 < cell_count && y - 1 >= 0 && y - 1 < cell_count)
            {
                int new_x = x + 1;
                int new_y = y - 1;

                aj.adjecent_indexes[7] = new_x + new_y * cell_count;
                aj.adjacent_existence[7] = true;
            }
            else
            {
                aj.adjecent_indexes[7] = -1;
            }

            // down_left
            if (x - 1 >= 0 && x - 1 < cell_count && y - 1 >= 0 && y - 1 < cell_count)
            {
                int new_x = x - 1;
                int new_y = y - 1;
                aj.adjecent_indexes[8] = new_x + new_y * cell_count;
                aj.adjacent_existence[8] = true;
            }
            else
            {
                aj.adjecent_indexes[8] = -1;
            }




            return aj;
        }
        else
        {
            // Debug.LogError("not in range");
            return aj;
        }






    }

}
