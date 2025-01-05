namespace MossWolfGames.Shared.Runtime.Extensions
{
    public static class ArrayExtensions
    {
        public static bool AnyAdjacentEqualTo<T>(this T[,] array2d, int x, int y, T equalTo)
        {
            return array2d.CountAdjacentEqualTo(x,y,equalTo) > 0;
        }

        public static int CountAdjacentEqualTo<T>(this T[,] array2d, int x, int y, T equalTo)
        {
            int count = 0;

            // Left
            if (x > 0)
            {
                if (array2d[x - 1, y].Equals(equalTo))
                {
                    count++;
                }
                // Bottom left
                if (y > 0 && array2d[x - 1, y - 1].Equals(equalTo))
                {
                    count++;
                }
                // Top left
                if (y > array2d.GetLength(1) - 1 && array2d[x - 1, y + 1].Equals(equalTo))
                {
                    count++;
                }
            }
            // Right
            if (x < array2d.GetLength(0) - 1)
            {
                if (array2d[x + 1, y].Equals(equalTo))
                {
                    count++;
                }
                // Bottom right
                if (y > 0 && array2d[x + 1, y - 1].Equals(equalTo))
                {
                    count++;
                }
                // Top right
                if (y < array2d.GetLength(1) - 1 && array2d[x + 1, y + 1].Equals(equalTo))
                {
                    count++;
                }
            }
            // Top
            if (y < array2d.GetLength(1) - 1 && array2d[x, y + 1].Equals(equalTo))
            {
                count++;
            }

            // Bottom
            if (y > 0 && array2d[x, y - 1].Equals(equalTo))
            {
                count++;
            }

            return count;
        }
    }
}