package Persister;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Random;

public class FileHandler 
{
	File file;
	ArrayList<Record> records;
	
	public FileHandler(String filename) throws IOException
	{
		file = new File(filename);
		records = new ArrayList();
		try
		{
			generateRecords();
			shuffleRecords();
		}
		catch(FileNotFoundException e)
		{
			System.out.println("File was not found");
		}
	}
	
	public void shuffleRecords()
	{
		Random rnd = new Random();
		for (int i = records.size() - 1; i > 0; i--)
		{
		  int index = rnd.nextInt(i + 1);
		  // Simple swap
		  Record a = records.get(index);
		  records.add(index, records.get(i));
		  records.add(i, a);
		}
	}

	public ArrayList<Record> getTeachingData()
	{
		int count = 0;
		int val = (int)(records.size() * 0.8);
		ArrayList<Record> send = new ArrayList<>();
		for (int i = 0; i < records.size(); i++) 
		{
			if(count > val)
				break;
			send.add(records.remove(0));
		}
		return send;
	}
	
	public ArrayList<Record> getTestingData()
	{
		int count = 0;
		int val = (int)(records.size() * 0.8);
		ArrayList<Record> send = new ArrayList<>();
		for (int i = 0; i < records.size(); i++) 
		{
			if(count > val)
				break;
			send.add(records.remove(0));
		}
		return send;
	}
	
	public ArrayList<Record> getUnseenData()
	{
		return records;
	}
	
	public String displayRecords()
	{
		String out = new String();
		for(int i = 0; i<records.size(); i++)
			out += records.get(i).toString() + "\n";
		return out;
	}
	
	public String[] getRecords()
	{
		String[] recs = new String[records.size()];
		for(int i = 0; i<recs.length; i++)
		{
			recs[i] = records.get(i).word;
		}
		return recs;
	}
	
	public boolean[] getClasses()
	{
		boolean[] recs = new boolean[records.size()];
		for (int i = 0; i < recs.length; i++) 
		{
			recs[i] = records.get(i).getIsPicture();
		}
		return recs;
	}
	
	private boolean generateRecords() throws FileNotFoundException, IOException
	{
		FileReader reader = new FileReader(file);
		BufferedReader br = new BufferedReader(reader);
		String line;
		while ((line = br.readLine()) != null) 
		{
			records.add(new Record(line));
		}
		br.close();
		return true;
	}
	
	public class Record
	{
		String word;
		boolean picture;
		
		public Record(String line)
		{
			if(line.contains("Photo"))
				picture = true;
			word = line;
		}
		
		public boolean getIsPicture()
		{
			return picture;
		}
	}
}
