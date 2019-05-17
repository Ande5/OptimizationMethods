namespace Bl
{
    public delegate void IterationInfoDelegate(object sender, IterationInfoEventArgs iterationInfoEventArgs);

    public abstract class Iteration
    {
        protected IterationInfoEventArgs IterationInfoEventArgs { get; set; }

        /// <summary>
        /// Левая граница
        /// </summary>
        public double LeftBound => IterationInfoEventArgs.LeftBound;

        /// <summary>
        /// Правая граница
        /// </summary>
        public double RightBound => IterationInfoEventArgs.RightBound;

        /// <summary>
        /// Кол-во итераций
        /// </summary>
        public int Iterations => IterationInfoEventArgs.Iteration;

        public event IterationInfoDelegate OnIteration;

        protected void ShowIteration(double leftBound, double rightBound, int iteration) 
            => OnIteration?.Invoke(this, new IterationInfoEventArgs(leftBound, rightBound, iteration));

        protected void ShowIteration(IterationInfoEventArgs iterationInfo)
            => OnIteration?.Invoke(this,
                new IterationInfoEventArgs(iterationInfo.LeftBound, iterationInfo.RightBound, iterationInfo.Iteration));
    }
}
