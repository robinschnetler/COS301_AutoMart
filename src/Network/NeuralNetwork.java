/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Network;

import Colour.Profiler;
import Exception.InputErrorExceptions;
import Persister.Persister;
import java.util.Random;


/**
 *
 * @author Yashu
 */
public class NeuralNetwork 
{
	//constants
	double learningRate;
	double momentumRate;
	int numEpochs;
	double threshold;
	double smoothConstant;
	
	double[][] inputs;
	boolean[] classifications;
	double[][] generalizationSet;
	Persister networkDescription;
	Persister weightData;
	Persister outputData;
	
	double trainingAccuracy;
	double generalisationAccuracy;
	boolean complicated = true;
	
	Node[][] hiddenLayers;
	Node[] outputLayer;
	
	public static int NUM_HIDDEN_NODES = 5;
	public static final int NUM_OUTPUT_NODES = 2;
	public static int NUM_HIDDEN_LAYERS = 1;
	public static final int CARTOON = 0;
	public static final int PICTURE = 1;
	
	/**
	 * The constructor for the Neural Network that will classify an image into either
	 * a cartoon or a photo. The Neural Network will be a feed forward network
	 * using back propogation for learning. 
	 * @param _inputs Input doubles into the network. consisting of pixels of the image profiled
	 * into buckets that categorize the pixel color into buckets that represent a color in a certain ranged
	 * @see Profiler.java
	 * Each row in the 2 dimensional array represents the inputs for each image that will be used for learning
	 * @param _classifications this array allows for us to teach the neural network. it gives the correct classifications
	 * of the _inputs
	 * 
	 * TRUE = PICTURE
	 * FALSE = CARTOON
	 * 
	 * OutputNode[0] = cartoon
	 * OutputNode[1] = picture
	 * @param _learningRate The factor by which the back propogated error value will be scaled by when adjusting weights
	 * between neurons
	 * @param _momentumRate The factor by which the same weight will be adjusted in the current iteration.
	 * This value depends on how much the previous(iteration) change in weight was.
	 * @param _numEpochs The maximum number of iterations the neural network must make before storing the weights it has adapted to.
	 * Each Epoch consists of the Neural network iterating through the entire set of images. 
	 * @param _threshold The threshold value will be used by the neurons for the sigmoid function
	 * @param _smoothConstant The constant that will be applied by the sigmoid function in order to increase or decrease the steepness of the function
	 * @param _generalizationSet this consists of all the images that have not been classified before hand. The neural Network must now classify them
	 * @param _numLayers allows us to have more than one hidden layer in the neural network
	 * @param _numHidden specifies how many nodes there will be in every hidden layer
	 * @see NeuralNetwork.Node
	 * @see Persister.Persister.java
	 */
	public NeuralNetwork(double[][] _inputs, boolean[] _classifications, double _learningRate, double _momentumRate, int _numEpochs, double _threshold, double _smoothConstant, double[][] _generalizationSet, int _numLayers, int _numHidden) throws InputErrorExceptions
	{
		//System.out.println(_learningRate  + " " + _momentumRate + " " + _numEpochs);
		NUM_HIDDEN_LAYERS = _numLayers;
		NUM_HIDDEN_NODES = _numHidden;
		inputs = _inputs;
		System.out.println("input length: " + inputs[0].length);
		classifications = _classifications;
		if(inputs.length != classifications.length)
		{
			System.out.println("exeption thrown");
			throw new InputErrorExceptions("number of classifications and inputs do not match");
		}
		learningRate = _learningRate;
		momentumRate = _momentumRate;
		numEpochs = _numEpochs;
		networkDescription = new Persister("Neural_Network_Class_Description.csv");
		weightData = new Persister("Neural_Network_Class_Weight_Data.csv");
		outputData = new Persister("Neural_Network_Class_Output_Data.csv");
		hiddenLayers = new Node[NUM_HIDDEN_LAYERS][NUM_HIDDEN_NODES];
		outputLayer = new Node[NUM_OUTPUT_NODES];
		threshold = _threshold;
		smoothConstant = _smoothConstant;
		generalizationSet = _generalizationSet;
	}
	
	/**
	 * This function implements the Neural Network feed forward - back-propogation algorithm.
	 * @return true if all works well, false otherwise
	 */
	public boolean teach(int repeat)
	{
		System.out.println("num Repeat: " + repeat);
		for (int z = 0; z < repeat; z++) 
		{
			Persister data = new Persister("Run"+z+".csv");
			if(!initialiseWeights())
				return false;
			weightData.write("initial weights");
			for (int i = 0; i < numEpochs; i++) 
			{
				trainingAccuracy = 0;
				for (int j = 0; j < inputs.length; j++) 
				{
					//data is sent throuh the network and everything is initialised
					if(!feedForward())
						return false;
					//this is where we tell the network what answer we want from it
					if(!setClassifier(j))
						return false;
					//we check if the output is what we expected
					if(checkOutputNodes())
						trainingAccuracy++;
					//we update the weights from Hidden Layer to Output layer
					if(!updateOutputToHiddenWeights())
						return false;
					if(!updateHiddenLayerSignals())
						return false;
					//we update the weights within the hidden layers
					if(!updateHiddenToHiddenWeights())
						return false;
					//we update the weights from the input layer to the hidden layer
					if(!updateInputToHiddenWeights())
						return false;
				}
				trainingAccuracy = trainingAccuracy / inputs.length * 100;

				classify();
				
				//write data to file for later documentation use
				persistDataWeights();
				persistDataOutput(i);
				persistExecution(data, i);

			}
		}
		return true;
	}
	
	public void persistExecution(Persister p, int i)
	{
		p.write(i + "," + trainingAccuracy + ", "+i+"," + generalisationAccuracy+ "\n");
		p.flush();
	}
	
	
	/**
	 * this function flips the switch allowing the user to use the sigmoid function without threshold and 
	 * smoothness as parameters as well.
	 */
	public void switchComplicated()
	{
		complicated = (complicated)?false:true;
	}
	
	
	/**
	 * Neural Network tries to classify the unseen data in this function
	 * @return false if there are any hiccups, else true;
	 */
	public boolean classify()
	{
		try
		{
			generalisationAccuracy = 0;
			for (int i = 0; i < generalizationSet.length; i++) 
			{
				for (int j = 0; j < hiddenLayers[0].length; j++) 
				{
					hiddenLayers[0][j].updateInputs(generalizationSet[i]);
				}
				if(!feedForward())
					return false;
				if(checkOutputNodes())
					generalisationAccuracy++;
			}
			generalisationAccuracy = generalisationAccuracy / generalizationSet.length * 100;
			return true;
		}
		catch(NullPointerException n)
		{
			System.out.println("null pointer exception in classify() in NeurelNetwork.java");
			return false;
		}
		catch(ArrayIndexOutOfBoundsException n)
		{
			System.out.println("ArrayIndexOutOfBounds Exception in classify() in NeurelNetwork.java");
			n.printStackTrace();
			return false;
		}
	}
	
	/**
	 * update the weights leading from the input layer to the first hidden layer
	 * @return 
	 */
	public boolean updateInputToHiddenWeights()
	{
		try
		{
			for (int i = 0; i < hiddenLayers[0].length; i++) 
			{
				for (int j = 0; j < hiddenLayers[0][i].inputs.length; j++) 
				{
					double[] _inputs = hiddenLayers[0][i].getInputs();
					//get the previous change in weight
					double change = hiddenLayers[0][i].getChange(j);
					//calculate the new change using momentum and learning rate
					change = (double)(((-1) *  learningRate * hiddenLayers[0][i].sigmoid()*(_inputs[j])) +  momentumRate * (change));
					//update the weight with the change
					hiddenLayers[0][i].updateWeight(j, change);
				}
			}
			return true;
		}
		catch(NullPointerException n)
		{
			System.out.println("null pointer exception in updateInputToHiddenWeights()");
			return false;
		}
		catch(ArrayIndexOutOfBoundsException a)
		{
			a.printStackTrace();
			System.out.println("Array index out of bounds exception in updateInputToHiddenWeights()");
			return false;
		}
	}
	
	/**
	 * alow the user to see what a selected image is, a cartoon or a picture.
	 * Should only be called after the neural network has already been taught
	 * @param _inputs input vector to the first hidden layer
	 * @return string representation of if the image if a cartoon or a picture
	 */
	public String classifySingleImage(double[] _inputs)
	{
		for (int j = 0; j < hiddenLayers[0].length; j++) 
		{
			hiddenLayers[0][j].updateInputs(_inputs);
		}
		String out = "classification Error";
		if(!feedForward())
			return "Classification Error.";
		boolean c = outputLayer[CARTOON].assignClass();
		boolean p = outputLayer[PICTURE].assignClass();
		//both neurons are fired
		if(c & p)
			out = "classification error";
		else if(p)
		{
			out = "image is a picture";
		}
		else if(c)
			out = "image is a cartoon";
		//no neurons are fired
		else
			out = "unclassified";
		out += "\nCartoon: " + outputLayer[CARTOON].getActivationValue() + "\npicture: " + outputLayer[PICTURE].getActivationValue() + "\n"; 
		return out;
	}
	
	/**
	 * Updates the weights of the connections between all the hidden layers of the neural network
	 * it is a useless function if you are going to use only one hidden layer in the neural network
	 * @return true if all went well, else false
	 */
	public boolean updateHiddenToHiddenWeights()
	{
		try
		{
			for (int i = NUM_HIDDEN_LAYERS-1; i > 0; i--) 
			{
				for (int j = 0; j < NUM_HIDDEN_NODES; j++) 
				{
					for (int k = 0; k < hiddenLayers[i-1].length; k++) 
					{
						//get the previous change in weight
						double change = hiddenLayers[i][j].getChange(k);
						//calculate the new change using momentum and learning rate
						change = (double)(((-1) *  learningRate * hiddenLayers[i][j].sigmoid()*(hiddenLayers[i-1][j].getActivationValue())) +  momentumRate * (change));
						
						//System.out.println(learningRate + " " +  momentumRate + " " +  change + " " );
						
						//update the weight with the change
						hiddenLayers[i][j].updateWeight(k, change);
					}
				}
			}
			return true;
		}
		catch(NullPointerException n)
		{
			System.out.println("null pointer exception in updateHiddenToHiddenWeights()");
			return false;
		}
		catch(ArrayIndexOutOfBoundsException a)
		{
			a.printStackTrace();
			System.out.println("Array index out of bounds exception in updateHiddenToHiddenWeights()");
			return false;
		}
	}
	
	/**
	 * Updates the weights of the connections between last layer in the hidden layers and the output layer of the neural network
	 * @return true if all went well, else false
	 */
	public boolean updateOutputToHiddenWeights()
	{
		try
		{
			for (int i = 0; i < NUM_OUTPUT_NODES; i++) 
			{
				int index = NUM_HIDDEN_LAYERS-1;
				for (int j = 0; j < hiddenLayers[index].length; j++) 
				{
					//get the previous change in weight
					double change = outputLayer[i].getChange(j);
					
					//calculate the new change using momentum and learning rate
					change = ((-1) * learningRate * outputLayer[i].getSignal()*(hiddenLayers[index][j].getActivationValue()) +  momentumRate * (change));
					//update the weight with the change
					outputLayer[i].updateWeight(j, change);
				}
			}
			return true;
		}
		catch(NullPointerException n)
		{
			System.out.println("null pointer exception in updateOutputToHiddenWeights()");
			return false;
		}
		catch(ArrayIndexOutOfBoundsException a)
		{
			System.out.println("Array index out of bounds exception in updateOutputToHiddenWeights()");
			return false;
		}
	}
	
	/**
	 * This function first assigns a new error signal for the last layer of the hidden layers based on the function :
	 * err(yj) = sum(k = i -> K)[err(Ok)*weight(kj)*(1 - yj) * yj]
	 * @return true if all went well, else false
	 */
	public boolean updateHiddenLayerSignals()
	{
		try
		{
			//update signal for last layer in hidden layers
			int index = NUM_HIDDEN_LAYERS-1;
			for (int i = 0; i < hiddenLayers[index].length; i++) 
			{
				double signal = 0;
				for (int j = 0; j < NUM_OUTPUT_NODES; j++) 
				{
					double hiddenActivation = hiddenLayers[index][j].getActivationValue();
					signal += (outputLayer[j].getSignal() * outputLayer[j].getWeight(i) * (1 - hiddenActivation) * hiddenActivation);
				}
				hiddenLayers[index][i].updateSignal(signal);
			}
			//now update signals along all hidden layers
			for (int i = NUM_HIDDEN_LAYERS-1; i > 0; i--) 
			{
				for (int k = 0; k < hiddenLayers[i].length; k++) 
				{
					double signal = 0;
					for (int j = 0; j < NUM_HIDDEN_NODES; j++) 
					{
						double hiddenActivation = hiddenLayers[i-1][j].getActivationValue();
						signal += (hiddenLayers[i][j].getSignal() * hiddenLayers[i][j].getWeight(i) * (1 - hiddenActivation) * hiddenActivation);
					}
					hiddenLayers[i-1][k].updateSignal(signal);
				}
			}
			return true;
		}
		catch(ArrayIndexOutOfBoundsException e)
		{
			System.out.println("Array error in NeuralNetwork.UpdateHiddenLayerSignals()");
			return false;
		}
		catch(NullPointerException n)
		{
			System.out.println("Null pointer exception in NeuralNetwork.UpdateHiddenLayerSignals()");
			return false;
		}
	}
	
	/**
	 * check for correct classification of the image.
	 * @return true if all output nodes correctly classify, else false
	 */
	public boolean checkOutputNodes()
	{
		for (int i = 0; i < NUM_OUTPUT_NODES; i++) 
		{
			if(!outputLayer[i].checkClassification())
				return false;
		}
		return true;
	}
	
	
	/**
	 * check classification of image at number index in classifications[] array and set the output nodes accordingly
	 * if classification is true at that index, the image is a picture, and not a cartoon
	 * @param number index of image to be classified
	 * @return true if all goes well, else false;
	 */
	public boolean setClassifier(int number)
	{
		try
		{
			if(classifications[number])
			{	
				outputLayer[CARTOON].setTarget( 0);
				outputLayer[PICTURE].setTarget( 1);
			}
			else
			{
				outputLayer[CARTOON].setTarget( 1);
				outputLayer[PICTURE].setTarget( 0);
			}
			return true;
		}
		catch(NullPointerException n)
		{
			System.out.println("null pointer exception in setClassifier() NeuralNetwork.java");
			return false;
		}
		catch(ArrayIndexOutOfBoundsException n)
		{
			System.out.println("ArrayIndexOutOfBounds exception in setClassifier() NeuralNetwork.java");
			return false;
		}
	}
	
	/**
	 * feed forward phase of the neural network learning.
	 * @param number the index of the image we are working on currently
	 * @return true if all goes well and false if any exception is thrown
	 */
	public boolean feedForward()
	{
		try
		{
			//feed forward through the hidden layers
			//unnesaccary if there is only one hidden layer but useful for further development
			for (int j = 0; j < NUM_HIDDEN_LAYERS-1; j++) 
			{
				//new inputs for next layer is current node's input layer
				double[] newinputs = new double[hiddenLayers[j].length];
				//loop through each node in the layer
				for (int k = 0; k < hiddenLayers[j].length; k++) 
					//set the new input to that node's activation value
					newinputs[k] = hiddenLayers[j][k].getActivationValue();
				for (int k = 0; k < NUM_HIDDEN_NODES; k++) 
				{
					hiddenLayers[j+1][k].updateInputs(newinputs);
				}
			}
			
			//taking outputs of last hidden layer*
			double[] newInputs = new double[hiddenLayers[NUM_HIDDEN_LAYERS-1].length];
			for (int k = 0; k < hiddenLayers[NUM_HIDDEN_LAYERS-1].length; k++) 
			{
				newInputs[k] = hiddenLayers[NUM_HIDDEN_LAYERS-1][k].getActivationValue();
			}
			
			//* and placing them as inputs to the output layer
			for (int j = 0; j < NUM_OUTPUT_NODES; j++) {
				outputLayer[j].updateInputs(newInputs);
			}
			return true;
		}
		catch(ArrayIndexOutOfBoundsException e)
		{
			e.printStackTrace();
			System.out.println("problem in feedForward() in NeuralNetwork.java");
			return false;
		}
			
	}
	
	/**
	 * write output data to .csv in the following format
	 * node0, output0,target0, diff0,
	 * node1, output1,target1, diff1,
	 * ..
	 * ..
	 * noden, outputn, targetn, diffn
	 * 
	 * with all outputn being the output Activation, targetn being the target activation, and diffn being the difference
	 * between targetn and outputn
	 */
	public void persistDataOutput(int i)
	{
		outputData.write("epoch," + i + ",training_accuracy, "+ trainingAccuracy + ", generalisation_Accuracy, " + generalisationAccuracy+"\n");
		outputData.flush();
	}
	
	/**
	 * write weights data to .csv file in following format:
	 * layer0, w0, w1, w2, .., wm,
	 * ..
	 * ..
	 * layern, w0, w1, w2, .., wm, 
	 * 
	 * with all weights w0..wm being the average mean of the node in that layer 
	 */
	public void persistDataWeights()
	{
		String out = "";
		for (int i = 0; i < NUM_HIDDEN_LAYERS; i++) 
		{
			out += "hidden " + i+ ",";
			for (int j = 0; j < hiddenLayers[i].length; j++) 
			{
				Node[] nodes = hiddenLayers[i];
				for(Node n: nodes)
				{
					out += n.getActivationValue()+ ",";
				}
			}
			out += "\n";
		}
		out += "output ,";
		Node[] outputs = outputLayer;
		for(Node o:outputs)
		{
			out += o.getActivationValue()+ ",";
		}
		out += "\n";
		weightData.write(out);
		weightData.flush();
	}
	
	/**
	 * this function will initialize all the Node objects in the hidden and output layers
	 * @return 
	 */
	public boolean initialiseWeights()
	{
		try
		{
			for (int i = 0; i < NUM_HIDDEN_NODES; i++) {
				hiddenLayers[0][i] = new Node(inputs[0].length, inputs[0]);
			}
			for (int i = 1; i < hiddenLayers.length; i++) 
			{
				for (int j = 0; j < hiddenLayers[i].length; j++)
				{
					hiddenLayers[i][j] = new Node(hiddenLayers[i-1].length);
				}
			}
			for (int i = 0; i < outputLayer.length; i++) 
			{
				outputLayer[i] = new Node(hiddenLayers[hiddenLayers.length-1].length);
			}
			return true;
		}
		catch(NullPointerException e)
		{
			System.out.println("something went wrong in NeuralNetwork.initialiseWeights()");
			return false;
		}
	}
	
	/**
	 * This class represents the Neuron that will form part of the Neural Network
	 */
	private class Node
	{
		double[] weights;
		double[] inputs;
		double[] changes;
		double output;
		double target;
		double signal;
		
		/**
		 * update input values. This will be called per image per node.
		 * bias unit is also set as -1
		 * 
		 * Also calls the sigmoid function to update the output global value for future use in the program
		 * @param _inputs double vector consisting of the double outputs of the layer before it
		 */
		public void updateInputs(double[] _inputs)
		{
			System.arraycopy(_inputs, 0, inputs, 0, _inputs.length);
			inputs[inputs.length-1] = -1;
			sigmoid();
		}
		
		public double getChange(int i)
		{
			return changes[i];
		}
		
		/**
		 * updates signal of node to new signal based on error values of other connected neurons
		 * @param value the new signal value
		 */
		public void updateSignal(double value)
		{
			signal = value;
		}
		
		
		/**
		 * see whether a neuron is actually fires
		 * @return boolean value that represent a neuron going off
		 */
		public boolean assignClass()
		{
			if(output > 0.7)
				return true;
			if(output > 3)
				return false;
			return false;
		}
		
		/**
		 * This checks whether the neuron correctly classified the image. if the target and output are different by 
		 * only 0.3 and the output is not between 0.3 and 0.7 it is correctly classified.
		 * @return 0 if correctly classified else (target value - Activation value)
		 */
		public boolean checkClassification()
		{
			if((target == 0 && output < 0.3) || (target == 1 && output > 0.7))
			{
				signal = (-1) * (target - output) *  (1 - output) * output;
				return true;
			}
			else
			{
				signal = (-1) * (target - output) *  (1 - output) * output;
				return false;
			}
		}
		
		/**
		 * the target value is what we want our output to be as close to as possible
		 * @param t target value that must be set for output nodes 
		 */
		public void setTarget(double t)
		{
			target = t;
		}
		
		/**
		 * output is knows as Activation Value in Neural Networks
		 * @return Activation value of Neuron
		 */
		public double getActivationValue()
		{
			return output;
		}
		
		/**
		 * get weight of a certain neuron connection
		 * @param index index of neurons input value
		 * @return weight
		 */
		public double getWeight(int index)
		{
			return weights[index];
		}
		
		//self explanatory
		public double[] getInputs()
		{
			return inputs;
		}
		
		
		/**
		 * sigmoid function that uses the weighted sum of the inputs and normalizes it to a range [0, 1]
		 * @return output value
		 */
		public double sigmoid()
		{
			double total = 0;
			for (int i = 0; i < inputs.length; i++) 
				total += inputs[i] * weights[i];
			if(complicated)
				return output =  (1 / ( 1 + Math.exp( ( (total - threshold) / smoothConstant ) *-1) ));
			else
				return output =  (1 / ( 1 + Math.exp( (total) * -1)));
		}
		
		/**
		 * updates weight associated with a specific neuron connection
		 * @param index index referencing the specific neuron connected to this weight
		 * @param change the amount by which the current weight must be updated 
		 */
		public void updateWeight(int index, double change)
		{
			changes[index] = change;
			weights[index] += changes[index];
		}
		
		/**
		 * @return all weights
		 */
		public double[] getWeights()
		{
			return weights;
		}
		
		
		//self explanatory
		public double getSignal()
		{
			return signal;
		}
		
		
		/**
		 * get the average value of all weights by adding all weights and dividing but number of weights
		 * @return mean value of weights
		 */
		public double getMean()
		{
			double total = 0;
			for(double f: weights)
			{
				total += f;
			}
			return total /= weights.length;
		}
		
		/**
		 * constructor creates node object and randomly sets weight values based on the number of inputs
		 * that the node will be using
		 * @param numInputs number of input values that will be used for calculations later
		 * @param _inputs input vector consisting of outputs of the layer before or initial input
		 */
		public Node(int numInputs, double[] _inputs)
		{
			inputs = new double[numInputs+1];
			//copy over all values enteres
			System.arraycopy(_inputs, 0, inputs, 0, numInputs);
			inputs[numInputs++] = -1;
			changes = new double[numInputs];
			weights = new double[numInputs];
			Random random = new Random(System.nanoTime());
			for (int i = 0; i < numInputs; i++) 
			{
				double r = random.nextInt(10);
				//using Colour.Profiler.java normalize function... DRY principles ;)
				weights[i] = Profiler.normalize(r,  (1/(Math.sqrt(numInputs) *-1)), (1/(Math.sqrt(numInputs))), 0, 10);
			}
			output = 0;
		}
		
		/**
		 * This constructor will be called for all layers except the first hidden layer as input values to other layers
		 * is unknown beforehand.
		 * @param numInputs number of nodes in previous layer
		 */
		public Node(int numInputs)
		{
			inputs = new double[++numInputs];
			weights = new double[numInputs];
			changes = new double[numInputs];
			Random random = new Random(System.nanoTime());
			for (int i = 0; i < numInputs; i++) 
			{
				double r = random.nextInt(10);
				weights[i] = Profiler.normalize(r, (1/(Math.sqrt(numInputs) *-1)), (1/(Math.sqrt(numInputs))), 0, 10);
			}
			output = 0;
		}
	}
	
}
