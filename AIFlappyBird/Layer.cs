using System;
using System.Collections.Generic;
using System.Text;

namespace AIFlappyBird
{
    public class Layer
    {
        public Neuron[] Neurons { get; }
        public double[] Outputs { get; }

        public Layer(int neuronCount, Layer previousLayer) 
        {
            Neurons = new Neuron[neuronCount];
            Outputs = new double[neuronCount];

            for(int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron(previousLayer.Neurons);
            }
        }

        public Layer(int neuronCount)
        {
            Neurons = new Neuron[neuronCount];
            Outputs = new double[neuronCount];

            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron();
            }
        }

        public void Randomize(Random random, double min, double max) 
        {
            foreach(Neuron n in Neurons)
            {
                n.Randomize(random, min, max);
            }
        }
        public double[] Compute() 
        {
            double[] rtrn = new double[Neurons.Length];
            for(int i = 0; i < Neurons.Length; i++)
            {
                rtrn[i] = Neurons[i].Compute();
            }

            return rtrn;
        }
    }
}
