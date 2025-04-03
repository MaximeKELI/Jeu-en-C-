using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics; // Ajout important pour Vector2

class SnakeGame
{
    static void Main()
    {
        // Configuration
        const int screenWidth = 800;
        const int screenHeight = 600;
        const int cellSize = 20;
        int fps = 10;

        // Initialisation
        Raylib.InitWindow(screenWidth, screenHeight, "Snake en C#");
        Raylib.SetTargetFPS(fps);

        // Serpent
        List<Vector2> snake = new List<Vector2>();
        snake.Add(new Vector2(screenWidth / 2, screenHeight / 2));
        Vector2 direction = new Vector2(cellSize, 0);

        // Nourriture
        Random rand = new Random();
        Vector2 food = new Vector2(
            rand.Next(0, screenWidth / cellSize) * cellSize,
            rand.Next(0, screenHeight / cellSize) * cellSize
        );

        bool gameOver = false;
        int score = 0;

        // Boucle de jeu
        while (!Raylib.WindowShouldClose() && !gameOver)
        {
            // Contrôles
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) && direction.X == 0)
                direction = new Vector2(cellSize, 0);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT) && direction.X == 0)
                direction = new Vector2(-cellSize, 0);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP) && direction.Y == 0)
                direction = new Vector2(0, -cellSize);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) && direction.Y == 0)
                direction = new Vector2(0, cellSize);

            // Mouvement
            Vector2 newHead = new Vector2(
                snake[0].X + direction.X,
                snake[0].Y + direction.Y
            );

            // Collisions
            if (newHead.X < 0 || newHead.X >= screenWidth || 
                newHead.Y < 0 || newHead.Y >= screenHeight)
                gameOver = true;

            foreach (var segment in snake)
                if (newHead.X == segment.X && newHead.Y == segment.Y)
                    gameOver = true;

            // Manger
            if (newHead.X == food.X && newHead.Y == food.Y)
            {
                food = new Vector2(
                    rand.Next(0, screenWidth / cellSize) * cellSize,
                    rand.Next(0, screenHeight / cellSize) * cellSize
                );
                score += 10;
                fps += 1;
                Raylib.SetTargetFPS(fps);
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            snake.Insert(0, newHead);

            // Dessin
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            // Grille
            for (int x = 0; x < screenWidth; x += cellSize)
                Raylib.DrawLine(x, 0, x, screenHeight, Color.DARKGRAY);
            for (int y = 0; y < screenHeight; y += cellSize)
                Raylib.DrawLine(0, y, screenWidth, y, Color.DARKGRAY);

            // Serpent
            foreach (var segment in snake)
                Raylib.DrawRectangle((int)segment.X, (int)segment.Y, cellSize, cellSize, Color.GREEN);

            // Nourriture
            Raylib.DrawRectangle((int)food.X, (int)food.Y, cellSize, cellSize, Color.RED);

            // Score
            Raylib.DrawText($"Score: {score}", 10, 10, 20, Color.WHITE);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
        Console.WriteLine($"Game Over! Score final: {score}");
    }
}