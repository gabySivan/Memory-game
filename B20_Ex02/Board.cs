using System;

namespace B20_Ex02
{
    internal class Board
    {
        private readonly int m_Height;
        private readonly int m_Width;
        private readonly Block<char>[,] m_Board;

        internal Board(int i_Width, int i_Height)
        {
            m_Board = new Block<char>[i_Height, i_Width]; 
            m_Height = i_Height;
            m_Width = i_Width;
            initialize();
        }

        public Block<char> this[int i, int j]
        {
            get
            {
                return m_Board[i, j];
            }

            set
            {
                m_Board[i, j] = value;
            }
        }

        public int BoardWidth
        {
            get
            {
                return m_Width;
            }
        }

        public int BoardHeight
        {
            get
            {
                return m_Height;
            }
        }

        public void Scramble()
        {
            Random rand = new Random();
            int[] scrambleArray = new int[m_Height];

            for (int i = 0; i < m_Height; i++)
            {
                scrambleArray[i] = m_Width - 1;
            }

            for (int i = 0; i < m_Height; i++)
            {
                for (int j = 0; j < m_Width; j++)
                {
                    int randomRow = rand.Next(0, m_Height);
                    if (scrambleArray[randomRow] != 0)
                    {
                        swap(randomRow, scrambleArray, i, j);
                        scrambleArray[randomRow]--;
                    }
                }
            }
        }
        
        private void swap(int i_RandomRow, int[] i_ScrambleArray, int i_Row, int i_Col)
        {
            int randomPlace = i_ScrambleArray[i_RandomRow];
            Block<char> temp = m_Board[i_Row, i_Col];
            m_Board[i_Row, i_Col] = m_Board[i_RandomRow, randomPlace];
            m_Board[i_RandomRow, randomPlace] = temp;
        }

        private void initialize()
        {
            double letter = 'A';
            for (int i = 0; i < m_Height; i++)
            {
                for (int j = 0; j < m_Width; j++)
                {
                    m_Board[i, j] = new Block<char>((char)Math.Floor(letter));
                    letter += 0.5;
                }
            }

            Scramble();
        }

        public char ValueToShow(int i_Row, int i_Col)
        {
            char value = ' ';
            if (m_Board[i_Row, i_Col].IsViewable == true)
            {
                value = m_Board[i_Row, i_Col].BlockValue;
            }

            return value;
        }

        internal class Block<T>
        {
            private readonly T m_Value;
            private bool m_ViewAble;

            public Block(T i_Value)
            {
                m_Value = i_Value;
                m_ViewAble = false;
            }

            public bool IsViewable
            {
                get
                {
                    return m_ViewAble;
                }

                set
                {
                    m_ViewAble = value;
                }
            }

            public T BlockValue
            {
                get
                {
                    return m_Value;
                }
            }

            public static bool operator ==(Block<T> i_Block1, Block<T> i_Block2)
            {
                return i_Block1.m_Value.Equals(i_Block2.m_Value);
            }

            public static bool operator !=(Block<T> i_Block1, Block<T> i_Block2)
            {
                return !(i_Block1 == i_Block2);             
            }

            public override bool Equals(object other) 
            {
                return this == (Block<T>)other;
            }
        }
    }
}
