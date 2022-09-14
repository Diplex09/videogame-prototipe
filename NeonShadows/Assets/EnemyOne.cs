using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : MonoBehaviour
{

    public float rangeAlert;
    public LayerMask playerLayer;
    public bool stayActive;
    public Transform player;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stayActive = Physics.CheckSphere(transform.position,rangeAlert,playerLayer);

        if(stayActive) {

            //transform.LookAt(new Vector3(player.position.x, player.position.y, player.position.z));

            // Para que solamente mueva la mirada en Y
            Vector3 playerPos = new Vector3(player.position.x, player.position.y, player.position.z);
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);

            
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,rangeAlert);
    }
}
