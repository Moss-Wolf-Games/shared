using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace MossWolfGames.Shared.Runtime.Utilities
{
    public static class MathUtility
    {
        public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }

        public static Vector3 GetNearestPointOnLineFinite(Vector3 start, Vector3 end, Vector3 point)
        {
            //Get heading
            Vector3 heading = (end - start);
            float magnitudeMax = heading.magnitude;
            heading.Normalize();

            //Do projection from the point but clamp it
            Vector3 lhs = point - start;
            float dotP = Vector3.Dot(lhs, heading);
            dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
            return start + heading * dotP;
        }

        public static float GetDistanceToLineFinite(Vector3 start, Vector3 end, Vector3 point)
        {
            Vector3 nearestPoint = GetNearestPointOnLineFinite(start, end, point);
            return Vector3.Distance(point, nearestPoint);
        }

        public static float GetDistanceToBezierLine(Vector3 start, Vector3 startAnchorOffset, Vector3 end, Vector3 endAnchorOffset, int segments, Vector3 point)
        {
            float minDistance = float.MaxValue;

            Vector2 startAnchor = start + startAnchorOffset;
            Vector2 endAnchor = end + endAnchorOffset;
            float incrementPerSegment = 1.0f / segments;
            for (int i = 0; i < segments; i++)
            {
                float startT = i * incrementPerSegment;
                float endT = (i + 1) * incrementPerSegment;

                Vector2 bezierStart = MathUtility.CalculateCubicBezierPoint(startT, start, startAnchor, endAnchor, end);
                Vector2 bezierEnd = MathUtility.CalculateCubicBezierPoint(endT, start, startAnchor, endAnchor, end);

                float distance = GetDistanceToLineFinite(bezierStart, bezierEnd, point);
                minDistance = Mathf.Min(minDistance, distance);
            }

            return minDistance;
        }

        public static bool TryGetLineLineIntesection(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            float Ax = end1.x - start1.x;
            float Bx = start2.x - end2.x;

            float x1lo, x1hi;
            if (Ax < 0)
            {
                x1lo = end1.x;
                x1hi = start1.x;
            }
            else
            {
                x1hi = end1.x;
                x1lo = start1.x;
            }

            if (Bx > 0)
            {
                if (x1hi < end2.x || start2.x < x1lo)
                {
                    return false;
                }
            }
            else
            {
                if (x1hi < start2.x || end2.x < x1lo)
                {
                    return false;
                }
            }

            float Ay = end1.y - start1.y;
            float By = start2.y - end2.y;

            float y1lo, y1hi;
            if (Ay < 0)
            {
                y1lo = end1.y;
                y1hi = start1.y;
            }
            else
            {
                y1hi = end1.y;
                y1lo = start1.y;
            }

            if (By > 0)
            {
                if (y1hi < end2.y || start2.y < y1lo)
                {
                    return false;
                }
            }
            else
            {
                if (y1hi < start2.y || end2.y < y1lo)
                {
                    return false;
                }
            }

            float Cx = start1.x - start2.x;
            float Cy = start1.y - start2.y;
            float d = By * Cx - Bx * Cy;
            float f = Ay * Bx - Ax * By;

            if (f > 0)
            {
                if (d < 0 || d > f)
                {
                    return false;
                }
            }
            else
            {
                if (d > 0 || d < f)
                {
                    return false;
                }
            }

            float e = Ax * Cy - Ay * Cx;

            if (f > 0)
            {
                if (e < 0 || e > f)
                {
                    return false;
                }
            }
            else
            {
                if (e > 0 || e < f)
                {
                    return false;
                }
            }

            if (f == 0)
            {
                return false;
            }

            float numX = d * Ax;
            intersection.x = start1.x + numX / f;
            float numY = d * Ay;
            intersection.y = start1.y + numY / f;
            
            return true;
        }
    }
}