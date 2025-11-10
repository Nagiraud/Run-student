using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


// Gére les mouvement du professeur, et son champs de vision
[RequireComponent(typeof(NavMeshAgent))]
public class TeacherController : MonoBehaviour
{
    [Header("Déplacement")]
    public GameObject[] listePosition;
    public float speed;
    NavMeshAgent m_Agent;

    [Header("Vision")]
    [Range(0, 360)] public float angleVision;
    public float DistanceVision;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        if (m_Agent.remainingDistance <= m_Agent.stoppingDistance) //attend que l'agent termine son déplacement
        {

            // Choix d'un point de patrouille
            int choosen=Random.Range(0, listePosition.Length);
            m_Agent.speed = speed;
            m_Agent.SetDestination(listePosition[choosen].transform.position);
        }
        Look();
    }

    // Vision du professeur
    void Look()
    {
        Collider[] target = Physics.OverlapSphere(transform.position, DistanceVision); // tout les objets à moins de DistaceVision du joueur
        foreach (Collider col in target) {
            if (col.tag == "Player")
            {
                float signedAngle = Vector3.Angle( // angle du joueur par rapport au centre de vision du professeur
                    transform.forward,
                    col.transform.position - transform.position);

                if (Mathf.Abs(signedAngle) < angleVision / 2) { // si il se trouve dans l'angle de vision du professeur
                    col.GetComponent<PlayerController>().GetCaught();
                }
            }
         }
    }

    // Debug : affiche la zone de détéction du joueur
    private void OnDrawGizmos()
    {
        Handles.color = new Color(0, 1, 0, 0.3f);
        Handles.DrawSolidArc(transform.position,
            transform.up,
            Quaternion.AngleAxis(-angleVision/2f,transform.up)*transform.forward,// orientation de l'angle de vue devant le professeur (autant à gauche et à droite)
            angleVision,
            DistanceVision);
    }
}
