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

        public int maxIterations = 5;
        public int populationSize = 100;

        List<Vector<double>> population;
        List<Vector<double>> newPopulation;

        MySorter s;
        Random r = new Random();

        public GeneticOtpimizer(int bones, double[][] A, double[] b)
        {
            this.bones = bones;

            this.A = Matrix<double>.Build.Dense(A.Length, A[0].Length);
            for (int i = 0; i < A.Length; i++)
                this.A.SetRow(i, A[i]);

            this.b = Vector<double>.Build.Dense(b);

            s = new MySorter();
            s.A = this.A;
            s.b = this.b;
        }

        public GeneticOtpimizer(int bones, Matrix<double> A, Vector<double> b)
        {
            this.bones = bones;
            this.A = A;
            this.b = b;

            s = new MySorter();
            s.A = A;
            s.b = b;
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

                if (Convergence() || iteration > maxIterations)
                    break;

                iteration++;
            }
            
            population.Sort(s);
            return population[0].ToArray();
        }

        // TODO how to compute fitness?
        private double FitnessFunction(Vector<double> solution)
        {
            Vector<double> sol = A * solution;
            Vector<double> diff = sol - b;
            double value = diff[0] * diff[0] + diff[1] * diff[1] + diff[2] * diff[2];
            return value;
        }

        private void Prune()
        {
            newPopulation = new List<Vector<double>>();
            population.Sort(s);

            for (int i = 0; i < populationSize; i++)
                newPopulation.Add(population[i]);

            population = newPopulation;
        }

        private void Mutate()
        {
            for (int i = 0; i < population.Count; i++)
            {
                if (r.NextDouble() < 0.3)
                {
                    int index = r.Next(0, bones);
                    if (r.NextDouble() < 0.5)
                    {
                        // add
                    }
                    else
                    {
                        // subtract
                    }
                }
            } 
        }

        private void MakeChildren()
        {
            population.Sort(s);

            double sum = 0;
            for (int i = 0; i < population.Count; i++)
                sum += FitnessFunction(population[i]);

            List<Vector<double>> children = new List<Vector<double>>();
            for (int i = 0; i < populationSize / 2; i++)
            {
                double num = r.NextDouble() * sum;
                Vector<double> parent1 = GetParent(num);

                num = r.NextDouble() * sum;
                Vector<double> parent2 = GetParent(num);

                Vector<double> child1 = Vector<double>.Build.Dense(bones);
                Vector<double> child2 = Vector<double>.Build.Dense(bones);

                // one point crossover
                for (int j = 0; j < bones; j++)
                {
                    if (j < bones / 2)
                    {
                        child1[j] = parent1[j];
                        child2[j] = parent2[j];
                    }
                    else
                    {
                        child1[j] = parent2[j];
                        child2[j] = parent1[j];
                    }
                }

                children.Add(child1);
                children.Add(child2);
            }

            for (int i = 0; i < children.Count; i++)
                population.Add(children[i]);
        }

        // Roulette Wheel Selection
        private Vector<double> GetParent(double num)
        {
            double runningSum = 0;
            for (int i = 0; i < population.Count; i++)
            {
                runningSum += FitnessFunction(population[i]);

                if (runningSum > num)
                    return population[i];
            }

            return population[0];
        }

        private bool Convergence()
        {
            return false;

            throw new NotImplementedException();
        }

        private void InitPopulation()
        {
            population = new List<Vector<double>>();
            Random r = new Random();
            for (int i = 0; i < populationSize; i++)
            {
                Vector<double> creecher = Vector<double>.Build.Dense(bones, 0);
                for (int j = 0; j < bones; j++)
                    creecher[j] = r.NextDouble();

                population.Add(creecher);
            }
        }
    }

    class MySorter : IComparer<Vector<double>>
    {
        public Matrix<double> A;
        public Vector<double> b;

        // TODO how to compute fitness?
        private double FitnessFunction(Vector<double> solution)
        {
            Vector<double> sol = A * solution;
            Vector<double> diff = sol - b;
            double value = diff[0] * diff[0] + diff[1] * diff[1] + diff[2] * diff[2];
            return value;
        }

        public int Compare(Vector<double> x, Vector<double> y)
        {
            double fx = FitnessFunction(x);
            double fy = FitnessFunction(y);
            if (fx < fy)
                return -1;
            if (fx > fy)
                return 1;

            return 0;
        }
    }
}
