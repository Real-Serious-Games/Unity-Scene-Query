# Unity Scene Query

A library to traverse and query the [Unity](http://en.wikipedia.org/wiki/Unity_(game_engine)) scene to find particular game objects.

A query language is used to identify game objects, it looks something similar to [CSS selectors](http://en.wikipedia.org/wiki/Cascading_Style_Sheets#Selector).


## Setup in Unity

Include the DLL or source code in your Unity project.

Include the namespace in your code:

	using RSG.Scene.Query;

Instantiate *SceneTraversal* and/or *SceneQuery* depending on what you want to use:

	var sceneTraversal = new SceneTraversal();

	var sceneQuery = new SceneQuery();


## Scene Traversal

Enumerate root objects in the [hierarchy](http://docs.unity3d.com/Manual/Hierarchy.html):

	foreach (var gameObject in sceneTraversal.RootNodes())
	{
		// ...
	}

Enumerate all objects using a [pre-order tree traveral](http://en.wikipedia.org/wiki/Tree_traversal#Pre-order): 

	foreach (var gameObject in sceneTraversal.PreOrderHierarchy())
	{
		// ...
	}

There are also functions for [bread-first](http://en.wikipedia.org/wiki/Tree_traversal#Breadth-first) (`BreadthFirst`), [post-order](http://en.wikipedia.org/wiki/Tree_traversal#Post-order) (`PostOrderHierarchy`) and just leaf nodes (`HierarchyLeafNodes`).


Enumerate children of a particular game object:

	GameObject someGameObject = ...
	foreach (var childGameObject in sceneTraversal.Children(someGameObject)) 
	{
		// ..
	}

Enumerate all descendents (children, grand-children, etc) of a particular game object:

	GameObject someGameObject = ...
	foreach (var descendentGameObject in sceneTraversal.Descendents(someGameObject)) 
	{
		// ..
	}

Enumerate all ancestors (parent, grand-parent, etc) of a particular game object:

	GameObject someGameObject = ...
	foreach (var ancestorGameObject in sceneTraversal.Ancestors(someGameObject)) 
	{
		// ..
	}


## Game Object Selectors

The query language allows you to identify the game objects to find.

If you are a language nerd please see the EBNF(-ish) grammar at the of the readme, otherwise I'll try and explain it in simpler terms here.

Game object(s) can be queried by name simply by specifying the name, for example to query for all objects named *pickup-truck* use:

	pickup-truck

Note that the queries are case-insensitive. 

A question mark activates the regular expression matching. You can use this to for partial name matching, for example to query for all objects that contain *truck*:

	?truck

The question mark is much more powerful than just partial name matching. It can match using the full power of [.NET regular expressions](https://msdn.microsoft.com/en-us/library/hs600312(v=vs.110).aspx). For example to patch all game objects whose names start with *pickup* and end in *truck* (with anything in between):

	?^pickup.*truck$

A leading slash matches objects that are at the root of the [hierarchy](http://docs.unity3d.com/Manual/Hierarchy.html), for example to query for a game object *pickup-truck* that is a root object: 

	/pickup-truck

Slashes can also be used, like a file-system path, to specify a path through the Unity hierarchy to particular game object(s), for instance to find all game objects named *pickup-truck* that are under *active* which is under *vehicles*:

	/vehicles/active/pickup-truck

The greater-than symbol can be used in place of slashes to find particular game object(s) that are nested *somewhere* under another game objects, for example to find game objects named *pickup-truck* anywhere in the hierarchy under objects called *vehicles*:

	vehicles>pickup-truck

Game object(s) can be by Unity [layer](http://docs.unity3d.com/Manual/Layers.html) or [tag](http://docs.unity3d.com/Manual/Tags.html) by placing a fullstop before the layer/tag name:

	.vehicle

Layers and tags can be combined for a more restrictive query:

	.vehicle.driveable

The game object name can be also be combined with layers and tags for an even more restrictive query:

	pickup-truck.vehicle.driveable

An exclamation mark can be added to invert the query, for example this will query for anything that is not a vehicle:

	!.vehicle

A hash character can be used to query for a single object by unique-id, this could be more useful but Unity [game object IDs](http://docs.unity3d.com/ScriptReference/Object.GetInstanceID.html) seem to change arbitrarily when you aren't expecting it: 

	#543253


## Query for Single Game Object

*SelectOne* returns the first game object that matches your specified selector.

Example of getting a game object by name:

	var myTruck = sceneQuery.SelectOne("pickup-truck");
	if (myTruck != null) 
	{
		// found it!
	}


## Query for Multiple Game Objects

*SelectAll* is used to enumerate the collection of game objects that matches your specified selector. 

Example of getting a game object by name and layers:

	var myTrucks = sceneQuery.SelectAll("pickup-truck.vehicle.driveable");
 	foreach (var truck in myTrucks)
    {
		// got a truck!
	}


## EBNF  

The grammar for the query language specified in [EBNF(-ish)](http://en.wikipedia.org/wiki/Extended_Backus%E2%80%93Naur_Form) format.

	 query 
	   = descendents_selector
	   ;
	
	 descendents_selector
	   = ['/'] compound_selector { ('/' | '>') compound_selector }
	   ;
	
	 compound_selector
	   = selector { selector }
	   ;
	
	 selector
	   = '.' matcher            -> Match by layer or tag.
	   | UNIQUE_ID              -> Match by unique id.
	   | '!' selector           -> Invert query and matching everything except...
	   |  matcher               -> Match a game object.
	   ;
	
	 matcher
	   = name                   -> Match exact name.
	   | '?' name               -> Match partial name/regular expression.
	   ;   
	
	 name
	   = character_sequence
	   | quoted_character_sequence
	   ;
	
	 quoted_character_sequence
	   = '"' character_sequence_with_spaces '"'
	   ;
	
	 UNIQUE_ID
	   = '#' character_sequence
	   ;
	
