using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class Drawer : MonoBehaviour
{
    [SerializeField] private XRSimpleInteractable handle = null;
    [SerializeField] private PhysicsMover mover = null;
    [SerializeField] private Transform start = null;
    [SerializeField] private Transform end = null;

    private Vector3 grabPosition = Vector3.zero;
    private float startingPercentage = 0.0f;
    private float currentPercentage = 0.0f;

    private PhotonView _view;

    void Awake() {
        _view = GetComponent<PhotonView>();
    }
    protected virtual void OnEnable() {
        handle.selectEntered.AddListener(StoreGrabInfo);
    }

    protected virtual void OnDisable() {
        handle.selectEntered.RemoveListener(StoreGrabInfo);
    }

    private void StoreGrabInfo(SelectEnterEventArgs args) {
        startingPercentage = currentPercentage;
        grabPosition = args.interactorObject.transform.position;
    }

    private float FindPercentageDifference() {
        Vector3 handPosition = handle.firstInteractorSelecting.transform.position;
        Vector3 pullDirection = handPosition - grabPosition;
        Vector3 targetDirection = end.position - start.position;

        float length = targetDirection.magnitude;
        targetDirection.Normalize();

        return Vector3.Dot(pullDirection, targetDirection) / length;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(handle.isSelected) {
            float newPercentage = startingPercentage + FindPercentageDifference();
            _view.RPC("UpdateDrawer", RpcTarget.All, newPercentage);
        }
    }

    [PunRPC]
    void UpdateDrawer(float newPercentage) {
        mover.MoveTo(Vector3.Lerp(start.position, end.position, newPercentage));
        currentPercentage = Mathf.Clamp01(newPercentage);
    }
}
