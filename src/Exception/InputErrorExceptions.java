package Exception;

/**
 * This exception is thrown if a Neural network is not given the correct classification data for it's input 
 * @author Yashu
 */
public class InputErrorExceptions extends Exception
{
	public InputErrorExceptions(String message) {
		super(message);
	}	
}
