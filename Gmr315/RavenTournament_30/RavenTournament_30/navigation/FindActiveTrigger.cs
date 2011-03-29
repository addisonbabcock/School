using Common.Game;
using Common.Graph;
using Common.Triggers;

namespace Raven.navigation
{
    public class FindActiveTrigger : ITerminatingCondition
    {


        public static bool isSatisfied(SparseGraph graph, int target, int CurrentNodeIdx)
        {
            bool bSatisfied = false;

            //get a reference to the node at the given node index
            NavGraphNode node = graph.GetNode(CurrentNodeIdx);

            //if the extrainfo field is pointing to a giver-trigger, test to make sure 
            //it is active and that it is of the correct type.
            if (node.ExtraInfo() != null)
            {
                var extra = (Trigger<AbstractBot>) node.ExtraInfo();
                if (extra.isActive() &&
                    (extra.EntityType() == target))
                {
                    bSatisfied = true;
                }
            }
            return bSatisfied;
        }

        bool ITerminatingCondition.isSatisfied(SparseGraph graph, int target, int CurrentNodeIdx)
        {
            return isSatisfied(graph, target, CurrentNodeIdx);
        }
    }
}