﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Reflection;

namespace EditorFormulas 
{
	public static class Utils {

		public static Type GetTypeFromAssembly(string typeName, string assemblyName)
		{
			Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
			foreach(Assembly assembly in assemblies) {
				if(! (assembly.FullName.StartsWith(assemblyName))) { continue; }
				Type[] types = assembly.GetTypes();
				foreach(Type type in types) {
					//check for inline classes using +
					if(type.FullName.Equals(typeName, StringComparison.CurrentCultureIgnoreCase) || type.FullName.Contains('+' + typeName))
					{
						return type;
					}
				}
			}
			return null;
		}

		public static MethodInfo[] GetAllFormulaMethods()
		{
			var editorFormulasType = Utils.GetTypeFromAssembly("EditorFormulas.Formulas", "Assembly-CSharp-Editor");
			if(editorFormulasType == null)
			{
				return new MethodInfo[]{};
			}
			var methods = editorFormulasType.GetMethods(BindingFlags.Static | BindingFlags.Public);
			return methods;
		}

		public static MethodInfo GetFormulaMethod(string name)
		{
			var editorFormulasType = Utils.GetTypeFromAssembly("EditorFormulas.Formulas", "Assembly-CSharp-Editor");
			if(editorFormulasType == null)
			{
				return null;
			}
			var method = editorFormulasType.GetMethod(name, BindingFlags.Static | BindingFlags.Public);
			return method;
		}

		public static string GetFullPathFromAssetsPath(string assetsPath)
		{
			var assetsDirectory = new DirectoryInfo(Application.dataPath);
			var fullPath = new DirectoryInfo(Path.Combine(assetsDirectory.Parent.FullName, assetsPath)).FullName;
			return fullPath;
		}

		public static LayerMask LayerMaskField( string label, LayerMask layerMask) {
			var layers = new string[32];
			var layerNumbers = new int[32];

			for (int i = 0; i < 32; i++) {
				string layerName = LayerMask.LayerToName(i);
				layers[i] = layerName;
				layerNumbers[i] = i;
			}
			int maskWithoutEmpty = 0;
			for (int i = 0; i < layerNumbers.Length; i++) {
				if (((1 << layerNumbers[i]) & layerMask.value) > 0)
					maskWithoutEmpty |= (1 << i);
			}
			maskWithoutEmpty = UnityEditor.EditorGUILayout.MaskField( label, maskWithoutEmpty, layers);
			int mask = 0;
			for (int i = 0; i < layerNumbers.Length; i++) {
				if ((maskWithoutEmpty & (1 << i)) > 0)
					mask |= (1 << layerNumbers[i]);
			}
			layerMask.value = mask;
			return layerMask;
		}
	}
}