using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MoveDirection
{
    Left, Right, Up, Down, Null
}

public class CleanMovement : MonoBehaviour
{
    #region Component References
    public Rigidbody2D playerRB;
    #endregion

    #region Variables
    public static int speed = 120;
    public MoveDirection directionInput;
    public Vector2 directionVector;
    public MoveDirection currentDirection;
    public bool controlEnabled = false;
    public static int cakesEaten;
    public static int blocksEaten;
    public bool Phase2 = false;
    public GameObject[] Nodes;
    public GameObject infoCanvas;
    public Text uiText;
    public int flip;
    public SpriteRenderer sr;
    #endregion

    public Node nodeScript;

    // Start is called before the first frame update
    void Start()
    {
        controlEnabled = true;
        playerRB = GetComponent<Rigidbody2D>();
        cakesEaten = 0;
        Nodes = GameObject.FindGameObjectsWithTag("Node");
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    private void Update()
    {
        ReadInput();
        DirectionChange();
        
        if (GameManager.state == GameStates.Phase2 && !Phase2)
        {
            foreach (GameObject node in Nodes)
            {
                node.SetActive(false);
                Phase2 = true;
            }
        } //Desactivar nodos

        if (currentDirection == MoveDirection.Left && flip != 1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            sr.flipX = false;
            flip = 1;
        }
        if (currentDirection == MoveDirection.Right && flip != 0)
        {

            transform.eulerAngles = new Vector3(0, 0, 0);
            sr.flipX = true;
            flip = 0;
        }
        if (currentDirection == MoveDirection.Down)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            sr.flipX = false;
            flip = 2;
        }
        if (currentDirection == MoveDirection.Up)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            sr.flipX = false;
            flip = 2;
        }

    }

    private void FixedUpdate()
    {
        SetDirectionVector();
        MovePlayer();
        DirectionCheck();
    }

    

    #region Update Functions
    void DirectionChange()
    {
        if (GameManager.state != GameStates.Phase2 && GameManager.state != GameStates.End)
        {
            //Change Direction
            if (directionInput == MoveDirection.Left)
            {
                if (currentDirection == MoveDirection.Right)
                {
                    currentDirection = MoveDirection.Left;
                }
            }

            if (directionInput == MoveDirection.Right)
            {
                if (currentDirection == MoveDirection.Left)
                {
                    currentDirection = MoveDirection.Right;
                }
            }

            if (directionInput == MoveDirection.Down)
            {
                if (currentDirection == MoveDirection.Up)
                {
                    currentDirection = MoveDirection.Down;
                }
            }

            if (directionInput == MoveDirection.Up)
            {
                if (currentDirection == MoveDirection.Down)
                {
                    currentDirection = MoveDirection.Up;
                }
            }
        }
        else if (GameManager.state == GameStates.Phase2 || GameManager.state == GameStates.End)
        {
            if (directionInput == MoveDirection.Left)
            {
                currentDirection = MoveDirection.Left;
            }
            if (directionInput == MoveDirection.Right)
            {
                currentDirection = MoveDirection.Right;
            }
            if (directionInput == MoveDirection.Up)
            {
                currentDirection = MoveDirection.Up;
            }
            if (directionInput == MoveDirection.Down)
            {
                currentDirection = MoveDirection.Down;
            }
        }
    }

    void ReadInput()
    {
        if (controlEnabled)
        {
            if (Input.GetButtonDown("Left"))
            {
                directionInput = MoveDirection.Left;
            }
            if (Input.GetButtonDown("Right"))
            {
                directionInput = MoveDirection.Right;
            }
            if (Input.GetButtonDown("Up"))
            {
                directionInput = MoveDirection.Up;
            }
            if (Input.GetButtonDown("Down"))
            {
                directionInput = MoveDirection.Down;
            }
        }
    }
    #endregion

    #region FixedUpdate Functions

    void SetDirectionVector()
    {
        if (currentDirection == MoveDirection.Left)
        {
            directionVector = new Vector2(-1, 0);
        }
        else if (currentDirection == MoveDirection.Up)
        {
            directionVector = new Vector2(0, 1);
        }
        else if (currentDirection == MoveDirection.Down)
        {
            directionVector = new Vector2(0, -1);
        }
        else if (currentDirection == MoveDirection.Right)
        {
            directionVector = new Vector2(1, 0);
        }
        else if (currentDirection == MoveDirection.Null)
        {
            directionVector = new Vector2(0, 0);
        }
    }
    void DirectionCheck()
    {
        #region Direction Change
        if (nodeScript != null)
        {
            //Change Direction
            if (directionInput == MoveDirection.Up) // Up
            {

                if (nodeScript.up)
                {
                    currentDirection = MoveDirection.Up;
                }
                else
                {
                    currentDirection = MoveDirection.Null;
                }
            }

            if (directionInput == MoveDirection.Down) // Down
            {
                if (nodeScript.down)
                {
                    currentDirection = MoveDirection.Down;
                }
                else
                {
                    currentDirection = MoveDirection.Null;
                }
            }

            if (directionInput == MoveDirection.Right) // Right
            {
                if (nodeScript.right)
                {
                    currentDirection = MoveDirection.Right;
                }
                else
                {
                    currentDirection = MoveDirection.Null;
                }
            }

            if (directionInput == MoveDirection.Left)
            {
                if (nodeScript.left)
                {
                    currentDirection = MoveDirection.Left;
                }
                else
                {
                    currentDirection = MoveDirection.Null;
                }
            }
        }
        #endregion
    }
    void MovePlayer()
    {
        playerRB.velocity = directionVector * speed * Time.fixedDeltaTime;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Node")
        {
            if (nodeScript == null)
            {
                nodeScript = other.GetComponent<Node>();
            }
            else if (nodeScript != null)
            {
                nodeScript = null;
                nodeScript = other.GetComponent<Node>();
            }

            transform.position = other.transform.position;
        }

        if (GameManager.state == GameStates.Phase1)
        {
            if (other.tag == "Cake")
            {
                cakesEaten++;
                other.gameObject.SetActive(false);
            }
        }
        else if (GameManager.state == GameStates.Phase2)
        {
            if (other.tag == "Block")
            {
                blocksEaten++;
                other.gameObject.SetActive(false);
            }
        }

        else if(GameManager.state == GameStates.End)
        {
            if (other.tag == "Candle1")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "El diseño de Pac-man está basado en en la forma de una pizza a la que le falta una porción.";
            }
            else if (other.tag == "Candle2")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "El juego iba a ser bautizado como Pakkuman por la onomatopeya paku paku (persona comiendo). El nombre pensado inicialmente para introducirlo en Estados Unidos fue Puck-man pero finalmente se cambió a Pac-man.";
            }
            else if (other.tag == "Candle3")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "El juego fue diseñado para llegar a un target/público/sector femenino que fue un factor determinante a la hora de escoger el diseño de Pac-man y los nombres de los fantasmas.";
            }
            else if (other.tag == "Candle4")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "Tiene un Récord Guiness al videojuego arcade con más máquinas en todo el mundo: 293 822.";
            }
            else if (other.tag == "Candle5")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "Pac-man ha protagonizado dos series (en 1982 y 2013). También ha hecho varios cameos en ‘Los Simpsons’, ‘Friends’, ‘Futurama’ y ‘Los Muppets’ entre otros.";
            }
            else if (other.tag == "Candle6")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "El puntaje máximo que se puede obtener es 3 333 360 alcanzado po primera por Billy Mitchell en 1999.";
            }
            else if (other.tag == "Candle7")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "«Primero, que no se dediquen solo a jugar a videojuegos, sino que tengan otras experiencias, pues luego las podrán aplicar a los juegos. Segundo, que no aspiren a hacer un juego que solo les guste a ellos, sino que piensen en quién va a jugarlo» son los dos consejos de Toru Iwatani (el creador de Pac-man) da a sus alumnos de la Universidad Politécnica de Tokio.";
            }
            else if (other.tag == "Candle8")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "Toru Iwatani (el creador de Pac-man) ha participado en proyectos de videojuegos para ayudar a personas discapacitadas o para rehabilitaciones.";
            }
            else if (other.tag == "Candle9")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "El juego tiene 255 niveles en total.";
            }
            else if (other.tag == "Candle10")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "Existe un truco para despistar a los fantasmas que consiste en girar en una dirección e inmediatamente después en la contraria.";
            }
            else if (other.tag == "Candle11")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "A lo largo del juego, Pac-Man puede encontrar diversos premios: cereza, fresa, naranja, manzana, uvas, galaxian, campanay llave, dependiendo del nivel";
            }
            else if (other.tag == "Candle12")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "Blinky es el fantasma rojo. Persigue directamente a Pac-man y su velocidad aumenta cuantos más puntos come Pac-man. ";
            }
            else if (other.tag == "Candle13")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "Inky es el fantasma azul. El juego calcula la distancia en línea recta entre Blinky y Pac-man y lo gira 180 grados, así que Inky siempre colabora con Blinky para acorralar a Pac-man ";
            }
            else if (other.tag == "Candle14")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "Pinky es el fantasma rosa. Trata de atrapar a Pac-man por el frente mientras Blinky lo persigue por detrás. ";
            }
            else if (other.tag == "Candle15")
            {
                directionInput = MoveDirection.Null;
                currentDirection = MoveDirection.Null;
                controlEnabled = false;
                infoCanvas.SetActive(true);
                other.gameObject.SetActive(false);
                uiText.text = "Clyde es el fantasma naranja. Persigue a Pac-man directamente (igual que Blinky) pero huye cuando se acerca demasiado a él moviéndose a la esquina inferior izquierda del laberinto.";
            }

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Node")
        {
            nodeScript = null;
        }
    }

    public void ResumePlay()
    {
        infoCanvas.SetActive(false);
        controlEnabled = true;
    }
}
