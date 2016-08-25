using System;
using System.Collections;
using Microsoft.SharePoint;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SPSite site = new SPSite("http://win-jptu8ed09qi/sites/JSDevPub"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    // Create a sortable list of content types.
                    ArrayList list = new ArrayList();
                    foreach (SPContentType ct in web.AvailableContentTypes)
                        list.Add(ct);

                    // Sort the list on group name.
                    list.Sort(new CTComparer());

                    // Print a report.
                    Console.WriteLine("{0,-35} {1,-12} {2}", "Site Content Type", "Parent", "Content Type ID");

                    for (int i = 0; i < list.Count; i++)
                    {
                        SPContentType ct = (SPContentType)list[i];

                        if (i == 0 || ((SPContentType)list[i - 1]).Group != ct.Group)
                        {
                            Console.WriteLine("\n{0}", ct.Group);
                            Console.WriteLine("------------------------");

                        }

                        Console.WriteLine("{0,-35} {1,-12} {2}", ct.Name, ct.Parent.Name, ct.Id);
                    }
                }
            }

            Console.Write("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }

    // Implements the Compare method from the IComparer interface.
    // Compares two content type objects by group name, then by content type Id.
    class CTComparer : IComparer
    {
        // The implementation of the Compare method.
        int IComparer.Compare(object x, object y)
        {
            SPContentType ct1 = (SPContentType)x;
            SPContentType ct2 = (SPContentType)y;

            // First compare group names.
            int result = string.Compare(ct1.Group, ct2.Group);
            if (result != 0)
                return result;

            // If the names are the same, compare IDs.
            return ct1.Id.CompareTo(ct2.Id);
        }
    }
}
