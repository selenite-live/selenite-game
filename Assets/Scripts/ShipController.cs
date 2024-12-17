using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipController : MonoBehaviour
{

    public float forwardSpeed, hoverSpeed = 5f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    private float forwardAcceleration = 2.5f;
    public float lookRateSpeed = 90f;
    private Vector2 lookInput, screenCenter, mouseDistance;
    private float rollInput;
    public float rollSpeed = 1f, rollAcceleration = 0.5f;
    public Camera camera;
    private float fov;
    [SerializeField] private Material light;
    [SerializeField] private AudioSource spaceship_engine_sound;
    [SerializeField] private AudioLowPassFilter lowPassFilter;
    public TextMeshPro speedIndicator;
    public TextMeshPro altitudeIndicator;
    public GameObject throttleBar;

    private Vector3 fps_position = new Vector3(0,.5f,0);
    private Vector3 tps_position = new Vector3(0,7,-30);
    // Start is called before the first frame update
    void Start()
    {

        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        fov = camera.fieldOfView;

        camera.transform.localPosition = fps_position;

    }

    // Update is called once per frame
    void Update()
    {

        // SHIP CONTROLS

        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.x;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, -Input.GetAxisRaw("Horizontal"), rollAcceleration * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed, Space.Self);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, activeHoverSpeed * Time.deltaTime);

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);

        if(Input.GetAxisRaw("Boost") != 0){
            forwardSpeed = 90f;

        }else{
            forwardSpeed = 50f;
        }

        // ENGINE SOUND

        spaceship_engine_sound.pitch = 0.8f + (Mathf.Abs(activeForwardSpeed*0.01f));

        //ENGINE LIGHTS

        light.SetColor("_EmissionColor", new Vector4(0,255,255) * ((Mathf.Abs(activeForwardSpeed) * 0.0009f)+ 0.005f));

        // UI Update

        float speed = Mathf.Round(activeForwardSpeed * 3.6f);
        speedIndicator.text = speed.ToString() + "km/h";

        float altitude = Mathf.Round(transform.position.y);
        altitudeIndicator.text = altitude.ToString() + "m";

        throttleBar.transform.localScale = new Vector3((activeForwardSpeed*.01f)+0.1f,1f,1f);

        if(activeForwardSpeed >= 60f){
            speedIndicator.color = new Vector4(255,0,0,255);
        }else{
            speedIndicator.color = new Vector4(0,255,0,255);
        }

        // CAMERA BEHAVIOR

            if(Mathf.Abs(activeForwardSpeed) >= 0f){
                camera.fieldOfView = fov + (activeForwardSpeed/3);
            }else{
                camera.fieldOfView = fov + (activeForwardSpeed/5);
            }

            if(Input.GetKeyDown("c")){

                if(camera.transform.localPosition == fps_position){
                    camera.transform.localPosition = tps_position;
                    spaceship_engine_sound.volume = 1;
                    lowPassFilter.enabled = false;
                }else{
                    camera.transform.localPosition = fps_position;
                    spaceship_engine_sound.volume = 0.9f;
                    lowPassFilter.enabled = true;
                }

            }

    }
}
