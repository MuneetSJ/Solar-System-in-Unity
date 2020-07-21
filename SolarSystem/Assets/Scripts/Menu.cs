using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    private string planet;


    private void Start()
    {
            Time.timeScale = 0f;
    }

    public void changeSelction(int option)
    {
        
        switch (option)
        {
            case 0:
                planet = "";
                GameObject.Find("Planet Text").GetComponent<TextMeshProUGUI>().text = planet;
                break;
            case 1:
                planet = "Mercury";
                GameObject.Find("Planet Text").GetComponent<TextMeshProUGUI>().text = planet;
                break;
            case 2:
                planet = "Venus";
                GameObject.Find("Planet Text").GetComponent<TextMeshProUGUI>().text = planet;
                break;
            case 3:
                planet = "Earth";
                GameObject.Find("Planet Text").GetComponent<TextMeshProUGUI>().text = planet;
                break;
            case 4:
                planet = "Mars";
                GameObject.Find("Planet Text").GetComponent<TextMeshProUGUI>().text = planet;
                
                break;
            default:
                break;
        }
    }
    public void play()
    {
        Destroy(gameObject);
        Time.timeScale = 1f;
        foreach (CelestialBody currBody in CelestialBody.bodies.ToList())
        {
            Rigidbody currRB = currBody.body;
            //Debug.Log("The planet " + currRB.name + " is going at this speed =>" + currBody.velocityMultiplier);
            Vector3 initialVelocity = currRB.transform.forward * currBody.velocityMultiplier;
            currRB.AddForce(initialVelocity, ForceMode.VelocityChange);
            LineRenderer line = currBody.gameObject.GetComponentInChildren<LineRenderer>();
            line.enabled = false;
        }
    }

    public void SetVelocity(string inputVelocity)
    {
        CelestialBody changeVel;
        float velocity;
        foreach (CelestialBody celBody in CelestialBody.bodies.ToList())
        {
            if (celBody.name == planet)
            {
                changeVel = celBody;
                if (float.TryParse(inputVelocity, out velocity))
                {
                    celBody.velocityMultiplier = velocity;
                }
            }
        }
    }
}