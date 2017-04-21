using UnityEngine;

public class ZombieScript : MonoBehaviour {

    public float MoveSpeed;
    public Animation zAnim;
    public bool z_isDead;
    public bool z_isAttacking;

	// Use this for initialization
	void Start () {
        z_isDead = false;
        z_isAttacking = false;
        zAnim = GetComponent<Animation>();
        zAnim.Play("walk");
    }
	
	// Update is called once per frame
	void Update () {
        if (!z_isDead && !z_isAttacking)
        {
            Vector3 distance = transform.position - Camera.main.transform.position;
            transform.LookAt(new Vector3(Camera.main.transform.position.x, 0.0f, Camera.main.transform.position.z));
        
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
	}

    public void ZombieDead(Collider col)
    {
        if (z_isDead == true)
        {
            //GetComponent<CapsuleCollider>().enabled = false;

            col.GetComponent<Animation>().Stop("attack");
            col.GetComponent<Animation>().Play("back_fall");
            
            z_isAttacking = false;
            Destroy(col.gameObject, 1.10f);
            
        }
        z_isDead = false;
    }
}
