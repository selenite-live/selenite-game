using UnityEngine;

public class MouseVisibilityManager : MonoBehaviour
{
    // Lorsque tu veux masquer la souris en jeu
    void Start()
    {
        HideMouseInGame();
    }

    void Update()
    {
        // Tu peux ajouter des conditions pour réafficher la souris si nécessaire (par exemple, quand une UI est ouverte)
        if (Input.GetKeyDown(KeyCode.Escape))  // Exemple : si on appuie sur échap, on rétablit la souris
        {
            ShowMouse();
        }
    }

    // Méthode pour masquer la souris et la verrouiller au centre
    public void HideMouseInGame()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Verrouille la souris au centre
        Cursor.visible = false;  // Rend la souris invisible
    }

    // Méthode pour afficher la souris et la déverrouiller
    public void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;  // Déverrouille la souris
        Cursor.visible = true;  // Affiche la souris
    }
}
