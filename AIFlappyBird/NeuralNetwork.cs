using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Text;


namespace AIFlappyBird
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

    public class GeneticTrain
    {
        Func<double[], double> f;
        public NeuralNetwork[] networks { get; set; }

        public GeneticTrain(Func<double[], double> f, int[] networkSize, int batchSize)
        {
            this.f = f;
            networks = new NeuralNetwork[batchSize];

            for (int i = 0; i < batchSize; i++)
            {
                networks[i] = new NeuralNetwork(networkSize);
            }
        }

        public void Mutate(NeuralNetwork n, Random random, double mutationRate)
        {
            for (int j = 1; j < n.Layers.Length; j++) // EXCLUDE THE FIRST LAYER
            {
                foreach (Neuron neuron in n.Layers[j].Neurons)
                {
                    //Mutate the Weights
                    for (int i = 0; i < neuron.dendrites.Length; i++)
                    {
                        if (random.NextDouble() < mutationRate)
                        {
                            if (random.Next(2) == 0)
                            {
                                neuron.dendrites[i].Weight *= random.NextDouble(0.2, 2.1); //scale weight
                            }
                            else
                            {
                                neuron.dendrites[i].Weight *= -1; //flip sign
                            }
                        }
                    }

                    //Mutate the Bias
                    if (random.NextDouble() < mutationRate)
                    {
                        if (random.Next(2) == 0)
                        {
                            neuron.bias *= random.NextDouble(0.5, 1.5); //scale weight
                        }
                        else
                        {
                            neuron.bias *= -1; //flip sign
                        }
                    }
                }
            }
        }

        public void Crossover(NeuralNetwork winner, NeuralNetwork loser, Random random)
        {
            for (int i = 1; i < winner.Layers.Length; i++) // array must start after input layer
            {
                //References to the Layers
                Layer winLayer = winner.Layers[i];
                Layer childLayer = loser.Layers[i];

                int cutPoint = random.Next(winLayer.Neurons.Length); //calculate a cut point for the layer
                bool flip = random.Next(2) == 0; //randomly decide which side of the cut point will come from winner

                //Either copy from 0->cutPoint or cutPoint->Neurons.Length from the winner based on the flip variable
                for (int j = (flip ? 0 : cutPoint); j < (flip ? cutPoint : winLayer.Neurons.Length); j++)
                {
                    //References to the Neurons
                    Neuron winNeuron = winLayer.Neurons[j];
                    Neuron childNeuron = childLayer.Neurons[j];

                    //Copy the winners Weights and Bias into the loser/child neuron
                    for (int n = 0; n < winNeuron.dendrites.Length; n++)
                    {
                        childNeuron.dendrites[n].Weight = winNeuron.dendrites[n].Weight;
                    }
                    childNeuron.bias = winNeuron.bias;
                }
            }
        }

        public double[] Train(Random random, double mutationRate) // returns best fitness
        {
            double[] best = new double[networks.Length];
            Array.Sort(networks, (a, b) => b.Fitness.CompareTo(a.Fitness));

            int start = (int)(networks.Length * 0.1);
            int end = (int)(networks.Length * 0.9);

            //Notice that this process is only called on networks in the middle 80% of the array
            for (int i = start; i < end; i++)
            {
                Crossover(networks[random.Next(start)], networks[i], random);
                Mutate(networks[i], random, mutationRate);
            }

            //Removes the worst performing networks
            for (int i = end; i < networks.Length; i++)
            {
                networks[i].Randomize(random, 0, 1);
            }

            for (int i = 0; i < networks.Length; i++)
            {
                best[i] = networks[i].Fitness;
            }

            return best;
        }

        public void SetFitness(double[][] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                networks[i].Fitness = f(data[i]);
            }
        }
    }
}
