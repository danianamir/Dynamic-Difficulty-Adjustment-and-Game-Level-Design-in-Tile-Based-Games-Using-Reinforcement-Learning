
using UnityEngine;

public class Items : MonoBehaviour
{




    public int type;

    [HideInInspector]
    public bool play_phase = false;


    public void FixedUpdate()
    {
        play_phase = !Manager.Instance.in_level_desing;
    }





}
