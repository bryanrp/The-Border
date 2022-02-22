using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rope : MonoBehaviour
    {
        private static GameManager _gameManager;
        private List<HingeJoint2D> _rope;
        [SerializeField] private GameObject _ropeSegment;
        private float _ropeSegmentLength;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _ropeSegmentLength = _ropeSegment.transform.localScale.x;
        }

        // Update is called once per frame
        void Update()
        {
            if (_gameManager.GetActiveType() != -1)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    if (_rope == null) CreateRope();
                    else DestroyRope();
                }
            }
        }

        private void CreateRope()
        {
            int activeType = _gameManager.GetActiveType();
            Player playerActive = _gameManager.GetPlayer(activeType);
            Player playerInactive = _gameManager.GetPlayer(1 - activeType);

            Vector2 difference = playerInactive.transform.position - playerActive.transform.position;
            Vector2 direction = difference.normalized;
            float distance = difference.magnitude;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            Debug.Log("Rope angle: " + angle);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            _rope = new List<HingeJoint2D>();
            for (int i = 1; i * _ropeSegmentLength <= distance; i++)
            {
                Vector2 position = (Vector2)playerActive.transform.position + _ropeSegmentLength * (i - 0.5f) * direction;
                HingeJoint2D ropeSegment = Instantiate(_ropeSegment, position, rotation).GetComponent<HingeJoint2D>();
                _rope.Add(ropeSegment);
            }

            Debug.Log("Rope length: " + _rope.Count + ", with distance: " + distance);

            HingeJoint2D playerActiveHinge = playerActive.GetComponent<HingeJoint2D>();
            HingeJoint2D playerInactiveHinge = playerInactive.GetComponent<HingeJoint2D>();
            playerActiveHinge.enabled = true;
            // playerInactiveHinge.enabled = true;
            playerActiveHinge.connectedBody = _rope[0].GetComponent<Rigidbody2D>();
            // playerInactiveHinge.connectedBody = _rope[_rope.Count - 1].GetComponent<Rigidbody2D>();
            for (int i = 0; i < _rope.Count; i++)
            {
                if (i + 1 < _rope.Count)
                {
                    _rope[i].connectedBody = _rope[i + 1].GetComponent<Rigidbody2D>();
                }
                else
                {
                    _rope[i].connectedBody = playerInactive.GetComponent<Rigidbody2D>();
                }
            }

            /*
            DistanceJoint2D playerActiveDistance = playerActive.GetComponent<DistanceJoint2D>();
            DistanceJoint2D playerInactiveDistance = playerInactive.GetComponent<DistanceJoint2D>();
            playerActiveDistance.enabled = true;
            playerInactiveDistance.enabled = true;
            playerActiveDistance.connectedBody = playerInactive.GetComponent<Rigidbody2D>();
            playerInactiveDistance.connectedBody = playerActive.GetComponent<Rigidbody2D>();
            */
        }

        private void DestroyRope()
        {
            foreach (HingeJoint2D ropeSegment in _rope)
            {
                Destroy(ropeSegment.gameObject);
            }
            _rope = null;

            int activeType = _gameManager.GetActiveType();
            HingeJoint2D playerActiveHinge = _gameManager.GetPlayer(activeType).GetComponent<HingeJoint2D>();
            HingeJoint2D playerInactiveHinge = _gameManager.GetPlayer(1 - activeType).GetComponent<HingeJoint2D>();
            playerActiveHinge.connectedBody = null;
            playerInactiveHinge.connectedBody = null;
            playerActiveHinge.enabled = false;
            playerInactiveHinge.enabled = false;

            DistanceJoint2D playerActiveDistance = playerActiveHinge.GetComponent<DistanceJoint2D>();
            DistanceJoint2D playerInactiveDistance = playerInactiveHinge.GetComponent<DistanceJoint2D>();
            playerActiveDistance.enabled = false;
            playerInactiveDistance.enabled = false;
            playerActiveDistance.connectedBody = null;
            playerInactiveDistance.connectedBody = null;
        }
    }
}