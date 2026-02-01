using UnityEngine;

[SelectionBase]
public class PressingPlate : MonoBehaviour
{
    [SerializeField] bool _autoMove = true;
	public AudioClip activateSound;
	public AudioClip deactivateSound;

    public Vector3 moveDirection = Vector3.forward;
    public float moveDistance = 2f;
    public float speed = 2f;
	public float delay = 0f;
    public float waitTime = 0.5f;
	public float onWaitTime = -1f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingForward = true;

	private AudioSource audioSource;

    void Start()
    {
		audioSource = GetComponent<AudioSource>();
        startPos = transform.localPosition;
        targetPos = startPos + moveDirection.normalized * moveDistance;

        if (_autoMove)
        {
            StartCoroutine(MovePlate());
        }
    }

    System.Collections.IEnumerator MovePlate()
    {
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
        while (true)
        {
			if (activateSound != null)
			{
				if (movingForward || deactivateSound == null)
				{
					audioSource.PlayOneShot(activateSound);
				}
				else
				{
					audioSource.PlayOneShot(deactivateSound);
				}
			}
            Vector3 destination = movingForward ? targetPos : startPos;

            while (Vector3.Distance(transform.localPosition, destination) > 0.01f)
            {
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    destination,
                    speed * Time.deltaTime
                );
                yield return null;
            }

            if (!_autoMove) break;

			if (movingForward && onWaitTime > 0f)
			{
				yield return new WaitForSeconds(onWaitTime);
			}
			else
			{
				yield return new WaitForSeconds(waitTime);
			}
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