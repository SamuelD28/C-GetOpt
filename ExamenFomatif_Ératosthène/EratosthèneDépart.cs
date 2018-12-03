using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eratosthène
{
	public static class EratosthèneDépart
	{
		private static List<bool> premiers = Enumerable.Repeat(true, Max + 1).ToList();

		private const int Max = 1000;

		/// <summary>
		/// Exemple de départ pour examen
		/// Tous les éléments sont initialisés à true
		/// </summary>
		private static void Eratosthène()
		{
			premiers[0] = false;
			premiers[1] = false;
			int racineDuMax = (int)Math.Ceiling(Math.Sqrt(Max));
			// Après la boucle, tous les éléments encore à true sont premiers
			for (int i = 2; i < racineDuMax; ++i)
			{
				if (premiers[i])
				{
					int j = i * i;
					while (j <= Max)
					{
						premiers[j] = false;
						j += i;
					}
				}
			}
		}

		public static List<bool> GetEratosthène()
		{
			Eratosthène();
			return premiers;
		}
	}
}

