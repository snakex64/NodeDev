﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeDev.Core.Class
{
    public class NodeClass
    {

		public record class SerializedNodeClass(string Name, string Namespace, List<string> Methods, List<string> Properties);

		public readonly Project Project;

        public string Name { get; set; }

		public string Namespace { get; set; }

        public List<NodeClassMethod> Methods { get; } = new();

        public List<NodeClassProperty> Properties { get; } = new();

		public NodeClass(string name, string @namespace, Project project)
		{
			Name = name;
			Namespace = @namespace;
			Project = project;
		}


        public static NodeClass Deserialize(string serialized, Project project)
        {
            var serializedNodeClass = System.Text.Json.JsonSerializer.Deserialize<SerializedNodeClass>(serialized) ?? throw new Exception("Unable to deserialize node class");
           
            var nodeClass = new NodeClass(serializedNodeClass.Name, serializedNodeClass.Namespace, project);

			foreach (var method in serializedNodeClass.Methods)
				nodeClass.Methods.Add(NodeClassMethod.Deserialize(nodeClass, method));

			foreach (var property in serializedNodeClass.Properties)
				nodeClass.Properties.Add(NodeClassProperty.Deserialize(nodeClass, property));

			return nodeClass;
        }

        public string Serialize()
        {
            var serializedNodeClass = new SerializedNodeClass(Name, Namespace, Methods.Select(x => x.Serialize()).ToList(), Properties.Select( x=> x.Serialize()).ToList());

            return System.Text.Json.JsonSerializer.Serialize(serializedNodeClass);
        }
    }
}
