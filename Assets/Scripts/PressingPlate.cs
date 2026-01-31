using UnityEngine;

public class PressingPlate : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.forward;
    public float moveDistance = 2f;
    public float speed = 2f;
    public float waitTime = 0.5f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingForward = true;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + moveDirection.normalized * moveDistance;
        StartCoroutine(MovePlate());
    }

    System.Collections.IEnumerator MovePlate()
    {
        while (true)
        {
            Vector3 destination = movingForward ? targetPos : startPos;

            while (Vector3.Distance(transform.position, destination) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    destination,
                    speed * Time.deltaTime
                );
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
            movingForward = !movingForward;
        }
    }

    //[SerializeField] Vector3 _position1;
    //[SerializeField] Vector3 _position2;
    //[SerializeField] float _velocity;

    //bool forward;

    //private void Awake()
    //{
    //    InvokeRepeating(nameof(SwitchDirections), 1f, 3f);
    //}

    //private void Update()
    //{
    //    if (forward)
    //    {
    //        if (_position1)
    //        transform.position += forward ? Vector3.forward : Vector3.back * Time.deltaTime * _velocity;
    //    }
    //    else
    //    {
    //        transform.position += forward ? Vector3.forward : Vector3.back * Time.deltaTime * _velocity;
    //    }
    //}

    //void SwitchDirections() 
    //{
    //    forward = !forward;
    //}
}