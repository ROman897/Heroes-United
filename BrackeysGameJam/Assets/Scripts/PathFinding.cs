using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    const float sqrt_diagonal = 1.41421356237f;

    class Node : IComparable {
        public Vector2Int tile_coords;
        public float passed_dist;
        public float estimate_dist;

        public int CompareTo(object obj) {
            if (obj == null) {
                return 1;
            }
            Node other_node = obj as Node;
            return (passed_dist + estimate_dist).CompareTo(other_node.passed_dist + other_node.estimate_dist);
        }

        public Node(Vector2Int tile_coords, float passed_dist, float estimate_dist) {
            this.tile_coords = tile_coords;
            this.passed_dist = passed_dist;
            this.estimate_dist = estimate_dist;
        }
    }

    private static PathFinding instance;

    public static PathFinding singleton() {
        init_instance();
        return instance;
    }

    private static void init_instance() {
        if (instance == null) {
            instance = new PathFinding();
        }
    }

    private float get_estimate_dist(Vector2Int coords, Vector2Int dest_coords) {
        int x_dif = Mathf.Abs(coords.x - dest_coords.x);
        int y_dif = Mathf.Abs(coords.y - dest_coords.y);

        int shorter_coord = Mathf.Min(x_dif, y_dif);
        return shorter_coord * sqrt_diagonal + x_dif + y_dif - 2 * shorter_coord;
    }

    public List<Vector2> find_path(Vector2 start_pos, Vector2 dest_pos) {
        Vector2Int start_coords = World.singleton().world_to_coord(start_pos);
        Vector2Int dest_coords = World.singleton().world_to_coord(dest_pos);
        IndexedPriorityQueue<Node> q = new IndexedPriorityQueue<Node>(1000);
        Dictionary<Vector2Int, int> q_indices = new Dictionary<Vector2Int, int>();
        HashSet<Vector2Int> done_tiles = new HashSet<Vector2Int>();

        Dictionary<Vector2Int, Vector2Int> predecessors = new Dictionary<Vector2Int, Vector2Int>();

        q.Insert(0, new Node(start_coords, 0, 0)); 
        q_indices[start_coords] = 0;

        int next_index = 1;

        while (q.Count > 0) {
            Node node = q.Top();
            q.Pop();
            if (node.tile_coords == dest_coords) {
                List<Vector2> res = new List<Vector2>();
                Vector2Int iter_coords = dest_coords; 
                res.Add(dest_pos);
                if (iter_coords != start_coords) {
                    iter_coords = predecessors[iter_coords];
                }

                while (iter_coords != start_coords) {
                    res.Add(World.singleton().coord_to_world(iter_coords));
                    iter_coords = predecessors[iter_coords];
                }
                return res;
            }
            done_tiles.Add(node.tile_coords);

            for (int y_dif =  - 1; y_dif <= 1; ++y_dif) {
                for (int x_dif = - 1; x_dif <= 1; ++x_dif) {
                    Vector2Int new_tile_coords = new Vector2Int(node.tile_coords.x + x_dif, node.tile_coords.y + y_dif);
                    if (done_tiles.Contains(new_tile_coords)) {
                        continue;
                    }

                    if (x_dif == 0 || y_dif == 0) {
                        if (!World.singleton().is_tile_passable(new_tile_coords)) {
                            continue;
                        }
                    } else {
                        Vector2Int x_inc = new Vector2Int(x_dif, 0);
                        Vector2Int y_inc = new Vector2Int(0, y_dif);
                        if (!World.singleton().is_tile_passable(node.tile_coords + x_inc)) {
                            continue;
                        }
                        if (!World.singleton().is_tile_passable(node.tile_coords + y_inc)) {
                            continue;
                        }
                        if (!World.singleton().is_tile_passable(new_tile_coords)) {
                            continue;
                        }
                    }

                    float inc = 1;
                    if (y_dif != 0 && x_dif != 0) {
                        inc = sqrt_diagonal;
                    }

                    int index;
                    float passed_dist = node.passed_dist + inc;
                    if (q_indices.TryGetValue(new_tile_coords, out index)) {
                        Node old_node = q[index];
                        if (passed_dist < old_node.passed_dist) {
                            q.DecreaseIndex(index, new Node(new_tile_coords, passed_dist, old_node.estimate_dist));
                            predecessors[new_tile_coords] = node.tile_coords;
                        }
                    } else {
                        q.Insert(next_index, new Node(new_tile_coords, passed_dist, get_estimate_dist(new_tile_coords, dest_coords))); 
                        q_indices[new_tile_coords] = next_index;
                        predecessors[new_tile_coords] = node.tile_coords;
                        ++next_index;
                    }
                }
            }
        }
        return null;
    }
}
