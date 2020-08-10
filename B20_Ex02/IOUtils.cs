using System;
using Ex02.ConsoleUtils;

namespace B20_Ex02
{
    internal class IOUtils
    {
        private static bool? s_IsPlayer2OrComputer;
        private int m_UserInputHeight;
        private int m_UserInputWidth;
        private Logic m_GameLogic;

        public IOUtils()
        {
            string player1Name = getPlayerNameInput();
            string player2Name = "Computer";
            s_IsPlayer2OrComputer = isPlayer2();

            if (s_IsPlayer2OrComputer == true)
            {
                 player2Name = getPlayerNameInput();
            }

            getSizeOfBoardInput();
            m_GameLogic = new Logic(s_IsPlayer2OrComputer, m_UserInputHeight, m_UserInputWidth, player1Name, player2Name);
        }

        public string GetPlayer1Name
        {
            get
            {
                return m_GameLogic.GetPlayer1Name;
            }
        }

        public string GetPlayer2Name
        {
            get
            {
                return m_GameLogic.GetPlayer2Name;
            }
        }

        public Logic GetLogic
        {
            get
            {
                return m_GameLogic;
            }
        }

        public int BoardWidth
        {
            get
            {
                return m_UserInputWidth;
            }
        }

        public int BoardHeight
        {
            get
            {
                return m_UserInputHeight;
            }
        }

        public static bool PlayGame()
        {
            IOUtils IO = new IOUtils();
            string playerTurn = IO.GetPlayer1Name;
            bool gameWon = false;
            bool playAnotherGame = false;
            bool? foundMatch;
            IOUtils.PrintBoard(IO.GetLogic.GameBoard);

            while (gameWon == false)
            {
                foundMatch = IO.TurnOutline(playerTurn);
                IO.GetLogic.ChooseNextPlayer(foundMatch, ref playerTurn);
                gameWon = IO.GetLogic.IsGameOver();

                if (gameWon == true)
                {
                    IO.WinningMessage();
                    playAnotherGame = IO.anotherGame();
                }
            }

            return playAnotherGame;
        }

        private void getSizeOfBoardInput()
        {
            Console.WriteLine("Enter Board height between 4 and 6: ");
            string userInputBoardHeight = Console.ReadLine();
            Console.WriteLine("Enter Board width between 4 and 6: ");
            string userInputBoardWidth = Console.ReadLine();
            bool goodInput;
            do
            {
                if(int.TryParse(userInputBoardHeight, out m_UserInputHeight) == false)
                {
                    goodInput = false;
                }
                else
                {
                    if (m_UserInputHeight >= 4 && m_UserInputHeight <= 6)
                    {
                        goodInput = true;
                    }
                    else
                    {
                        goodInput = false;
                    }
                }

                if (int.TryParse(userInputBoardWidth, out m_UserInputWidth) == false)
                {
                    goodInput = false;
                }
                else if (goodInput == true && m_UserInputWidth >= 4 && m_UserInputWidth <= 6)
                {
                    goodInput = true;
                }
                else
                {
                    goodInput = false;
                }

                if(m_UserInputWidth == 5 && m_UserInputHeight == 5)
                {
                    Console.WriteLine("Number of Squares must be even");
                    goodInput = false;
                }

                if (m_UserInputHeight < 4 || m_UserInputHeight > 6)
                {
                    Console.WriteLine("Board height must be between 4 and 6");
                }

                if (m_UserInputWidth < 4 || m_UserInputWidth > 6)
                {
                    Console.WriteLine("Board width must be between 4 and 6");
                }

                if (goodInput == false)
                {
                    Console.WriteLine("Enter Board height between 4 and 6: ");
                    userInputBoardHeight = Console.ReadLine();
                    Console.WriteLine("Enter Board width between 4 and 6: ");
                    userInputBoardWidth = Console.ReadLine();
                }
            }
            while (goodInput == false);
        }

        private string getPlayerNameInput()
        {
            Console.WriteLine("Please enter your name: ");
            string playerName = Console.ReadLine();

            while (playerName.CompareTo(string.Empty) == 0 || playerName[0].CompareTo(' ') == 0)
            {
                Console.WriteLine("invalid name");
                Console.WriteLine("Please enter your name: ");
                playerName = Console.ReadLine();
            }

            return playerName;
        }

        private bool? isPlayer2()
        {
            string playerChoice;
            bool? is2Players;

            Console.WriteLine("To play against a computer enter \"yes\" to play against a person enter anything else: ");
            playerChoice = Console.ReadLine();
            if (playerChoice.CompareTo("yes") == 0)
            {
                is2Players = false;
            }
            else
            {
                is2Players = true;
            }

            return is2Players;
        }

        public static void PrintBoard(Board i_GameBoard)
        {
            char topOfBoard = 'A';
            int numRow = 1;

            Console.Write(" ");
            for (int i = 0; i < i_GameBoard.BoardWidth; i++)
            {
                    Console.Write("   " + topOfBoard);
                    topOfBoard++;
            }

            Console.WriteLine(string.Empty);

            for (int i = 0; i <= i_GameBoard.BoardHeight * 2; i++)
            {
                if (i % 2 == 0)
                {
                    Console.Write("  ");
                    for (int j = 0; j <= i_GameBoard.BoardWidth * 4; j++)
                    {
                        Console.Write("=");
                    }

                    Console.WriteLine(string.Empty);
                }
                else
                {
                    Console.Write(numRow + " |");
                    for(int j = 0; j < i_GameBoard.BoardWidth; j++)
                    {
                        Console.Write(" " + i_GameBoard.ValueToShow(numRow - 1, j ) + " |");
                    }

                    Console.WriteLine(string.Empty);
                    numRow++;
                }
            }
        }

        public bool? TurnOutline(string i_Player)
        {
            bool? isMatch;
            bool? goodInput;
            string firstChoice;
            string secondChoice;

            Console.WriteLine(i_Player + "'s turn:");
            if(i_Player.CompareTo("Computer") != 0)
            {
                do
                {
                    Console.WriteLine("choose a tile to flip");
                    firstChoice = Console.ReadLine();
                    if (firstChoice.CompareTo("Q") == 0)
                    {
                        Environment.Exit(-1);
                    }

                    goodInput = checkUserChoice(firstChoice);
                }
                while (goodInput == false);
                m_GameLogic.UpdateBoard(true, firstChoice[0] - '1', firstChoice[1] - 'A');

                do
                {
                    Console.WriteLine("choose another tile to flip");
                    secondChoice = Console.ReadLine();
                    if (secondChoice.CompareTo("Q") == 0)
                    {
                        Environment.Exit(-1);
                    }

                    goodInput = checkUserChoice(secondChoice);
                }
                while (goodInput == false);

                m_GameLogic.UpdateBoard(true, secondChoice[0] - '1', secondChoice[1] - 'A');
                System.Threading.Thread.Sleep(2000);
                isMatch = m_GameLogic.IsMatch(i_Player, firstChoice[0] - '1', firstChoice[1] - 'A', secondChoice[0] - '1', secondChoice[1] - 'A');

                if (isMatch  == true)
                {
                    m_GameLogic.MakeMemoryBlockViewAble(firstChoice[0] - '1', firstChoice[1] - 'A');
                    m_GameLogic.MakeMemoryBlockViewAble(secondChoice[0] - '1', secondChoice[1] - 'A');
                    Console.WriteLine("It's a match!");
                }
                else
                {
                    m_GameLogic.AddToComputerMemory(firstChoice[0] - '1', firstChoice[1] - 'A');
                    m_GameLogic.AddToComputerMemory(secondChoice[0] - '1', secondChoice[1] - 'A');
                }              
            }
            else
            {
                isMatch = m_GameLogic.ComputerTurn();
            }

            return isMatch;
        }
   
        private bool checkUserChoice(string i_Choice)
        {
            bool validInput = true;

            if (i_Choice.Length == 2)
            {
                if (i_Choice[0] - '0' < 1 || i_Choice[0] - '0' > m_UserInputHeight)
                {
                    validInput = false;
                    Console.WriteLine("row choice isn't valid, choose values between 1 and " + m_UserInputHeight);
                }

                if (i_Choice[1] - 'A' < 0 || i_Choice[1] - 'A' >= m_UserInputWidth)
                {
                    validInput = false;
                    Console.WriteLine("column choice isn't valid, choose values between A and " + (char)(m_UserInputWidth + 'A' - 1));
                }

                if (validInput == true)
                {
                    validInput = m_GameLogic.CanBlockBeFlipped(i_Choice[0] - '1', i_Choice[1] - 'A');
                    if (validInput == false)
                    {
                        Console.WriteLine("Tile is already viewable, choose a different tile.");
                    }
                }
            }
            else
            {
                validInput = false;
                Console.WriteLine("Bad input, input should look like this: RowNumber,ColumnNumber.");
            }

            return validInput;
        }

        public void WinningMessage()
        {
            if (m_GameLogic.GetPlayer1Score != m_GameLogic.GetPlayer2Score)
            {
                if (m_GameLogic.GetPlayer1Score > m_GameLogic.GetPlayer2Score)
                {
                    Console.WriteLine("Congratulations! " + m_GameLogic.GetPlayer1Name + " won!");
                    printScore();
                }
                else
                {
                    Console.WriteLine("Congratulations! " + m_GameLogic.GetPlayer2Name + " won!");
                    printScore();
                }
            }
            else
            {
                Console.WriteLine("It's a Tie!");
                printScore();
            }

            System.Threading.Thread.Sleep(2000);
        }

        private bool anotherGame()
        {
            bool playAnother = false;

            Console.WriteLine("If you wish to play another game enter \"yes\" if you wish to quit enter anything else.");
            string answer = Console.ReadLine();
            if (answer.CompareTo("yes") == 0)
            {
                playAnother = true;
            }

            return playAnother;
        }

        private void printScore()
        {
            Console.WriteLine(m_GameLogic.GetPlayer1Name + " had :" + m_GameLogic.GetPlayer1Score + "points.");
            Console.WriteLine(m_GameLogic.GetPlayer2Name + " had :" + m_GameLogic.GetPlayer2Score + "points.");
        }
    }
}
