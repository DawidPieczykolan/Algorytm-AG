using System;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static Random random = new Random();

    static void Main(string[] args)
    {
        int numberOfCities = 25; // liczba miast do odwiedzenia
        int populationSize = 400; // wielkość populacji
        int generations = 700; // liczba pokoleń
        double mutationRate = 0.01; // współczynnik mutacji

        // Inicjalizacja populacji
        List<int[]> population = new List<int[]>();
        for (int i = 0; i < populationSize; i++)
        {
            int[] chromosome = Enumerable.Range(0, numberOfCities).ToArray();
            Shuffle(chromosome);
            population.Add(chromosome);
        }

        // Główna pętla algorytmu genetycznego
        for (int generation = 0; generation < generations; generation++)
        {
            // Ocena populacji
            List<double> fitnessScores = new List<double>();
            for (int i = 0; i < populationSize; i++)
            {
                double distance = CalculateDistance(population[i]);
                double fitnessScore = 1.0 / distance;
                fitnessScores.Add(fitnessScore);
            }

            // Selekcja
            List<int[]> selectedPopulation = new List<int[]>();
            for (int i = 0; i < populationSize; i++)
            {
                int index1 = SelectIndexByFitness(fitnessScores);
                int index2 = SelectIndexByFitness(fitnessScores);
                int[] chromosome1 = population[index1];
                int[] chromosome2 = population[index2];
                int[] selectedChromosome = Crossover(chromosome1, chromosome2);
                selectedPopulation.Add(selectedChromosome);
            }

            // Mutacja
            for (int i = 0; i < populationSize; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    Mutate(selectedPopulation[i]);
                }
            }

            // Zastąpienie starej populacji nową
            population = selectedPopulation;
        }

        // Znalezienie najlepszego rozwiązania
        int[] bestChromosome = null;
        double bestDistance = double.MaxValue;
        for (int i = 10; i < populationSize; i++)
        {
            double distance = CalculateDistance(population[i]);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestChromosome = population[i];
            }
        }

        Console.WriteLine("Najkrótsza trasa: " + string.Join(" -> ", bestChromosome));
        Console.WriteLine("Długość trasy: " + bestDistance);
        Console.ReadKey();
    }

    // Funkcja przemieszująca elementy tablicy w losowej kolejności
    static void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            T temp = array[j];
            array[j] = array[i];
            array[i] = temp;
        }
    }

    // Funkcja obliczająca odległość między kolejnymi miastami w chromosomie
    static double CalculateDistance(int[] chromosome)
    {
        double distance = 0.0;
        for (int i = 0; i < chromosome.Length - 1; i++)
        {
            int city1 = chromosome[i];
            int city2 = chromosome[i + 1];
            distance += GetDistanceBetweenCities(city1, city2);
        }
        int lastCity = chromosome[chromosome.Length - 1];
        int firstCity = chromosome[0];
        distance += GetDistanceBetweenCities(lastCity, firstCity);
        return distance;
    }

    // Funkcja zwracająca odległość między dwoma miastami
    // W tym przykładzie odległość między miastami jest losowana z zakresu 1-100
    static double GetDistanceBetweenCities(int city1, int city2)
    {
        // W tym przykładzie odległość między miastami jest losowana z zakresu 1-100
        return random.Next(1, 101);
    }

    // Funkcja wybierająca indeks chromosomu na podstawie wartości funkcji przystosowania
    static int SelectIndexByFitness(List<double> fitnessScores)
    {
        double totalFitness = fitnessScores.Sum();
        double randomValue = random.NextDouble() * totalFitness;
        double sum = 0.0;
        for (int i = 0; i < fitnessScores.Count; i++)
        {
            sum += fitnessScores[i];
            if (sum > randomValue)
            {
                return i;
            }
        }
        return fitnessScores.Count - 1;
    }

    // Funkcja krzyżująca dwa chromosomy
    static int[] Crossover(int[] chromosome1, int[] chromosome2)
    {
        int[] child = new int[chromosome1.Length];
        int start = random.Next(0, chromosome1.Length);
        int end = random.Next(start, chromosome1.Length);
        for (int i = start; i <= end; i++)
        {
            child[i] = chromosome1[i];
        }
        int index = 0;
        for (int i = 0; i < chromosome2.Length; i++)
        {
            if (!child.Contains(chromosome2[i]))
            {
                while (child[index] != 0)
                {
                    index++;
                }
                child[index] = chromosome2[i];
            }
        }
        return child;
    }

    // Funkcja mutująca chromosom
    static void Mutate(int[] chromosome)
    {
        int index1 = random.Next(0, chromosome.Length);
        int index2 = random.Next(0, chromosome.Length);
        int temp = chromosome[index1];
        chromosome[index1] = chromosome[index2];
        chromosome[index2] = temp;
    }
}


