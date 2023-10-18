using UnityEngine;
using UnityEditor;

namespace Core.Editor
{
    public class LevelPostprocessor : AssetPostprocessor
    {
        void OnPostprocessMeshHierarchy(GameObject g)
        {
            foreach (Transform child in g.transform)
            {
                OnPostprocessMeshHierarchy(child.gameObject);
            }

            bool checkPoint = g.name.Contains("chk_");
            bool win = g.name.Contains("win_");
            bool die = g.name.Contains("die_");
            bool spring = g.name.Contains("spr_");
            bool collider = g.name.Contains("col_");
            bool trap = g.name.Contains("trp_");

            bool solid = g.name.Contains("solid_");
            bool player = g.name.Contains("player_");
            bool platform = g.name.Contains("platform_");

            if (win || die || checkPoint || spring || collider || trap)
            {
                var mc = g.AddComponent<MeshCollider>();
                if (!collider)
                {
                    mc.convex = true;
                    mc.isTrigger = true;
                }

                if (checkPoint || win || trap)
                {
                    GameObject.DestroyImmediate(g.GetComponent<MeshRenderer>());
                    GameObject.DestroyImmediate(g.GetComponent<MeshFilter>());
                }
            }

            if (player) g.AddComponent<Player.Character>();
            if (platform) g.AddComponent<Platforms.Platform>();
            if (solid) g.AddComponent<MeshCollider>();
            if (checkPoint) g.AddComponent<Triggers.CheckPoint>();
            if (win) g.AddComponent<Triggers.Win>();
            if (die) g.AddComponent<Triggers.Die>();
            if (spring) g.AddComponent<Triggers.Spring>();
            if (trap) g.AddComponent<Triggers.Trap>();

        }

    }
}