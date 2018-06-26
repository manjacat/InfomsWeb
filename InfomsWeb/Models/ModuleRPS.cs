using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfomsWeb.Models
{
    [Serializable]
    public class ModuleRPS
    {
        public ModuleRPS(string name, Uri url, int order = 1)
        {
            Name = name;
            Url = url;
            Order = order;
        }

        private readonly SortedList<int, ModuleRPS> childrens = new SortedList<int, ModuleRPS>();

        public string Name { get; }

        public Uri Url { get; }

        public int Order { get; }

        public IEnumerable<ModuleRPS> Child => childrens.Values;

        public void AddChild(ModuleRPS child)
        {
            childrens.Add(child.Order, child);
        }

        private static ModuleRPS BuildTree(IEnumerable<ModuleRPSTable> results)
        {
            // collect nodes

            var nodes = results.ToDictionary(k => k.ID,
                v => new ModuleRPS(v.Name, new Uri(v.LinkURL, UriKind.Relative), v.SortId));

            // build tree

            var root = new ModuleRPS("root", new Uri("#", UriKind.Relative));

            foreach (var result in results)
            {
                var node = nodes[result.ID];
                var parentNode = result.ParentId == 0 ? root : nodes[result.ParentId];

                parentNode.AddChild(node);
            }

            return root;
        }

        public static ModuleRPS BuildTree()
        {
            return BuildTree(ModuleRPSTable.GetListFromDataTable());
        }
    }
}