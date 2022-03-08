using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    public float toastingTime = 0;
    public Temperature _temp = null;
    private Transform _transform = null;
    private Seasonable _seasoning = null;
    private MeshRenderer _mesh = null;
    private HeatingElement h = null;

    public Transform start = null;
    public Transform end = null;

    //public Material startMaterial = null;

    //public Material endMaterial = null;

    private AudioSource _audio = null;
    private ParticleSystem _smoke = null;

    void Awake() {
        _temp = GetComponentInParent<Temperature>();
        _transform = GetComponent<Transform>();
        _mesh = GetComponent<MeshRenderer>();
        _seasoning = GetComponentInParent<Seasonable>();
        _audio = GetComponent<AudioSource>();
        _smoke = GetComponentInChildren<ParticleSystem>();
        _mesh.enabled = false;
        _smoke.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        h = _temp.heater;
        if (h != null && _mesh.enabled == true && h.s.val == 1) {
            _audio.Play();
            _smoke.Play();
            toastingTime += Time.deltaTime;
        } else if (_audio.isPlaying) {
            _audio.Stop();
            _smoke.Stop();
        }
        if(_mesh.enabled == false && _seasoning.gruyere > 5) {
            //_mesh.material = startMaterial;
            _mesh.enabled = true;
        }
        if (toastingTime > 0) {
            float percentDone = Mathf.Min(toastingTime / 10, 1);
            _transform.localScale = Vector3.Lerp(start.localScale, end.localScale, percentDone);
            _transform.position = Vector3.Lerp(start.position, end.position, percentDone);
            //_mesh.material.Lerp(startMaterial, endMaterial, percentDone);
        }
    }
}
