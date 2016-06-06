using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
	public static int leftScore = 0;
	public static int rightScore = 0;

	private static bool gameOver = false;

	[Range(1, 20)]
	public int maxScore = 3;

	Text scoreText;

	// Use this for initialization
	void Start () {
		scoreText = GetComponent<Text>();
		scoreText.text = leftScore + " " + rightScore;
	}

	float gameTime = 0;

	// Update is called once per frame
	void Update () {
		scoreText.text = leftScore + " " + rightScore;
		if (!gameOver && (leftScore >= maxScore || rightScore >= maxScore))
		{
			gameOver = true;
		}
		else if (gameOver)
		{
			if (Input.GetAxisRaw("Jump") > 0)
			{
				gameOver = false;
				leftScore = rightScore = 0;
				GameObject ball = GameObject.Find("Ball");
				ball.GetComponent<Ball>().resetGame();
				gameTime = gameTime + Time.deltaTime;
			}
		}
		// Atalho para fechar o jogo
		if (Input.GetKey("escape"))
		{
			Application.Quit();
		}
	}

	public static void setGameOver(bool b)
	{
		gameOver = b;
	}

	public static bool isGameOver()
	{
		return gameOver;
	}
}
