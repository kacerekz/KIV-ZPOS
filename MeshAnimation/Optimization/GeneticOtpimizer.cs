using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation.Optimization
{
    class GeneticOtpimizer
    {
        int bones;
        Matrix<double> A;
        Vector<double> b;

        public int maxIterations = 10;
        public int populationSize = 50;
        public int bestToSkip = 5;
        public int cached = 10;
        
        public double crossChance = 0.5;
        public double mutateChance = 0.05;
        public double maxMutation = 0.2;
        public double maxError = 0.01;

        Vector<double>[] population;
        Vector<double>[] newPopulation;

        bool cache;

        static Vector<double> cachedResult;

        readonly Random r = new Random();

        public GeneticOtpimizer(int bones, double[][] A, double[] b, bool cache)
        {
            this.bones = bones;
            this.cache = cache;

            this.A = Matrix<double>.Build.Dense(A.Length, A[0].Length);
            for (int i = 0; i < A.Length; i++)
                for (int j = 0; j < A[0].Length; j++)
                    this.A[i, j] = A[i][j];

            this.b = Vector<double>.Build.Dense(b);
        }

        public GeneticOtpimizer(int bones, Matrix<double> A, Vector<double> b, bool cache)
        {
            this.bones = bones;
            this.cache = cache;

            this.A = A;
            this.b = b;
        }

        public double[] SolveForLeastSquares()
        {
            InitPopulation();

            int iteration = 0; 

            while(true)
            {
                MakeChildren();
                Mutate();
                Prune();

                // Sort by fitnesses
                double[] fitnesses = new double[population.Length];
                for (int i = 0; i < fitnesses.Length; i++)
                    fitnesses[i] = FitnessFunction(population[i]);

                Array.Sort(fitnesses, population);

                if (fitnesses[0] < maxError || iteration > maxIterations)
                    break;

                iteration++;
            }

            if (cache)
                cachedResult = population[0];
            
            var winner = population[0].ToArray();
            
            //Console.Write("Winner: [" + winner[0]);
            //for (int i = 1; i < winner.Length; i++)
            //    Console.Write(", " + winner[i]);
            //Console.WriteLine($"] (it: {iteration})");

            return winner;
        }

        private void InitPopulation()
        {
            population = new Vector<double>[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                Vector<double> creecher = Vector<double>.Build.Dense(bones, 0);
                for (int j = 0; j < bones; j++)
                    creecher[j] = r.NextDouble();

                population[i] = creecher;
            }

            if (cachedResult != null && cache)
                for (int i = 0; i < cached; i++)
                {
                    int ri = r.Next(0, populationSize / cached + i * cached);
                    population[ri] = cachedResult.Clone();

                    if (i == 0) continue;

                    // Get the spot to mutate
                    int index = r.Next(0, bones);

                    // Throw for addition / subtraction
                    if (r.NextDouble() < 0.5)
                        population[ri][index] += r.NextDouble() * maxMutation;
                    else
                        population[ri][index] -= r.NextDouble() * maxMutation;
                }

        }

        private void MakeChildren()
        {
            // Generate new population
            newPopulation = new Vector<double>[populationSize];
            
            // Don't overwrite the best few results
            for (int i = 0; i < bestToSkip; i++)
            {
                newPopulation[i] = population[i];
            }

            // For the rest, cross parents
            for (int i = bestToSkip; i < populationSize; i++)
            {
                // If you fail cross chance, you pass into the next round
                if (r.NextDouble() > crossChance)
                {
                    newPopulation[i] = population[i];
                }

                // Otherwise, make a baby
                var parent1 = population[i];
                var parent2 = population[r.Next(0, populationSize)];

                var child = Vector<double>.Build.Dense(bones);

                // One point crossover
                for (int j = 0; j < bones; j++)
                {
                    if (j < bones / 2)
                        child[j] = parent1[j];
                    else
                        child[j] = parent2[j];
                }

                newPopulation[i] = child;
            }

            population = newPopulation;
        }

        private void Mutate()
        {
            for (int i = 0; i < populationSize; i++)
            {
                if (r.NextDouble() < mutateChance)
                {
                    // Get the spot to mutate
                    int index = r.Next(0, bones);

                    // Throw for addition / subtraction
                    if (r.NextDouble() < 0.5)
                        population[i][index] += r.NextDouble() * maxMutation;
                    else
                        population[i][index] -= r.NextDouble() * maxMutation;
                }
            }
        }

        private void Prune()
        {
            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < population[i].Count; j++)
                {
                    if (population[i][j] < 0) population[i][j] = 0;
                    else
                    if (population[i][j] > 1) population[i][j] = 1;
                }
            }
        }

        private double FitnessFunction(Vector<double> solution)
        {
            Vector<double> sol = A * solution;
            Vector<double> diff = sol - b;
            double sum = 0;
            for (int i = 0; i < diff.Count; i++)
                sum += diff[i] * diff[i];
            return sum;
        }

    }

}
