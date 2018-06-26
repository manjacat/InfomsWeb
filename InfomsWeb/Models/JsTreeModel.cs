using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfomsWeb.Models
{
    public class JsTreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public string state { get; set; }
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
        public string li_attr { get; set; }
        public string a_attr { get; set; }

        public static List<JsTreeModel> BuildTree()
        {
            return BuildTree(ModuleRPSTable.GetListFromDataTable());
        }

        private static List<JsTreeModel> BuildTree(IEnumerable<ModuleRPSTable> results)
        {
            List<JsTreeModel> output = new List<Models.JsTreeModel>();

            foreach (var result in results)
            {
                JsTreeModel node = new JsTreeModel
                {
                    id = result.ID.ToString(),
                    parent = result.ParentId == 0 ? "#" : result.ParentId.ToString(),
                    text = result.Name,
                    li_attr = "{ \"opened\" : true }"
                    //a_attr = string.Format("{\"href\",\"{0}\"}", result.LinkURL)
                };
                output.Add(node);
            }

            return output;
        }
    }
}