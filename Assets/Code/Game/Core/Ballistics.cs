using UnityEngine;
using System;

namespace Warships
{
    public static class Ballistics
    {

        // Note, doesn't take drag into account.

        /// <summary>
        /// Calculate the lanch angle.
        /// </summary>
        /// <returns>Angle to be fired on.</returns>
        /// <param name="start">The muzzle.</param>
        /// <param name="end">Wanted hit point.</param>
        /// <param name="muzzleVelocity">Muzzle velocity.</param>
        public static bool CalculateTrajectory(Vector3 start, Vector3 end, float muzzleVelocity, out float angle)
        {

            Vector3 targetVector = end - start;
            float vSqr = muzzleVelocity * muzzleVelocity;
            float y = targetVector.y;   //Vertical distance
            targetVector.y = 0.0f;
            float x = targetVector.sqrMagnitude;    // Horizontal distance
            float g = -Physics.gravity.y;

            float underRoot = vSqr * vSqr - g * (g * x + 2.0f * y * vSqr);


            if (underRoot < 0.0f)
            {

                //target out of range.
                angle = -45.0f;
                //highAngle = -45.0f;
                return false;
            }


            angle = -Mathf.Atan2(g * Mathf.Sqrt(x), vSqr + Mathf.Sqrt(underRoot)) * Mathf.Rad2Deg;
            return true;

        }

        /// <summary>
        /// Gets the ballistic path.
        /// </summary>
        /// <returns>The ballistic path.</returns>
        /// <param name="startPos">Start position.</param>
        /// <param name="forward">Forward direction.</param>
        /// <param name="velocity">Velocity.</param>
        /// <param name="timeResolution">Time from frame to frame.</param>
        /// <param name="maxTime">Max time to simulate, will be clamped to reach height 0 (aprox.).</param>
        public static Vector3[] GetBallisticPath(Vector3 startPos, Vector3 forward, float velocity, float timeResolution, float maxTime = Mathf.Infinity)
        {

            maxTime = Mathf.Min(maxTime, Ballistics.GetTimeOfFlight(velocity, Vector3.Angle(forward, Vector3.up) * Mathf.Deg2Rad));
            Vector3[] positions = new Vector3[Mathf.CeilToInt(maxTime / timeResolution)];
            Vector3 velVector = forward * velocity;
            int index = 0;
            Vector3 curPosition = startPos;
            for (float t = 0.0f; t < maxTime; t += timeResolution)
            {

                if (index >= positions.Length)
                    break;//rounding error using certain values for maxTime and timeResolution

                positions[index] = curPosition;
                curPosition += velVector * timeResolution;
                velVector += Physics.gravity * timeResolution;
                index++;
            }
            return positions;
        }

        /// <summary>
        /// Gets maximum time of flight in seconds.
        /// </summary>
        public static float GetTimeOfFlight(float vel, float angle)
        {
            return (2.0f * vel * Mathf.Sin(angle)) / -Physics.gravity.y;
        }

        /// <summary>
        /// Simplified range calculation which does not account for gun height and shell air drag.
        /// </summary>
        /// <param name="projectileVelocity">Initial velocity of projectiles</param>
        /// <param name="angle">Angle of barrels in radians</param>
        /// <returns>Range in units (meters)</returns>
        public static float GetRange(float projectileVelocity, float angle)
        {
            return Mathf.Sin(2 * angle) * Mathf.Pow(projectileVelocity, 2) / -Physics.gravity.y;
        }

        /// <summary>
        /// Simplified range calculation which does not account for gun height and shell air drag.
        /// </summary>
        /// <param name="projectileVelocity">Initial velocity of projectiles</param>
        /// <param name="angle">Angle of barrels in radians</param>
        /// <returns>Range in units (meters)</returns>
        public static float GetRange(float projectileVelocity, float angle, float height)
        {
            var sqrt = Mathf.Sqrt(Mathf.Pow(projectileVelocity, 2) * Mathf.Sin(angle) + 2 * Physics.gravity.y * height);
            return projectileVelocity * Mathf.Cos(angle) / Physics.gravity.y * (projectileVelocity * Mathf.Sin(angle) + sqrt / Physics.gravity.y);
        }

        /// <summary>
        /// Simplified elevation calculation method which does not account for gun height and shell air drag.
        /// </summary>
        /// <param name="projectileVelocity">Initial velocity of projectiles</param>
        /// <returns>Angle in radians</returns>
        public static float GetElevation(Vector3 start, Vector3 end, float projectileVelocity)
        {
            Vector3 dir = end - start;
            float vSqr = projectileVelocity * projectileVelocity;
            float y = dir.y;
            dir.y = 0.0f;
            float x = dir.sqrMagnitude;
            float g = -Physics.gravity.y;

            float uRoot = vSqr * vSqr - g * (g * (x) + (2.0f * y * vSqr));

            if (uRoot < 0.0f)
            {
                return 0;
            }

            return -Mathf.Atan2(g * Mathf.Sqrt(x), vSqr + Mathf.Sqrt(uRoot));
        }



        public static float GetTime(Vector3 direction, float projectileVelocity)
        {
            float deltaAngle = 90 - Vector3.Angle(direction, Vector3.up);
            return GetTimeOfFlight(projectileVelocity, deltaAngle * Mathf.Deg2Rad);
        }

        public static Vector3 GetDirection(Vector3 start, Vector3 end, float offset, float projectileVelocity)
        {


            Vector3 dir = end - start;

            //dir += dir.normalized * offset;

            float vSqr = projectileVelocity * projectileVelocity;
            float y = dir.y;

            float x = dir.sqrMagnitude + offset;
            float g = -Physics.gravity.y;

            float uRoot = vSqr * vSqr - g * (g * (x) + (2.0f * y * vSqr));

            if (uRoot < 0.0f)
            {
                return dir.normalized;
            }

            float xE = g * Mathf.Sqrt(x);
            float yE = vSqr + Mathf.Sqrt(uRoot);
            Vector3 elevation = dir.normalized * yE + Vector3.up * xE;

            return elevation.normalized; //-Mathf.Atan2(g * Mathf.Sqrt(x), vSqr + Mathf.Sqrt(uRoot));
        }

        /// <summary>
        /// Displays the trajectory path with a line renderer
        /// </summary>
        public static void DrawTrajectoryPath(float projectileVelocity, Transform barrelTransform, LineRenderer lineRenderer)
        {
            float step = Time.deltaTime;
            //How long did it take to hit the target?
            float timeToHitTarget = GetFlightTime(projectileVelocity, barrelTransform, step);

            //How many segments we will have
            int maxIndex = Mathf.RoundToInt(timeToHitTarget / step);

            lineRenderer.positionCount = maxIndex;

            //Start values
            Vector3 currentVelocity = barrelTransform.forward * projectileVelocity;
            Vector3 currentPosition = barrelTransform.position;

            Vector3 newPosition = Vector3.zero;
            Vector3 newVelocity = Vector3.zero;

            //Build the trajectory line
            for (int index = 0; index < maxIndex; index++)
            {
                lineRenderer.SetPosition(index, currentPosition);

                //Calculate the new position of the bullet
                BackwardEuler(Time.fixedDeltaTime, currentPosition, currentVelocity, out newPosition, out newVelocity);

                currentPosition = newPosition;
                currentVelocity = newVelocity;
            }
        }

        public static void BackwardEuler(float h, Vector3 currentPosition, Vector3 currentVelocity, out Vector3 newPosition, out Vector3 newVelocity)
        {
            //Init acceleration
            //Gravity
            Vector3 acceleartionFactor = Physics.gravity;

            //Main algorithm
            newVelocity = currentVelocity + h * acceleartionFactor;

            newPosition = currentPosition + h * newVelocity;
        }

        /// <summary>
        /// Range calculation with gun height and shell air drag.
        /// </summary>
        /// <param name="projectileVelocity">Initial velocity of projectiles</param>
        /// <param name="angle">Angle of barrels in degrees</param>
        /// <param name="initialHeight">Barrel height in units (meters)</param>
        /// <param name="drag">https://docs.unity3d.com/Manual/class-Rigidbody.html</param>
        /// <returns>Range in units (meters)</returns>
        public static float GetRange(float projectileVelocity, float angle, float initialHeight, float drag)
        {
            return 0;
        }

        /// <summary>
        /// Elevation calculation method with height and airdrag
        /// </summary>
        /// <param name="projectileVelocity">Initial velocity of projectiles</param>
        /// <param name="range">Expected range at which the guns are aimed</param>
        /// <param name="initialHeight">Barrel height in units (meters)</param>
        /// <param name="drag">https://docs.unity3d.com/Manual/class-Rigidbody.html</param>
        /// <returns>Angle in degrees</returns>
        public static float GetElevation(float projectileVelocity, float range, float initialHeight, float drag)
        {
            return 0;
        }

        /// <summary>
        /// Computes time of flight in seconds
        /// </summary>
        /// <param name="projectileVelocity">Initial velocity of projectiles</param>
        /// <param name="range">Expected range at which the guns are aimed</param>
        /// <param name="initialHeight">Barrel height in units (meters)</param>
        /// <param name="drag">https://docs.unity3d.com/Manual/class-Rigidbody.html</param>
        /// <returns>Flight time in seconds</returns>
        public static float GetFlightTime(float projectileVelocity, Transform barrelTransform, float step)
        {
            //Init values
            Vector3 currentVelocity = barrelTransform.forward * projectileVelocity;
            Vector3 currentPosition = barrelTransform.position;

            Vector3 newPosition = Vector3.zero;
            Vector3 newVelocity = Vector3.zero;

            //The total time it will take before we hit the target
            float time = 0f;

            //Limit to 30 seconds to avoid infinite loop if we never reach the target
            for (time = 0f; time < 30f; time += step)
            {
                BackwardEuler(step, currentPosition, currentVelocity, out newPosition, out newVelocity);

                //If we are moving downwards and are below the target, then we have hit
                if (newPosition.y < currentPosition.y && newPosition.y < 0)
                {
                    //Add 2 times to make sure we end up below the target when we display the path
                    time += step * 2f;

                    break;
                }

                currentPosition = newPosition;
                currentVelocity = newVelocity;
            }

            return time;
        }
    }
}
