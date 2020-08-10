using System;
using System.Collections.Generic;
using Ex02.ConsoleUtils;

namespace B20_Ex02
{
    internal class Logic
    {
        private const int ComputerFullMemoryCapacity = 16;
        internal string m_Turn;
        internal Board m_GameBoard;
        private static bool? s_IsSecondPlayer;
        internal int m_UnturnedTiles;
        public Player m_Player1;
        public Player m_Player2;
        internal List<AIMemoryBlock<char>> m_ComputerMemory;

        public Logic(bool? i_IsSecondPlayer, int i_Height, int i_Width, string i_Player1Name, string i_Player2Name)
        {         
            m_GameBoard = new Board(i_Width, i_Height);
            m_UnturnedTiles = i_Height * i_Width;
            m_Turn = "Player1";
            s_IsSecondPlayer = i_IsSecondPlayer;
            m_ComputerMemory = new List<AIMemoryBlock<char>>(ComputerFullMemoryCapacity);
            m_Player1 = new Player(i_Player1Name);
            m_Player2 = new Player(i_Player2Name);
        }
        
        public static bool? IsSecondPlayer
        {
            get
            {
                return s_IsSecondPlayer;
            }
        }

        public string GetPlayer1Name
        {
            get
            {
                return m_Player1.GetPlayerName;
            }
        }

        public string GetPlayer2Name
        {
            get
            {
                return m_Player2.GetPlayerName;
            }
        }

        public int GetPlayer1Score
        {
            get
            {
                return m_Player1.GetPlayerScore;
            }
        }

        public int GetPlayer2Score
        {
            get
            {
                return m_Player2.GetPlayerScore;
            }
        }

        public Board GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public bool CanBlockBeFlipped(int i_Row, int i_Col)
        {
            return !m_GameBoard[i_Row, i_Col].IsViewable; 
        }

        internal bool? IsMatch(string i_Player, int i_FirstChoiceRow, int i_FirstChoiceCol, int i_SecondChoiceRow, int i_SecondChoiceCol)
        {
            bool? doTheTilesMatch;

            if (m_GameBoard[i_FirstChoiceRow, i_FirstChoiceCol] == m_GameBoard[i_SecondChoiceRow, i_SecondChoiceCol])
            {
                doTheTilesMatch = true;
                if (i_Player.CompareTo(m_Player1.GetPlayerName) == 0)
                {
                    m_Player1.RaisePlayerScore();
                }
                else
                {
                    m_Player2.RaisePlayerScore();
                }

                m_UnturnedTiles -= 2;
            }
            else
            {
                UpdateBoard(false, i_FirstChoiceRow, i_FirstChoiceCol);
                UpdateBoard(false, i_SecondChoiceRow, i_SecondChoiceCol);
                doTheTilesMatch = false;
            }

            return doTheTilesMatch;
        }

        internal bool IsGameOver()
        {
            bool gameOver;

            if(m_UnturnedTiles == 0)
            {
                gameOver = true;
            }
            else
            {
                gameOver = false;
            }

            return gameOver;
        }

        internal void ChooseNextPlayer(bool? i_FoundMatch, ref string i_CurrentPlayer)
        {
            if (i_FoundMatch == false)
            {
                if (i_CurrentPlayer == m_Player1.GetPlayerName)
                {
                    i_CurrentPlayer = m_Player2.GetPlayerName;
                }
                else
                {
                    i_CurrentPlayer = m_Player1.GetPlayerName;
                }
            }
        }
            
        internal bool ComputerTurn()
        {
            Random rand = new Random();
            int row1 = rand.Next(0, m_GameBoard.BoardHeight);
            int col1 = rand.Next(0, m_GameBoard.BoardWidth );
            int row2 = rand.Next(0, m_GameBoard.BoardHeight);
            int col2 = rand.Next(0, m_GameBoard.BoardWidth);
            int placeInMemory = 0;

            bool foundMach = lookForMatchWithLastFlippedCard();
            if (foundMach == false)
            {
                foundMach = findRandTileAndLookInMemory(ref row1, ref col1, ref placeInMemory, ref row2, ref col2, rand);

                if (foundMach == false)
                {                    
                    findRandValidTile(ref row2, ref col2, rand);
                    UpdateBoard(true, row2, col2);
                    System.Threading.Thread.Sleep(2000);

                    if (IsMatch("Computer", row1, col1, row2, col2) == true)
                    {
                        foundMach = true;
                        UpdateBoard(true, row2, col2);
                    }
                    else
                    {
                        foundMach = false;
                        updateUnMatchedAfterRandom(row1, col1, row2, col2);
                    }
                }
                else
                {
                    foundMach = true;
                    foundMatchWithMemory(placeInMemory, row2, col2);
                }
            }

            return foundMach;
        }

        internal void AddToComputerMemory(int i_Row, int i_Col)
        {
            AIMemoryBlock<char> newMemory = new AIMemoryBlock<char>(m_GameBoard[i_Row, i_Col].BlockValue, i_Row, i_Col);
            if (checkIfAlreadyInMem(newMemory) == false)
            {
                if (m_ComputerMemory.Count == m_ComputerMemory.Capacity)
                {
                    m_ComputerMemory.RemoveAt(0);
                    newMemory.IsViewAble = false;
                    m_ComputerMemory.Add(newMemory);
                }
                else
                {
                    newMemory.IsViewAble = false;
                    m_ComputerMemory.Add(newMemory);
                }
            }
        }

        private bool lookForMatchWithLastFlippedCard()
        {
            bool foundMatch = false;
            int lastBlockInCompMem = m_ComputerMemory.Count - 1;

            foreach (AIMemoryBlock<char> mem in m_ComputerMemory)
            {
                if (mem.Value == m_ComputerMemory[lastBlockInCompMem].Value && (mem.Row != m_ComputerMemory[lastBlockInCompMem].Row || mem.Col != m_ComputerMemory[lastBlockInCompMem].Col))
                {
                    foundMatch = true;
                    m_Player2.RaisePlayerScore();
                    m_UnturnedTiles -= 2;
                    UpdateBoard(true, mem.Row, mem.Col);
                    UpdateBoard(true, m_ComputerMemory[lastBlockInCompMem].Row, m_ComputerMemory[lastBlockInCompMem].Col);
                    System.Threading.Thread.Sleep(2000);
                    m_ComputerMemory.RemoveAt(lastBlockInCompMem);
                    m_ComputerMemory.Remove(mem);                 
                    break;
                }
            }

            return foundMatch;
        }

        internal void UpdateBoard(bool i_Viewalble, int i_Row, int i_Col)
        {
            m_GameBoard[i_Row, i_Col].IsViewable = i_Viewalble;
            Screen.Clear();
            IOUtils.PrintBoard(m_GameBoard);
        }

        private bool findRandTileAndLookInMemory(ref int i_Row, ref int i_Col, ref int i_PlaceInMemory, ref int i_Row2, ref int i_Col2, Random i_Rnd)
        {
            bool foundMatch = false;

            findRandValidTile(ref i_Row, ref i_Col, i_Rnd);
            UpdateBoard(true, i_Row, i_Col);

            foreach (AIMemoryBlock<char> mem in m_ComputerMemory)
            {
                if (mem.IsViewAble == false)
                {
                    if (mem.Value == m_GameBoard[i_Row, i_Col].BlockValue && (mem.Row != i_Row || mem.Col != i_Col))
                    {
                        foundMatch = true;
                        i_Row2 = mem.Row;
                        i_Col2 = mem.Col;
                        break;
                    }
                }
              
                i_PlaceInMemory++;
            }

            return foundMatch;
        }

        private void updateUnMatchedAfterRandom(int i_Row1, int i_Col1, int i_Row2, int i_Col2)
        {
            AddToComputerMemory(i_Row2, i_Col2);
            UpdateBoard(false, i_Row1, i_Col1);
            UpdateBoard(false, i_Row2, i_Col2);
        }

        private void foundMatchWithMemory(int i_PlaceInMemory, int i_Row, int i_Col)
        {
            m_Player2.RaisePlayerScore();
            m_UnturnedTiles -= 2;
            m_ComputerMemory.RemoveAt(i_PlaceInMemory);
            UpdateBoard(true, i_Row, i_Col);
            System.Threading.Thread.Sleep(2000);
        }

        private void findRandValidTile(ref int i_Row, ref int i_Col, Random i_Rnd)
        {
            while (m_GameBoard[i_Row, i_Col].IsViewable == true) 
            {
                i_Row = i_Rnd.Next(0, m_GameBoard.BoardHeight);
                i_Col = i_Rnd.Next(0, m_GameBoard.BoardWidth);
            }
        }

        private bool checkIfAlreadyInMem(AIMemoryBlock<char> i_BlockToCheck)
        {
            bool blockInMem = false;
            foreach (AIMemoryBlock<char> mem in m_ComputerMemory)
            {
                if (mem.Row == i_BlockToCheck.Row && mem.Col == i_BlockToCheck.Col)
                {
                    blockInMem = true;
                }
            }

            return blockInMem;
        }

        internal void MakeMemoryBlockViewAble(int row1, int col1)
        {
            foreach (AIMemoryBlock<char> mem in m_ComputerMemory)
            {
                if (mem.Row == row1 && mem.Col == col1)
                {
                    mem.IsViewAble = true;
                }
            }
        }
    }
}
