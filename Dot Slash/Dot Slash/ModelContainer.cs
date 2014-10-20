using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dot_Slash
{
	class ModelContainer
	{
		List<Model> models;

		public ModelContainer()
		{
			models = new List<Model>();
		}

		public Boolean addModel(String name, String file)
		{
			Model proto = new Model(name, file);
			if(models.Contains(proto))
				return false;
			else
			{
				models.Add(proto);
				return true;
			}
		}

		private class Model
		{
			String modelName;
			String classifier;
			public Model(String name, String file)
			{
				modelName = name;
				classifier = file;
			}

			// override object.Equals
			public override bool Equals (object obj)
			{

				if (obj == null || GetType() != obj.GetType()) 
				{
					return false;
				}
        
				Model proto = (Model) obj;
				if(!proto.modelName.Equals(this.modelName))
					return false;
				return true;
			}
		}
	}
}
