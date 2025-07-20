using System;
using Bts.Models;

namespace Bts.Models;

public class BugDependency
{
    public int ParentBugId { get; set; }   // The "blocker" bug
    public Bug ParentBug { get; set; }

    public int ChildBugId { get; set; }    // The "dependent" bug
    public Bug ChildBug { get; set; }
}
