
using UnityEngine;
using static PathFinding;

public class Inteligence : Items
{
    public float speed;



   

    [HideInInspector]
    public PathFinding path;
    [HideInInspector]
    public Rigidbody2D rg;




    public void move_to_aim()
    {

        if (path != null)
        {
            float x = (Mathf.Ceil(gameObject.transform.position.x * (1 / path.cell_size)) / (1 / path.cell_size)) - path.cell_size / 2;
            float y = (Mathf.Ceil(gameObject.transform.position.y * (1 / path.cell_size)) / (1 / path.cell_size)) - path.cell_size / 2;
            int index_x = (int)((x + path.cell_size / 2) / path.cell_size) - 1;
            int index_y = (int)((y + path.cell_size / 2) / path.cell_size) - 1;

            int main_cell_index = index_x + index_y * path.cell_count;


            PathFinding.Grid g = new PathFinding.Grid();




            if (path.grids.Count > 0 && 0 <= main_cell_index && main_cell_index < path.grids.Count)
            {

                g = path.grids[main_cell_index];



                if (g.target)
                {
                    
                }
                else
                {


                    if (g.directions == Directions.None)
                    {

                    }
                    if (g.directions == Directions.up)
                    {
                        rg.velocity = new Vector2(0, speed);
                    }
                    if (g.directions == Directions.down)
                    {
                        rg.velocity = new Vector2(0, -speed);
                    }
                    if (g.directions == Directions.left)
                    {
                        rg.velocity = new Vector2(-speed, 0);
                    }
                    if (g.directions == Directions.right)
                    {

                        rg.velocity = new Vector2(speed, 0);
                    }
                    if (g.directions == Directions.up_right)
                    {
                        rg.velocity = new Vector2(speed, speed);
                    }
                    if (g.directions == Directions.up_left)
                    {
                        rg.velocity = new Vector2(-speed, speed);
                    }
                    if (g.directions == Directions.down_right)
                    {
                        rg.velocity = new Vector2(speed, -speed);
                    }
                    if (g.directions == Directions.down_left)
                    {
                        rg.velocity = new Vector2(-speed, -speed);
                    }
                }

            }
            else
            {
                rg.velocity = new Vector2(0, 0);
            }

        }
        else
        {
            throw new System.Exception("path for navigation is empty");
        }

    }



}
