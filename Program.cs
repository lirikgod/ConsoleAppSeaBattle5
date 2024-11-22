using System;

class Program
{
    const int gridSize = 10; // Размер поля
    const int shipCount = 10; // Количество кораблей на каждого игрока

    static void Main(string[] args)
    {
        char[,] player1Grid = new char[gridSize, gridSize];
        char[,] player2Grid = new char[gridSize, gridSize];
        char[,] player1Radar = new char[gridSize, gridSize];
        char[,] player2Radar = new char[gridSize, gridSize];

        // Инициализация сеток
        InitializeGrid(player1Grid);
        InitializeGrid(player2Grid);
        InitializeGrid(player1Radar);
        InitializeGrid(player2Radar);

        Random random = new Random();
        PlaceShips(player1Grid, random);
        PlaceShips(player2Grid, random);

        Console.WriteLine("Добро пожаловать в Морской бой!");
        bool gameOver = false;
        int currentPlayer = 1;

        while (!gameOver)
        {
            Console.Clear();
            Console.WriteLine($"Ход игрока {currentPlayer}");
            Console.WriteLine("Ваше поле:");
            PrintGrid(currentPlayer == 1 ? player1Grid : player2Grid);
            Console.WriteLine("\nРадар соперника:");
            PrintGrid(currentPlayer == 1 ? player1Radar : player2Radar);

            Console.WriteLine("\nВведите координаты для атаки (строка и столбец через пробел):");
            string[] input = Console.ReadLine()?.Split(' ');
            if (input == null || input.Length != 2 ||
                !int.TryParse(input[0], out int row) ||
                !int.TryParse(input[1], out int col) ||
                row < 0 || row >= gridSize || col < 0 || col >= gridSize)
            {
                Console.WriteLine("Неверный ввод. Нажмите любую клавишу, чтобы попробовать ещё раз.");
                Console.ReadKey();
                continue;
            }

            char[,] enemyGrid = currentPlayer == 1 ? player2Grid : player1Grid;
            char[,] currentRadar = currentPlayer == 1 ? player1Radar : player2Radar;

            if (currentRadar[row, col] == 'X' || currentRadar[row, col] == 'O')
            {
                Console.WriteLine("Вы уже стреляли сюда. Нажмите любую клавишу, чтобы попробовать ещё раз.");
                Console.ReadKey();
                continue;
            }

            // Проверка попадания
            if (enemyGrid[row, col] == 'S')
            {
                Console.WriteLine("Попадание!");
                enemyGrid[row, col] = 'X';
                currentRadar[row, col] = 'X';
            }
            else
            {
                Console.WriteLine("Мимо!");
                currentRadar[row, col] = 'O';
            }

            Console.WriteLine("Нажмите любую клавишу, чтобы передать ход.");
            Console.ReadKey();

            // Проверка на победу
            if (CheckWin(enemyGrid))
            {
                Console.Clear();
                Console.WriteLine($"Игрок {currentPlayer} выиграл! Поздравляем!");
                gameOver = true;
            }

            // Передача хода другому игроку
            currentPlayer = currentPlayer == 1 ? 2 : 1;
        }
    }

    static void InitializeGrid(char[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
            for (int j = 0; j < grid.GetLength(1); j++)
                grid[i, j] = '.';
    }

    static void PrintGrid(char[,] grid)
    {
        Console.Write("  ");
        for (int i = 0; i < grid.GetLength(1); i++)
            Console.Write($"{i} ");
        Console.WriteLine();

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            Console.Write($"{i} ");
            for (int j = 0; j < grid.GetLength(1); j++)
                Console.Write($"{grid[i, j]} ");
            Console.WriteLine();
        }
    }

    static void PlaceShips(char[,] grid, Random random)
    {
        int placedShips = 0;
        while (placedShips < shipCount)
        {
            int row = random.Next(gridSize);
            int col = random.Next(gridSize);

            if (grid[row, col] == '.')
            {
                grid[row, col] = 'S';
                placedShips++;
            }
        }
    }

    static bool CheckWin(char[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
            for (int j = 0; j < grid.GetLength(1); j++)
                if (grid[i, j] == 'S')
                    return false;
        return true;
    }
}
