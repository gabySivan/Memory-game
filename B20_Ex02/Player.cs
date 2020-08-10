using System;

namespace B20_Ex02
{
    internal class Player
    {
        internal readonly string m_PlayerName;
        internal int m_PlayerScore;

        public Player(string i_PlayerName)
        {
            m_PlayerName = i_PlayerName;
            m_PlayerScore = 0;
        }

        public string GetPlayerName
        {
            get
            {
                return m_PlayerName;
            }
        }

        public int GetPlayerScore
        {
            get
            {
                return m_PlayerScore;
            }
        }

        internal void RaisePlayerScore()
        {
            m_PlayerScore++;
        }
    }
}
