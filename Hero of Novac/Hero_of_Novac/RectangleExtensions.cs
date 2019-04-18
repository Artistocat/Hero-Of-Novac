using System;
using Microsoft.Xna.Framework;

namespace Hero_of_Novac
{
    static class RectangleExtensions
    {
        public static Vector2 GetIntersectionDepth(this Rectangle recA, Rectangle recB)
        {
            float halfWidthA = recA.Width / 2.0f;
            float halfHeightA = recA.Height / 2.0f;
            float halfWidthB = recB.Width / 2.0f;
            float halfHeightB = recB.Height / 2.0f;

            Vector2 centerA = new Vector2(recA.Left + halfWidthA, recA.Top + halfHeightA);
            Vector2 centerB = new Vector2(recB.Left + halfWidthB, recB.Top + halfHeightB);

            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }
    }
}
