using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomRaycastUtility
{
    public static Vector3 RelativeMouseAtScaled(Vector2 position)
    {
#if !UNITY_EDITOR && !UNITY_WSA
            // If the main display is now the same resolution as the system then we need to scale the mouse position. (case 1141732)
            if (Display.main.renderingWidth != Display.main.systemWidth || Display.main.renderingHeight != Display.main.systemHeight)
            {
                // The system will add padding when in full-screen and using a non-native aspect ratio. (case UUM-7893)
                // For example Rendering 1920x1080 with a systeem resolution of 3440x1440 would create black bars on each side that are 330 pixels wide.
                // we need to account for this or it will offset our coordinates when we are not on the main display.
                var systemAspectRatio = Display.main.systemWidth / (float)Display.main.systemHeight;

                var sizePlusPadding = new Vector2(Display.main.renderingWidth, Display.main.renderingHeight);
                var padding = Vector2.zero;
                if (Screen.fullScreen)
                {
                    var aspectRatio = Screen.width / (float)Screen.height;
                    if (Display.main.systemHeight * aspectRatio < Display.main.systemWidth)
                    {
                        // Horizontal padding
                        sizePlusPadding.x = Display.main.renderingHeight * systemAspectRatio;
                        padding.x = (sizePlusPadding.x - Display.main.renderingWidth) * 0.5f;
                    }
                    else
                    {
                        // Vertical padding
                        sizePlusPadding.y = Display.main.renderingWidth / systemAspectRatio;
                        padding.y = (sizePlusPadding.y - Display.main.renderingHeight) * 0.5f;
                    }
                }

                var sizePlusPositivePadding = sizePlusPadding - padding;

                // If we are not inside of the main display then we must adjust the mouse position so it is scaled by
                // the main display and adjusted for any padding that may have been added due to different aspect ratios.
                if (position.y < -padding.y || position.y > sizePlusPositivePadding.y ||
                     position.x < -padding.x || position.x > sizePlusPositivePadding.x)
                {
                    var adjustedPosition = position;

                    if (!Screen.fullScreen)
                    {
                        // When in windowed mode, the window will be centered with the 0,0 coordinate at the top left, we need to adjust so it is relative to the screen instead.
                        adjustedPosition.x -= (Display.main.renderingWidth - Display.main.systemWidth) * 0.5f;
                        adjustedPosition.y -= (Display.main.renderingHeight - Display.main.systemHeight) * 0.5f;
                    }
                    else
                    {
                        // Scale the mouse position to account for the black bars when in a non-native aspect ratio.
                        adjustedPosition += padding;
                        adjustedPosition.x *= Display.main.systemWidth / sizePlusPadding.x;
                        adjustedPosition.y *= Display.main.systemHeight / sizePlusPadding.y;
                    }

                    var relativePos = Display.RelativeMouseAt(adjustedPosition);

                    // If we are not on the main display then return the adjusted position.
                    if (relativePos.z != 0)
                        return relativePos;
                }

                // We are using the main display.
                return new Vector3(position.x, position.y, 0);
            }
#endif
        return Display.RelativeMouseAt(position);
    }
}
