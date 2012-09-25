namespace Common.Misc
{
    public class FrameCounter
    {
        private static FrameCounter instance;
        private int m_iFramesElapsed;
        private long m_lCount;

        private FrameCounter()
        {
            m_lCount = 0;
            m_iFramesElapsed = 0;
        }


        public static FrameCounter Instance()
        {
            if (instance == null)
            {
                instance = new FrameCounter();
            }
            return instance;
        }

        public void Update()
        {
            ++m_lCount;
            ++m_iFramesElapsed;
        }

        public long GetCurrentFrame()
        {
            return m_lCount;
        }

        public void Reset()
        {
            m_lCount = 0;
        }

        public void Start()
        {
            m_iFramesElapsed = 0;
        }

        public int FramesElapsedSinceStartCalled()
        {
            return m_iFramesElapsed;
        }
    }
}