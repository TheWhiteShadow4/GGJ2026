using System.Collections.Generic;

namespace TWS.Utils
{
    public static class ListExtensions
    {
		private static System.Random rng = new System.Random();

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

        /// <summary>
        /// Entfernt ein Element an einem bestimmten Index, indem es mit dem letzten Element getauscht wird.
        /// Dies ist effizienter als List.RemoveAt, wenn die Reihenfolge der Elemente nicht wichtig ist.
        /// </summary>
        /// <typeparam name="T">Der Typ der Listenelemente</typeparam>
        /// <param name="list">Die Liste, aus der entfernt werden soll</param>
        /// <param name="index">Der Index des zu entfernenden Elements</param>
        public static void SwapRemove<T>(this List<T> list, int index)
        {
            int lastIndex = list.Count - 1;
            if (index != lastIndex)
            {
                list[index] = list[lastIndex];
            }
            list.RemoveAt(lastIndex);
        }
    }
} 