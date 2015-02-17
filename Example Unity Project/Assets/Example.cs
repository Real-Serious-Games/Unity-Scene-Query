using UnityEngine;
using System.Collections;
using System.Linq;
using RSG.Scene.Query;

public class Example : MonoBehaviour 
{
    void Start () 
    {
		var sceneTraversal = new SceneTraversal();
		var sceneQuery = new SceneQuery();

		//
		// Traverse root objects.
		//
		var rootObjects = sceneTraversal.RootNodes();
		Debug.Log("Root game objects: " + string.Join(", ", rootObjects.Select (go => go.name).ToArray()));

		//
		// Traverse all scene objects.
		//
		var allObjects = sceneTraversal.PreOrderHierarchy();
		Debug.Log("All game objects: " + string.Join(", ", allObjects.Select (go => go.name).ToArray()));

        //
        // Find first object in scene named Cube.
        //
        var singleCube = sceneQuery.SelectOne("Cube");
        Debug.Log("Found cube: " + singleCube.name + " (#" + (uint)singleCube.GetInstanceID() + ")");

        // 
        // Find all objects named cube
        //
        var cubes = sceneQuery.SelectAll("cUbE"); // Note case-insensitivity.
        Debug.Log("Found cubes: " + string.Join(", ", cubes.Select(c => c.name).ToArray()));

        // 
        // Find all objects that start with 'sphere'.
        //
        var spheres = sceneQuery.SelectAll("?sphere");
        Debug.Log("Found spheres: " + string.Join(", ", spheres.Select(s => s.name).ToArray()));

        // 
        // Find all objects on a particular layer.
        //
        var gameObjectsOnLayer = sceneQuery.SelectAll(".MyTestLayer");
        Debug.Log("Found game objects by layer: " + string.Join(", ", gameObjectsOnLayer.Select(go => go.name).ToArray()));

        // 
        // Find all objects that are tagged.
        //
        var taggedGameObjects = sceneQuery.SelectAll(".MyTestTag");
        Debug.Log("Found game objects by tag: " + string.Join(", ", taggedGameObjects.Select(go => go.name).ToArray()));

        // 
        // Find exact object in hierarcy.
        //
        var exactGameObject = sceneQuery.SelectOne("/Parent/Sphere3/Cube");
        Debug.Log("Found exact game object: " + exactGameObject.name);
        
        // 
        // Find a game object some where under a particular parent.
        //
        var gameObjectSomewhereUnderParent = sceneQuery.SelectOne("Parent>Cube");
        Debug.Log("Found game object some where under parent: " + gameObjectSomewhereUnderParent.name);

		//
		// Traverse child game objects under a particular parent.
		//
		var childObjects = sceneTraversal.Children(sceneQuery.SelectOne("Parent"));
		Debug.Log("Children: " + string.Join(", ", childObjects.Select(go => go.name).ToArray()));

		//
		// Traverse all descendents under a particular parent.
		//
		var descendentObjects = sceneTraversal.Descendents(sceneQuery.SelectOne("Parent"));
		Debug.Log("Decendents: " + string.Join(", ", descendentObjects.Select(go => go.name).ToArray()));

		//
		// Traverse all ancestor in the hierarchy above a particular game object.
		//
		var ancestorsObjects = sceneTraversal.Ancestors(sceneQuery.SelectOne("Parent/Sphere3/Cube"));
		Debug.Log("Ancestors: " + string.Join(", ", ancestorsObjects.Select(go => go.name).ToArray()));

        // 
        // Find all game objects directly under a particular parent.
        //
        var gameObjectsUnderParent = sceneQuery.SelectAll("Parent/Cube");
        Debug.Log("All game objects under a particular parent: " + string.Join(", ", gameObjectsUnderParent.Select(go => go.name).ToArray()));

        // 
        // Find all nested game objects somewhere under a particular parent.
        //
        var gameObjectsNestedUnderParent = sceneQuery.SelectAll("Parent > Cube");
        Debug.Log("All game objects nested somewhere under a particular parent: " + string.Join(", ", gameObjectsNestedUnderParent.Select(go => go.name).ToArray()));

		// 
        // Find all objects by name and layer.
        //
        var gameObjectsByNameAndLayer = sceneQuery.SelectAll("Cube.MyTestLayer");
        Debug.Log("Found game objects by name and layer: " + string.Join(", ", gameObjectsByNameAndLayer.Select(go => go.name).ToArray()));

        // 
        // Use quotes to find games object with spaces in the name.
        //
        var gameObjectWithSpacesInName = sceneQuery.SelectOne("\"Something with a space in the name\"");
        Debug.Log("Used quotes to find a game object with spaces in a name: " + gameObjectWithSpacesInName.name);
    }
    
}
