using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportDocumentationConsole.ViewModels
{
    public class CheckboxList
    {
        public List<CheckboxItem> checkboxList { get; set; }
        public string info { get; set; }
        public CheckboxList()
        {
            checkboxList = new List<CheckboxItem>();
        }
    }

    public class CheckboxItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}