/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Persister;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * This class provides and API to write data to a file. This will be in use for recording all the data that the
 * neural network provides during execution including all data and information that is fed into the network
 * @author Yashu
 */
public class Persister {
	File file;
	FileWriter fw;
	BufferedWriter bw;
	public Persister(String f)
	{
		try 
		{
			file = new File("data\\"+ f);
			// if file doesnt exists, then create it
			if (!file.exists()) {
				file.createNewFile();
			}
			fw = new FileWriter(file.getAbsoluteFile());
			bw = new BufferedWriter(fw);
		} 
		catch (IOException e) 
		{
			System.out.println("Problem creating persister");
		}
	}
	
	
	//self explanatory
	public String getFilename()
	{
		return file.getAbsolutePath();
	}
	
	/**
	 * Write whatever is sent to the persister object to the file created in the constructor
	 * @param s string to be written
	 * @return true if string was able to be written, false if an exception is thrown
	 */
	public boolean write(String s)
	{
		try 
		{
			bw = new BufferedWriter(fw);
			bw.write(s);
			return true;
		} 
		catch (IOException ex) 
		{
			Logger.getLogger(Persister.class.getName()).log(Level.SEVERE, null, ex);
			System.out.println("Problem writing to " + file.getAbsolutePath() + ".");
			return false;
		}
	}
	
	public boolean close()
	{
		try
		{
			bw.close();
			return true;
		}
		catch(IOException e)
		{
			System.out.println("IOException caught in close() in Persister.java");
			return false;
		}
	}
	
	
	/**
	 * writes everything that is in the buffer out.
	 * @return  true if the buffer empties out without any errors, else false;
	 */
	public boolean flush()
	{
		try
		{
			bw.flush();
			return true;
		}
		catch(IOException e)
		{
			System.out.println("IOException caught in close() in Persister.java");
			return false;
		}
	}
}
