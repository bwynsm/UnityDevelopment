using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Examples

[Tiled2Unity.CustomTiledImporter]
class CustomImporterAddComponent : Tiled2Unity.ICustomTiledImporter
{
    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {


		if ((props.ContainsKey("unity:sortingLayerName") && props["unity:sortingLayerName"] == "Player") || props.ContainsKey("Player"))
		{
			//gameObject.AddComponent(props["AddComp"]);
			//gameObject.AddComponent<SortObjects> ();
		}
    }


    public void CustomizePrefab(GameObject prefab)
    {
		// get all of the mesh renderers
		var renderers = prefab.GetComponentsInChildren<MeshRenderer>();

		foreach (var rendererItem in renderers)
		{
			// get the sorting layer
			GameObject gameObjectItem = rendererItem.gameObject;

			if (rendererItem.sortingLayerName == "Player")
			{
				rendererItem.gameObject.AddComponent<SortObjects> ();
			}
		}

    }
}
