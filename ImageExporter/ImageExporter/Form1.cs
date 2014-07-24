using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;

namespace ImageExporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] bytImage = null;
            string constring = @"Data Source=localhost;Initial Catalog=AutomartImages;Integrated Security=True";

            SqlCommand command = new SqlCommand(@"SELECT advert.AdvertID, advert.MainImage
FROM [AutomartImages].[dbo].[DSAdvertImage] AS advert
INNER JOIN
(SELECT table2.AdvertID, table2.CategoryID 
FROM [AutomartImages].[dbo].[DSAdvert] AS table2
WHERE CategoryID = 172 or CategoryID = 173 or CategoryID = 119 or CategoryID = 120 or CategoryID = 121 or CategoryID = 122) AS images
ON advert.AdvertID = images.AdvertID");

            command.CommandType = CommandType.Text;
            SqlConnection myconn = new SqlConnection(constring);

            command.Connection = myconn;
            myconn.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                bytImage = (byte[])reader["MainImage"];
                String id = reader["AdvertID"].ToString();
                if (bytImage != null)
                {
                    MemoryStream ms = new MemoryStream(bytImage);
                    Image img = new Bitmap(ms);
                    String imgName = "adverts/" + id;

                    if(File.Exists(imgName + ".jpeg"))
                    {
                        int i = 0;
                        imgName += "_";
                        while(true)
                        {
                            i++;
                            if(File.Exists(imgName + i + ".jpeg"))
                            {
                                continue;
                            }
                            else
                            {
                                imgName += i;
                                break;
                            }
                        }
                    }
                    imgName += ".jpeg";
                    img.Save(imgName);
                }
            }
            System.Windows.Forms.MessageBox.Show("Extraction complete.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }            
    }
}
