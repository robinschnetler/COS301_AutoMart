package com.yoco;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;


public class MainActivity extends Activity {

	ImageView pictureFrame;
	
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }


    public void takePicture(View view)
    {
    	Intent pictureIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
    	if(pictureIntent.resolveActivity(getPackageManager()) != null)
    	{
    		File photoFile = null;
    		try
    		{
    			photoFile = createImageFile();
    		}
    		catch(IOException e)
    		{
    			
    		}
    		if(photoFile != null)
    		{
    			pictureIntent.putExtra(MediaStore.EXTRA_OUTPUT, Uri.fromFile(photoFile));
    			startActivityForResult(pictureIntent, 0);
    		}
    	}
    }
    
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data)
    {
    	super.onActivityResult(requestCode, resultCode, data);
    	AlertDialog.Builder ab = new AlertDialog.Builder(this);
    	Bitmap b;
    	if(data != null)
    	{
    		b = (Bitmap) data.getExtras().get("data");
    		pictureFrame.setImageBitmap(b);
    	}
    }
    
    String currentPhotoPath;
    private File createImageFile() throws IOException
    {
    	String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").format(new Date());
    	String imageFileName = "JPEG_" + timeStamp + "_";
    	File storageDir = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES);
    	File image = File.createTempFile(imageFileName, ".jpg", storageDir);
    	
    	currentPhotoPath = "file:"+image.getAbsolutePath();
    	return image;
    }
    
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        pictureFrame = (ImageView) findViewById(R.id.pictureframe);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if (id == R.id.action_settings) {
            return true;
        }
        return super.onOptionsItemSelected(item);
    }
}
