using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Singularity.JsonService.Extensions
{
	public static class FileInfoExtension
	{
		public static T ReadJsonFile<T>(this FileInfo sourceJsonFileInfo, Boolean rootedObject = false)
		{
			using (StreamReader reader = new StreamReader(sourceJsonFileInfo.FullName))
			{
				String jsonText = reader.ReadToEnd();
				if (rootedObject)
				{
					String rootValue = JObject.Parse(jsonText).SelectToken(typeof(T).Name).ToString();
					return JsonConvert.DeserializeObject<T>(rootValue);
				}
				return JsonConvert.DeserializeObject<T>(jsonText);
			}
		}

		public static void WriteJsonFile<T>(this FileInfo targetJsonFileInfo, T entity)
		{
			JsonSerializer serializer = new JsonSerializer();
			using (StreamWriter writer = new StreamWriter(targetJsonFileInfo.FullName))
			using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
			{
				serializer.Serialize(jsonWriter, entity);
			}
		}
	}
}
