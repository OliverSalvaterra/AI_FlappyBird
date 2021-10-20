using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIFlappyBird
{
    public class ActivationFunction
    {
        public ActivationFunction()
        {
        }

        public double Function(double input)
        {
            return 1 / (1 + Math.Pow(Math.E, -input));
        }

        public double Derivative(double input)
        {
            double func = 1 / (1 + Math.Pow(Math.E, -input));
            return func * (1 - func);
        }
    }

    public class Neuron
    {
        public double bias;
        public Dendrite[] dendrites;
        public double Output { get; set; }
        public double Input { get; private set; }
        public ActivationFunction Activation { get; set; }

        public Neuron(Neuron[] previousNeurons) 
        {
            dendrites = new Dendrite[previousNeurons.Length];

            for(int i = 0; i < dendrites.Length; i++)
            {
                dendrites[i] = new Dendrite(previousNeurons[i], this, 1);
            }

            Activation = new ActivationFunction();
        }

        public Neuron()
        {
            Activation = new ActivationFunction();
        }

        public void Randomize(Random random, double min, double max) 
        {
            if(dendrites != null)
            {
                foreach(Dendrite d in dendrites)
                {
                    d.Weight = random.NextDouble(min, max);
                }
            }
            
            bias = random.NextDouble(min, max);
        }
        public double Compute() 
        {
            double rtrn = 0;
            
            foreach(Dendrite d in dendrites)
            {
                rtrn += d.Compute();
            }

            Output = Activation.Function(rtrn + bias);
            return Output;
        }
    }
}
