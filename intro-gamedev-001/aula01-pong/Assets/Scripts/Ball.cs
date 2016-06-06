using UnityEngine;
using System.Collections;

/**
 * Funcionalidades para movimentação e lógica da Bola
 */
public class Ball : MonoBehaviour
{
	// Range(min, max) é um atributo que determina um alcance para speed
	// Atributos são escritos como um método entre colchetes antes da variável
	[Range(5f, 80f)]
	public float speed = 30f;
	
	// Um atalho para não precisar escrever GetComponent<Rigidbody2D>() o tempo inteiro
	Rigidbody2D rb2d;

	// Posição da bola ao passar pela rede
	Vector2 midPosition;
	private float defaultSpeed;

	// Use this for initialization
	void Start()
	{
		// GetComponent<>() recebe qualquer Component existente nesse GameObject
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = new Vector2(1, 0) * speed;
		
		// Como o usuário pode alterar seu speed no Inspector, 
		// precisamos atualizar o defaultSpeed corretamente no método Start()
		defaultSpeed = speed;
	}


	/**
	 * FixedUpdate() é chamado em intervalos de tempo fixos, e é ideal chamá-lo
	 * sempre que mexer na parte física do jogo
	 */
	void FixedUpdate()
	{
		/**
		 * Queremos resetar a posição e velocidade da bola se o jogo acabar
		 */
		if (Score.isGameOver())
		{
			transform.position = new Vector3(0, 0, 0);
			rb2d.velocity = new Vector2(0, 0);
		}
	}

	public void resetGame()
	{
		rb2d.velocity = new Vector2(1, 0) * speed;
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Goal")
		{
			transform.position = new Vector3(netPosition.x, netPosition.y, 0f);
			rb2d.velocity = new Vector2(netVelocity.x, netVelocity.y);
			// toda vez que encostarmos na raquete, queremos aumentar a velocidade em 1
			// porém não queremos que a bola vá rápido demais
			// Mathf.Clamp(valorAtual, min, max) recebe o valorAtual e verifica se min <= valorAtual <= max
			// e retorna um valor dentro desses limites
			speed = Mathf.Clamp(speed + 1, 1, defaultSpeed);
			if (col.gameObject.name == "RightWall")
			{
				Score.rightScore++;
			}
			else if (col.gameObject.name == "LeftWall")
			{
				Score.leftScore++;
			}
		}

		if (col.gameObject.name == "LeftRacket")
		{
			float vy = directionImpact(transform.position.y, col.transform.position.y, col.collider.bounds.size.y);
			Vector2 dir = new Vector2(1, vy);
			rb2d.velocity = dir * speed;
		}
		if (col.gameObject.name == "RightRacket")
		{
			float vy = directionImpact(transform.position.y, col.transform.position.y, col.collider.bounds.size.y);
			Vector2 dir = new Vector2(-1, vy);
			rb2d.velocity = dir * speed;
		}
	}

	/**
	 * <summary>Essa função subtrai a posição y da bola com a posição y da raquete e divide pela altura da raquete</summary>
	 * ballY posição y da bola
	 * racketY posição y da raquete
	 * racketHeight altura da raquete
	 * <value>um float com variação entre -1 e 1 dependendo da altura em que a bola acertou a raquete com relação ao centro</value>
	 */
	float directionImpact(float ballY, float racketY, float racketHeight)
	{
		return ((ballY - racketY) / racketHeight);
	}

	private Vector2 netVelocity;
	private Vector2 netPosition;

	public void OnTriggerEnter2D(Collider2D col)
	{
		// Verificamos a colisão da bola com a rede para que possamos atualizar sua posição em caso de ponto
		if (col.gameObject.name == "Net")
		{
			netVelocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
			netPosition = new Vector2(transform.position.x, transform.position.y);
		}
	}
}
