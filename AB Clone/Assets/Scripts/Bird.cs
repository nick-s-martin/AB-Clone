using System.Collections;
using System.Collections.Generic; // you don't need this
using UnityEngine;

// Example namespace, look em up
namespace Scripts.Enemies
{
	public class Bird : MonoBehaviour
	{
		//suffix floats with f
		//look up doubles with d
		[SerializeField]
		float _launchForce = 500f;

		[SerializeField]
		float _maxDragDistance = 5f;

		Vector2 _startPosition;

		[SerializeField]
		Rigidbody2D _rigidbody2D;

		[SerializeField]
		SpriteRenderer _spriteRenderer;

		void Awake()
		{
			//In general just set the references in editor.
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		// Start is called before the first frame update
		void Start()
		{
			_startPosition = _rigidbody2D.position;
			_rigidbody2D.isKinematic = true;
		}

		void OnMouseDown()
		{
			_spriteRenderer.color = Color.red;
		}

		void OnMouseUp()
		{
			var currentPosition = _rigidbody2D.position;
			Vector2 direction = _startPosition - currentPosition;
			direction.Normalize();

			_rigidbody2D.isKinematic = false;
			_rigidbody2D.AddForce(direction * _launchForce);

			_spriteRenderer.color = Color.white;
		}

		void OnMouseDrag()
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 desiredPosition = mousePosition;

			float distance = Vector2.Distance(desiredPosition, _startPosition);
			if (distance > _maxDragDistance)
			{
				Vector2 direction = desiredPosition - _startPosition;
				direction.Normalize();
				desiredPosition = _startPosition + direction * _maxDragDistance;
			}

			if (desiredPosition.x > _startPosition.x)
				desiredPosition.x = _startPosition.x;

			_rigidbody2D.position = desiredPosition;
		}

		// Update is called once per frame
		//delete this
		void Update()
		{

		}

		void OnCollisionEnter2D(Collision2D collision)
		{
			//don't use coroutines
			StartCoroutine(ResetAfterDelay());
		}

		IEnumerator ResetAfterDelay()
		{
			//this is a float.
			yield return new WaitForSeconds(3);
			_rigidbody2D.position = _startPosition;
			_rigidbody2D.isKinematic = true;
			_rigidbody2D.velocity = Vector2.zero;
		}
	}
}
