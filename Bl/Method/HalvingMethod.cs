namespace Bl.Method
{
    public class HalvingMethod:Iteration
    {
        private readonly SingleVariableFunctionDelegate _f;

        public double MiddleInterval(double leftBound, double rightBound) => (leftBound + rightBound) / 2;

        public double IntervalLength(double leftBound, double rightBound) => rightBound - leftBound;

        public double X1(double leftBound, double intervalLength) => leftBound + intervalLength / 4;

        public double X2(double rightBound, double intervalLength) => rightBound - intervalLength / 4;

        public HalvingMethod(SingleVariableFunctionDelegate function)
        {
            _f = function;
        }

        /// <summary>
        /// Вычисление методом деления пополам
        /// </summary>
        /// <param name="leftBound">Значение левой границы</param>
        /// <param name="rightBound">Значение правой границы</param>
        /// <returns>Средение значение x между границами</returns>
        public double Calculation(double leftBound, double rightBound)
        {
            IterationInfoEventArgs = new IterationInfoEventArgs(leftBound, rightBound, 1);
            var middleInterval = MiddleInterval(leftBound, rightBound);
            return RunIterations(IterationInfoEventArgs, ref middleInterval);
        }

        /// <summary>
        /// Вычисление методом деления пополам c заданой eps
        /// </summary>
        /// <param name="leftBound">Значение левой границы</param>
        /// <param name="rightBound">Значение правой границы</param>
        /// <param name="eps">Заданая точность</param>
        /// <returns></returns>
        public double Calculation(double leftBound, double rightBound, double eps)
        {
            double a = leftBound, b = rightBound; var iteration = 0;
            var middleInterval = MiddleInterval(leftBound, rightBound);
            var len = b-a;
            var err = len;
           
            while (err >= eps)
            {
                var dx = IntervalLength(a, b);
                var x1 = X1(a, dx);
                var x2 = X2(b, dx);

                if (_f(x1) < _f(middleInterval))
                {
                    b = middleInterval;
                    middleInterval = x1;
                }
                else if (_f(x2) < _f(middleInterval))
                {
                    a = middleInterval;
                    middleInterval = x2;
                }
                else if (_f(x2) >= _f(middleInterval) || _f(x1) >= _f(middleInterval))
                {
                    a = x1;
                    b = x2;
                }

                err = (b-a) / len;
                iteration++;
                ShowIteration(a, b, iteration);
            }
            IterationInfoEventArgs = new IterationInfoEventArgs(a, b, iteration);
            return middleInterval;
        }

        /// <summary>
        /// Выполнение итераций
        /// </summary>
        /// <param name="iterationInfo">Информация об итерациях</param>
        /// <param name="middleInterval">Средение значение x</param>
        /// <returns>Средение значение x между границами</returns>
        private double RunIterations(IterationInfoEventArgs iterationInfo, ref double middleInterval)
        {
            var dx = IntervalLength(iterationInfo.LeftBound, iterationInfo.RightBound);
            var x1 = X1(iterationInfo.LeftBound, dx);
            var x2 = X2(iterationInfo.RightBound, dx);
           
            if (_f(x1) < _f(middleInterval))
            {
                iterationInfo.RightBound = middleInterval;
                middleInterval = x1;
                ShowIteration(iterationInfo);
                iterationInfo.Iteration++;
                RunIterations(iterationInfo, ref middleInterval);
            }
            else if (_f(x2) < _f(middleInterval))
            {
                iterationInfo.LeftBound = middleInterval;
                middleInterval = x2;
                ShowIteration(iterationInfo);
                iterationInfo.Iteration++;
                RunIterations(iterationInfo, ref middleInterval);
            }

            return middleInterval;
        }

    }
}
