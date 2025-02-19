using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;
    public Animator A;
    public Camera cam;

    public float Speed = 0.01f;

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0) // Movement
        {
            A.Play("Walk");
            
        }
        else
        {
            A.Play("Idle");
        }

        Vector3 Move = new Vector3(h, 0, v) * Speed;
        Controller.Move(Move);

        Plane playerplane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist;
        if (playerplane.Raycast(ray, out hitdist))
        {
            Vector3 targetpoint = ray.GetPoint(hitdist);
            Quaternion targetrotation = Quaternion.LookRotation(targetpoint - transform.position);
            this.transform.rotation = (Quaternion.Slerp(transform.rotation, targetrotation, 50f * Time.deltaTime));
        }


        

    }


    private void LateUpdate()
    {
        Vector3 targetposition = transform.position + new Vector3(0, 69, 0);
        Vector3 newPosition = Vector3.MoveTowards(cam.transform.position, targetposition, 150f * Time.deltaTime);
        cam.transform.position = newPosition;
    }


}
