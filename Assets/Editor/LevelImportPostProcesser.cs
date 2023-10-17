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

            bool chk = g.name.Contains("chk_");
            bool win = g.name.Contains("win_");
            bool die = g.name.Contains("die_");
            bool spr = g.name.Contains("spr_");
            bool col = g.name.Contains("col_");

            bool solid = g.name.Contains("solid_");
            bool player = g.name.Contains("player_");
            bool platform = g.name.Contains("platform_");

            if (win || die || chk || spr || col)
            {
                var mc = g.AddComponent<MeshCollider>();
                if (!col)
                {
                    mc.convex = true;
                    mc.isTrigger = true;
                }

                if(!spr)
                {
                    GameObject.DestroyImmediate(g.GetComponent<MeshRenderer>());
                    GameObject.DestroyImmediate(g.GetComponent<MeshFilter>());
                }
            }



            if (player) g.AddComponent<Player.Character>();
            if (platform) g.AddComponent<Platforms.Platform>();
            if (solid) g.AddComponent<MeshCollider>();
            if (chk) g.AddComponent<Triggers.CheckPoint>();
            if (win) g.AddComponent<Triggers.Win>();
            if (die) g.AddComponent<Triggers.Die>();
            if (spr) g.AddComponent<Triggers.Spring>();
        }

    }
}