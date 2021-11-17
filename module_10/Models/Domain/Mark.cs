namespace Models.Domain
{
    public class Mark
    {
        private int _mark;
        public int CorrectMark { get { return _mark; } }

        private Mark(int mark)
        {
            _mark = mark;
        }

        public static Mark TryCreate(int mark)
        {
            if (!IsValid(mark))
            {
                return null;
            }
            return new Mark(mark);
        }

        private static bool IsValid(int mark)
        {
            if (mark < 0 || mark > 5)
            {
                return false;
            }
            return true;
        }
    }
}
