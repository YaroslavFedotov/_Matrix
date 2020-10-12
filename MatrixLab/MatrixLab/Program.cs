using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MatrixLab
{
	class Program
	{
		static void Main(string[] args)
		{
			int N = 0;
			StreamReader File = null;
			string s = "";
			
			try { File = new StreamReader("input.txt"); }
			catch (Exception)
			{
				Console.Write("\nНе удалось найти или открыть файл");
				Console.ReadLine();
				return;
			}
			
			s = File.ReadLine();
			
			try { N = int.Parse(s); }
			catch (Exception)
			{
				Console.Write("\nНе удалось проанализировать данные в 1-ой строке файла");
				Console.ReadLine();
				return;
			}
			
			if (N > 6)
			{
				Console.Write("\nКоличество строк и столбцов матрицы не должно превышать 6");
				Console.ReadLine();
				return;
			}
			
			int[,] Matrix = new int[N, N];
			int[] Answers = new int[N];
			string[] buf = new string[N];
			
			for (int i = 0; i < N; i++)
			{
				s = File.ReadLine();
				buf = s.Split(' ');

				for (int j = 0; j < N; j++)
				{
					try { Matrix[i, j] = int.Parse(buf[j]); }
					catch (Exception)
					{
						Console.Write("\nНе удалось проанализировать данные в {0}-ой строке, {1}-ом столбце файла", i+1, j+1);
						Console.ReadLine();
						return;
					}

					if (Matrix[i, j] > 1000)
					{
						Console.Write("\nОдин из элементов матрицы коэффициентов превышает 1000");
						Console.ReadLine();
						return;
					}
				}
			}
			
			for (int i = 0; i < N; i++)
			{
				s = File.ReadLine();
				try { Answers[i] = int.Parse(s); }
				catch (Exception)
				{
					Console.Write("\nНе удалось проанализировать данные в {0}-ой строке файла", i + 1);
					Console.ReadLine();
					return;
				}

				if (Answers[i] > 1000)
				{
					Console.Write("\nОдин из элементов матрицы ответов превышает 1000");
					Console.ReadLine();
					return;
				}
			}
			
			bool bNoNumbersInLine = true;

			for (int i = 0; i < N; i++)
			{
				bNoNumbersInLine = true;

				for (int j = 0; j < N; ++j)
				{
					if (Matrix[i, j] != 0)
					{
						if (j == 0)
							Console.Write(Matrix[i, j] != 1 ? "{0}*x{1}" : "x{1}", Matrix[i, j], j + 1);
						else
						{
							if (Matrix[i,j] == 1)
								Console.Write("x{0}", j + 1);
							else
								Console.Write(Matrix[i, j] > 0 ? " + {0}*x{1}" : " - {0}*x{1}", Math.Abs(Matrix[i, j]), j + 1);
						}

						bNoNumbersInLine = false;
					}
				}
				
				if (!bNoNumbersInLine)
					Console.Write(" = {0}", Answers[i]);
				else
					Console.Write("0 = {0}", Answers[i]);

				Console.Write("\n");
			}
			
			File.Close();

			double I, I1 = 0;
			int counter = 1;
			for (int i = 0; i < N - 1; i++)
			{
				for (int j = 0; j < N; j++)
				{
					I = Matrix[i, j] / Matrix[i + 1, j];
					if (I1 == I)
						counter++;
					I1 = I;
					if (counter == N)
					{
						Console.WriteLine("Система недоопределенна");
						Console.ReadKey();
						return;
					}
				}
				counter = 1;
			}

			int[] A1 = new int[N];
			double[] X = new double[N];
			long[] Det = new long[N];
			long x = Determinant(Matrix);            
			Console.Write("\nДетерминант матрицы: {0:d}\n", x);
			int count_i = 0;
			for (int j = 0; j < N; j++)
			{
				for (int i = 0; i < N; i++)
				{
					A1[i] = Matrix[i, j];
					Matrix[i, j] = Answers[i];
				}
				Det[j] = Determinant(Matrix);
				Console.WriteLine(Det[j]);
				if (x == 0)
				{
					if (Det[j] != 0)
						count_i++;
				}
				for (int i = 0; i < N; i++)
				{
					Matrix[i, j] = A1[i];
				}
			}

			if (x == 0)
			{
				if (count_i == 0)
				{
					Console.Write("\nСистема недоопределена");
					Console.ReadLine();
					return;
				}
				if (count_i > 0)
				{
					Console.Write("\nСистема несовместна");
					Console.ReadLine();
					return;
				}
			}
			Console.Write("\nКорни системы:");
            Console.Write("\n");
            for (int i = 0; i < N; i++)
			{
				X[i] = Det[i] / (double)x;
				Console.WriteLine("x{0:d} = {1:F3}", i + 1, X[i]);
			}

			Console.ReadLine();
		}

		static long Determinant(int[,] Arr)
		{
			if (Arr.GetLength(0) == 1)
				return Arr[0, 0];
			else
			{
				long D = 0;
				int R = Arr.GetLength(0);
				int i = 0;
				for (int j = 0; j < R; j++)
				{
					int[,] NewArr = new int[R - 1, R - 1];
					for (int i2 = 0; i2 < R - 1; i2++)
					{
						for (int j2 = 0; j2 < R - 1; j2++)
						{
							int i3 = i2 < i ? i2 : i2 + 1;
							int j3 = j2 < j ? j2 : j2 + 1;
							NewArr[i2, j2] = Arr[i3, j3];
						}
						D += ((i + j) % 2 == 0 ? 1 : -1) * Arr[i, j] * Determinant(NewArr);
					}
				}
				return D;
			}
		}
	}
}
