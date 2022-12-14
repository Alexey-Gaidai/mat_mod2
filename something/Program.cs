using System;
using System.Collections.Generic;
using System.Linq;

namespace something
{
    class Program
    {
        static void Main(string[] args)
        {
            SecondExc second = new SecondExc();
            second.start();
            //Program abc = new Program();
            //abc.calc();
            //abc.calcB();
        }

        void calc()
        {
            double[] profit = new double[100000];
            for (int i = 0; i < profit.Length; i++)
            {
                profit[i] = calculateProfit(1, 40);
            }
            double sum = 0;
            for (int i = 0; i < profit.Length; i++)
            {
                sum += profit[i];
            }
            double avg = sum / profit.Length;
            Console.WriteLine("среднее со скидкой: "+avg);
        }
        void calcB()
        {
            double[] profit = new double[100000];
            for (int i = 0; i < profit.Length; i++)
            {
                profit[i] = calculateProfitB(1, 20);
            }
            double sum = 0;
            for (int i = 0; i < profit.Length; i++)
            {
                sum += profit[i];
            }
            double avg = sum / profit.Length;
            Console.WriteLine("среднее без скидки: "+ avg);
        }


        double calculateProfit(int A, int B)
        {
            const int price = 25;
            const int priceB = 20;
            const int costPrice = 12;
            Random rand = new Random();
            double R = rand.Next(A, B);
            if (R <= 10)
            {
                return (price - costPrice) * R;
            } else
            {
                return (priceB - costPrice) * R;
            }
        }
        double calculateProfitB(int A, int B)
        {
            const int price = 25;
            const int costPrice = 12;
            Random rand = new Random();
            double R = rand.Next(A, B);
            return (price - costPrice) * R;
        }
    }
    class SecondExc
    {
        public void start() 
        {
            Random rnd = new Random();
            double lambda=0.1;
            double t=10;
            double f = lambda * Math.Pow(Math.E, (-lambda * t));
            double[] result = new double[100];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = -(1 / lambda) * Math.Log(rnd.NextDouble());
            }
            double sum = 0;
            foreach (var item in result)
            {
                sum += item;
            }
            double xm = sum / result.Length;
            Console.WriteLine("size :" + result.Length);
            Console.WriteLine("Мат. Ожидание :"+sum/result.Length);
            Console.WriteLine("Мат. Ожидание :" + 1 /lambda);
            double DXsum = 0;
            for (int i = 0; i < result.Length; i++)
            {
                DXsum += Math.Pow(result[i] - xm, 2);
            }
            double DX = DXsum / result.Length;
            double O = Math.Sqrt(DX);
            Console.WriteLine("DX :" + DX);
            Console.WriteLine("O:" + O);
            double Xmax = Double.MinValue;
            double Xmin = Double.MaxValue;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] >= Xmax)
                    Xmax = result[i];
                if (result[i] <= Xmin)
                    Xmin = result[i];
            }
            Console.WriteLine("x max: " + Xmax);
            Console.WriteLine("x min: " + Xmin);
            double h = (Xmax - Xmin) / (1 + 3.3221 * Math.Log10(result.Length));
            Console.WriteLine("h:" + h);
            List<int> interval = new List<int>();
            List<double> seredinyIntervalov = new List<double>();
            int k = 1;
            double[,] intervaly2 = new double[Convert.ToInt32(Xmax / h), 2];
            double srrr = 0;
            do
            {
                int count = 0;
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i] >= Xmin+(k-1)*h && result[i] < Xmin + k * h)
                    {
                        count++;
                    }
                }
                double sredInt = (((Xmin + (k - 1) * h) + (Xmin + k * h)) / 2);
                srrr += sredInt * count;
                intervaly2[k - 1, 0] = Xmin + (k - 1) * h;
                intervaly2[k - 1, 1] = Xmin + k * h; 
                interval.Add(count);
                seredinyIntervalov.Add(((Xmin + (k - 1) * h)+(Xmin + k * h))/2);
                Console.WriteLine($"интервал {k} от {Xmin+(k-1)*h} до {Xmin + k*h}");
                Console.WriteLine($"середина интервала {k}: {((Xmin + (k - 1) * h) + (Xmin + k * h)) / 2}");
                k++;
            } while (getSum(interval) < 100);
            Console.WriteLine($"кол-во интервалов: {interval.Count}");
            for (int i = 0; i < interval.Count; i++)
            {
                Console.WriteLine($"n{i}: " + interval[i]);
            }
            var sredn = srrr/100;
            Console.WriteLine($"выборочное среднее: {sredn}");
            double lamda = 1 / sredn;
            double[] pi = new double[interval.Count];
            double sumP = 0;
            for (int i = 0; i < intervaly2.GetLength(0); i++)
            {
                pi[i] = Math.Exp(-lamda * intervaly2[i,0]) - Math.Exp(-lamda * intervaly2[i, 1]);
                Console.WriteLine($"вероятность попадания в интервал [{intervaly2[i, 0]};{intervaly2[i, 1]}]: {pi[i]}");
                sumP += pi[i];
            };
            Console.WriteLine($"Сумма вероятности: {sumP}");
            double[] nis = new double[pi.Length];
            for (int i = 0; i < nis.Length; i++)
            {
                nis[i] = result.Length * pi[i];
                Console.WriteLine($"теоретическая вероятность попадания в интервал [{intervaly2[i, 0]};{intervaly2[i, 1]}]: {nis[i]}");
            }
            double xi2 = 0;

            double[] frequency = new double[interval.Where(c => c > 5).Count()+1];
            double[] newNis = new double[frequency.Length];
            for (int i = 0; i < frequency.Length-1; i++)
            {
                frequency[i] = interval[i];
                newNis[i] = nis[i];
            }
            frequency[frequency.Length - 1] = interval.Where(c => c < 5).Sum();
            for (int i = 0; i < nis.Length; i++)
            {
                if (interval[i] < 5)
                    newNis[newNis.Length - 1] += nis[i];
            }
            Console.WriteLine($"объединенные частоты(кол-во): {frequency.Length}");
            int ka = frequency.Length - 2;
            double a = 0.5;
            double XiKr4 = 3.357;
            double XiKr5 = 4.351;
            double XiKr6 = 5.348;
            for (int i = 0; i < ka; i++)
            {
                xi2 += Math.Pow((frequency[i] - newNis[i]), 2) / newNis[i];
            }
            Console.WriteLine($"Хи2набл: {xi2}");
            if (frequency.Length == 4)
            {
                Console.WriteLine($"Хи2критическое: {XiKr4}");
                if (xi2 < XiKr4)
                {
                    Console.WriteLine($"Нет оснований отвергать гипотезу");
                }
                else
                {
                    Console.WriteLine($"Гипотезу отвергаем");
                }
            }
            if (frequency.Length == 5)
            {
                Console.WriteLine($"Хи2критическое: {XiKr5}");
                if(xi2 < XiKr5)
                {
                    Console.WriteLine($"Нет оснований отвергать гипотезу");
                } else
                {
                    Console.WriteLine($"Гипотезу отвергаем");
                }
            }
            if(frequency.Length == 6)
            {
                Console.WriteLine($"Хи2критическое: {XiKr6}");
                if (xi2 < XiKr6)
                {
                    Console.WriteLine($"Нет оснований отвергать гипотезу");
                }
                else
                {
                    Console.WriteLine($"Гипотезу отвергаем");
                }
            }
        }
        private static double srednee(List<int> count, List<double> serediny)
        {
            double sum = 0;
            for (int i = 0; i < serediny.Count; i++)
            {
                sum += serediny[i] * count[i];
            }
            return sum / serediny.Count;
        }

        private int getSum(List<int> list)
        {
            int sum = 0;
            foreach (var item in list)
            {
                sum += item;
            }
            return sum;
        }
    }
}
