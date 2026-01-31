using UnityEngine;

public class PressingPlate : MonoBehaviour
{
    [SerializeField] bool _autoMove = true;

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

        if (_autoMove)
        {
            StartCoroutine(MovePlate());
        }
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

            if (!_autoMove) break;

            yield return new WaitForSeconds(waitTime);
            movingForward = !movingForward;
        }
    }

    public void OnSwitchToggled(bool on) 
    {
        if (_autoMove) return;

        movingForward = on;
        StartCoroutine(MovePlate());
    }
}