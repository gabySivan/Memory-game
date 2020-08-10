using System;

namespace B20_Ex02
{
    internal class AIMemoryBlock<T>
    {
        private T m_Value;
        private int m_Row;
        private int m_Col;
        private bool m_ViewAble;

        public AIMemoryBlock(T i_Value, int i_Row, int i_Col)
        {
            m_Value = i_Value;
            m_Row = i_Row;
            m_Col = i_Col;
            m_ViewAble = true;
        }

        public T Value
        {
            get
            {
                return m_Value;
            }

            set
            {
                m_Value = value;
            }
        }

        public int Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int Col
        {
            get
            {
                return m_Col;
            }

            set
            {
                m_Col = value;
            }
        }

        public bool IsViewAble
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
    }
}
