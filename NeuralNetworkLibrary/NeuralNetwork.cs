using System;

namespace NeuralNetworkLibrary
{
    public class ErrorFunction
    {
        Func<double, double, double> function;
        Func<double, double, double> derivative;
        public ErrorFunction(Func<double, double, double> function, Func<double, double, double> derivative)
        {
            this.function = function;
            this.derivative = derivative;
        }

        public ErrorFunction()
        {

        }

        public double Function(double a, double d)
        {
            return Math.Pow(d - a, 2);
        }
        public double Derivative(double a, double d)
        {
            return -2 * (d - a);
        }
    }

    public class NeuralNetwork
    {
        public Layer[] Layers;
        ErrorFunction errorFunc;
        public double Fitness { get; set; }

        public NeuralNetwork(params int[] neuronsPerLayer)
        {
            errorFunc = new ErrorFunction();
            Layers = new Layer[neuronsPerLayer.Length];
            Fitness = 1;

            Layers[0] = new Layer(neuronsPerLayer[0]);
            for (int i = 1; i < Layers.Length; i++)
            {
                Layers[i] = new Layer(neuronsPerLayer[i], Layers[i - 1]);
            }
        }
        public void Randomize(Random random, double min, double max)
        {
            foreach (Layer l in Layers)
            {
                l.Randomize(random, min, max);
            }
        }
        public double[] Compute(double[] inputs)
        {
            double[] outputs = new double[Layers[Layers.Length - 1].Neurons.Length];

            for (int i = 0; i < Layers[0].Neurons.Length; i++)
            {
                Layers[0].Neurons[i].Output = inputs[i];
            }

            for (int i = 1; i < Layers.Length; i++)
            {
                if (i >= Layers.Length - 1)
                {
                    outputs = Layers[i].Compute();
                }
                else
                {
                    Layers[i].Compute();
                }
            }

            return outputs;
        }
        public double GetError(double[] inputs, double[] desiredOutputs)
        {
            double rtrn = 0;
            double[] a = Compute(inputs);

            for (int i = 0; i < desiredOutputs.Length; i++)
            {
                rtrn += Math.Abs(desiredOutputs[i] - a[i]);
            }

            return rtrn / desiredOutputs.Length;
        }
    }
}
