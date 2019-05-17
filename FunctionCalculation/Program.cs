using System;
using Bl;
using Bl.Method;

namespace FunctionCalculation
{
    class Program
    {
        private static double startPoint = 0, h = 0.5, eps = 0.001;

        private static double FunctionD1(double x) => 4 * (x - 3) + 0.5 * Math.Pow(Math.E, 0.5 * x);

        private static double FunctionD2(double x) => 4 + 0.25 * Math.Pow(Math.E, 0.5 * x);

        private static double MinimizationFunction(double x) => 2 * Math.Pow(x - 3, 2) + Math.Pow(Math.E, 0.5 * x);

        static void Main(string[] args)
        {
            Console.WriteLine("Начальная точка X0 = {0}; h = {1}; eps = {2}", startPoint, h, eps);

            var optimization = new UnconditionalOptimization(MinimizationFunction);
            optimization.OnIteration += Print_OnIteration;
            var (leftBound, rightBound) = optimization.GetBoundForMinimization(startPoint, h);
            PrintBoundaries(leftBound,rightBound, optimization.Iteration);

            Console.ReadKey();
            Console.WriteLine("\n///Уточнение границ методом деления пополам///");

            var halvingMethod = new HalvingMethod(MinimizationFunction);
            halvingMethod.OnIteration += Print_OnIteration;
            var resultHalving = halvingMethod.Calculation(leftBound, rightBound, eps);
            PrintBoundaries(halvingMethod.LeftBound, halvingMethod.RightBound, halvingMethod.Iterations);
            PrintFunction(resultHalving, MinimizationFunction);

            Console.ReadKey();
            Console.WriteLine("\n///Уточнение метом квадратичной аппроксимации///");

            var approximationMethod = new ApproximationMethod(MinimizationFunction);
            approximationMethod.OnIteration += Print_OnIteration;
            var resultApproximation = approximationMethod.Calculation(leftBound, rightBound, eps);
            PrintBoundaries(approximationMethod.LeftBound, approximationMethod.RightBound, approximationMethod.Iterations);
            PrintFunction(resultApproximation, MinimizationFunction);

            Console.ReadKey();
            Console.WriteLine("\n///Метод Ньютона///");
            var newtonMethod = new NewtonMethod(FunctionD1, FunctionD2);
            newtonMethod.OnIteration += Print_OnIterationNewtonMethod;
            var resultNewtonMethod = newtonMethod.Calculation(startPoint);
            Console.Write("\nРезультат: ");
            PrintFunction(resultNewtonMethod, MinimizationFunction);

            //var goldenSection = new GoldenSection(MinimizationFunction);
            //goldenSection.OnIteration += Print_OnIteration;
            //var r = goldenSection.FindMin(leftBound, rightBound, eps);
            //PrintFunction(r,MinimizationFunction);

            Console.ReadKey();
        }

        private static void PrintFunction(double x, SingleVariableFunctionDelegate function) 
            => Console.WriteLine("x = {0:f5}\tЗначение функции {1:f5}", x, function(x));

        private static void PrintBoundaries(double leftBound, double rightBound, int iteration) 
            => Console.WriteLine("\nРезультат: [{0:f5}; {1:f5}] Итераций = {2}", leftBound, rightBound, iteration);

        private static void Print_OnIteration(object sender, IterationInfoEventArgs infoEventArgs) 
            => Console.WriteLine("Границы: [{0:f5}; {1:f5}]\tИтерация = {2}", infoEventArgs.LeftBound,
                                                              infoEventArgs.RightBound, infoEventArgs.Iteration);

        private static void Print_OnIterationNewtonMethod(object sender, IterationInfoEventArgs infoEventArgs)
            => Console.WriteLine("x = {0:f5}\tИтерация = {1}", infoEventArgs.RightBound, infoEventArgs.Iteration);

    }
}
