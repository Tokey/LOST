using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using UnityEditor;

namespace MoreMountains.CorgiEngine
{	
	/// <summary>
	/// Adds items to the More Mountains menu
	/// </summary>
	public static class MoreMountainsMenu 
	{
		[MenuItem("More Mountains/Asset's page",false,50)]
		/// <summary>
		/// Adds a asset's page link to the More Mountains menu
		/// </summary>
		private static void OpenAssetsPage()
	    {
			Application.OpenURL("http://www.moremountains.com/corgi-engine-best-unity-2d-platformer");
		}

		[MenuItem("More Mountains/Documentation",false,50)]
		/// <summary>
		/// Adds a documentation link to the More Mountains menu
		/// </summary>
		private static void OpenDocumentation()
	    {
			Application.OpenURL("http://reunono.github.io/CorgiEngine/");
		}

		[MenuItem("More Mountains/API Documentation",false,51)]
		/// <summary>
		/// Adds an API documentation link to the More Mountains menu
		/// </summary>
		private static void OpenAPIDocumentation()
	    {
			Application.OpenURL("http://www.moremountains.com/corgi-engine/docs/index.html");
		}

		[MenuItem("More Mountains/Video Tutorials",false,50)]
		/// <summary>
		/// Adds a youtube link to the More Mountains menu
		/// </summary>
		private static void OpenVideoTutorials()
	    {
			Application.OpenURL("https://www.youtube.com/playlist?list=PLl3caEhMYxQEsA5Fbg0M2aB9Q9Z9BTVNS");
		}

		[MenuItem("More Mountains/More assets by More Mountains",false,52)]
		/// <summary>
		/// Adds a store link to the More Mountains menu
		/// </summary>
		private static void OpenStorePage()
	    {
			Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:10305");
	    }
	}
}