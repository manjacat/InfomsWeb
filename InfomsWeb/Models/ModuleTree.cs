using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace InfomsWeb.Models
{
    [Serializable]
    public class ModuleTree
    {
        public ModuleTree()
        {

        }

        public ModuleTree(string name, Uri url, int Id, int order = 1)
        {
            ID = Id;
            Name = name;
            Url = url;
            Order = order;
        }

        private readonly SortedList<int, ModuleTree> childrens = new SortedList<int, ModuleTree>();

        public int ID { get; }

        public string Name { get; }

        public Uri Url { get; }

        public int Order { get; }

        public IEnumerable<ModuleTree> Child => childrens.Values;

        public void AddChild(ModuleTree child)
        {
            childrens.Add(child.Order, child);
        }

        private static ModuleTree BuildTree(IEnumerable<ModuleRPS> results)
        {
            // collect nodes
            var nodes = results.ToDictionary(k => k.ID,
                v => new ModuleTree(v.Name, new Uri(v.LinkURL, UriKind.Relative), v.ID, v.SortId));

            // build tree
            var root = new ModuleTree("root", new Uri("#", UriKind.Relative), 0);

            foreach (var result in results)
            {
                var node = nodes[result.ID];
                var parentNode = result.ParentId == 0 ? root : nodes[result.ParentId];

                parentNode.AddChild(node);
            }

            return root;
        }

        public static ModuleTree BuildTree()
        {
            return BuildTree(ModuleRPS.GetListFromDataTable());
        }
    }
}