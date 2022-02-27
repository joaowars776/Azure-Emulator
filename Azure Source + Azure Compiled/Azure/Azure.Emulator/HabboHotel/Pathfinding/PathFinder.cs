using Azure.HabboHotel.Rooms;
using System;
using System.Collections.Generic;

namespace Azure.HabboHotel.PathFinding
{
    /// <summary>
    /// Class PathFinder.
    /// </summary>
    internal class PathFinder
    {
        /// <summary>
        /// The diag move points
        /// </summary>
        public static Vector2D[] DiagMovePoints =
        {
            new Vector2D(-1, -1),
            new Vector2D(0, -1),
            new Vector2D(1, -1),
            new Vector2D(1, 0),
            new Vector2D(1, 1),
            new Vector2D(0, 1),
            new Vector2D(-1, 1),
            new Vector2D(-1, 0)
        };

        /// <summary>
        /// The no diag move points
        /// </summary>
        public static Vector2D[] NoDiagMovePoints =
        {
            new Vector2D(0, -1),
            new Vector2D(1, 0),
            new Vector2D(0, 1),
            new Vector2D(-1, 0)
        };

        /// <summary>
        /// Finds the path.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="diag">if set to <c>true</c> [diag].</param>
        /// <param name="map">The map.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>List&lt;Vector2D&gt;.</returns>
        public static List<Vector2D> FindPath(RoomUser user, bool diag, Gamemap map, Vector2D start, Vector2D end)
        {
            var list = new List<Vector2D>();
            var pathFinderNode = FindPathReversed(user, diag, map, start, end);
            if (pathFinderNode == null)
                return list;
            list.Add(end);
            while (pathFinderNode.Next != null)
            {
                list.Add(pathFinderNode.Next.Position);
                pathFinderNode = pathFinderNode.Next;
            }
            return list;
        }

        /// <summary>
        /// Finds the path reversed.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="diag">if set to <c>true</c> [diag].</param>
        /// <param name="map">The map.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>PathFinderNode.</returns>
        public static PathFinderNode FindPathReversed(RoomUser user, bool diag, Gamemap map, Vector2D start,
            Vector2D end)
        {
            var minHeap = new MinHeap<PathFinderNode>(256);
            var array = new PathFinderNode[map.Model.MapSizeX, map.Model.MapSizeY];
            var pathFinderNode = new PathFinderNode(start) {Cost = 0};
            var breadcrumb = new PathFinderNode(end);
            array[pathFinderNode.Position.X, pathFinderNode.Position.Y] = pathFinderNode;
            minHeap.Add(pathFinderNode);
            while (minHeap.Count > 0)
            {
                pathFinderNode = minHeap.ExtractFirst();
                pathFinderNode.InClosed = true;
                var num = 0;
                while (diag ? (num < DiagMovePoints.Length) : (num < NoDiagMovePoints.Length))
                {
                    var vector2D = pathFinderNode.Position + (diag ? DiagMovePoints[num] : NoDiagMovePoints[num]);
                    var endOfPath = vector2D.X == end.X && vector2D.Y == end.Y;
                    if (map.IsValidStep(user, new Vector2D(pathFinderNode.Position.X, pathFinderNode.Position.Y),
                        vector2D, endOfPath, user.AllowOverride))
                    {
                        PathFinderNode pathFinderNode2;
                        if (array[vector2D.X, vector2D.Y] == null)
                        {
                            pathFinderNode2 = new PathFinderNode(vector2D);
                            array[vector2D.X, vector2D.Y] = pathFinderNode2;
                        }
                        else pathFinderNode2 = array[vector2D.X, vector2D.Y];
                        if (!pathFinderNode2.InClosed)
                        {
                            var num2 = 0;
                            if (pathFinderNode.Position.X != pathFinderNode2.Position.X) num2++;
                            if (pathFinderNode.Position.Y != pathFinderNode2.Position.Y) num2++;
                            var num3 = pathFinderNode.Cost + num2 + pathFinderNode2.Position.GetDistanceSquared(end);
                            if (num3 < pathFinderNode2.Cost)
                            {
                                pathFinderNode2.Cost = num3;
                                pathFinderNode2.Next = pathFinderNode;
                            }
                            if (!pathFinderNode2.InOpen)
                            {
                                if (pathFinderNode2.Equals(breadcrumb))
                                {
                                    pathFinderNode2.Next = pathFinderNode;
                                    return pathFinderNode2;
                                }
                                pathFinderNode2.InOpen = true;
                                minHeap.Add(pathFinderNode2);
                            }
                        }
                    }
                    num++;
                }
            }
            return null;
        }

        /// <summary>
        /// Calculates the rotation.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <returns>System.Int32.</returns>
        internal static int CalculateRotation(int x1, int y1, int x2, int y2)
        {
            var dX = x2 - x1;
            var dY = y2 - y1;

            var d = Math.Atan2(dY, dX) * 180 / Math.PI;
            return ((int)d + 90) / 45;
        }

        /// <summary>
        /// Gets the distance.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        /// <returns>System.Int32.</returns>
        public static int GetDistance(int x, int y, int toX, int toY)
        {
            return Convert.ToInt32(Math.Sqrt(Math.Pow(toX - x, 2) + Math.Pow(toY - y, 2)));
        }
    }
}